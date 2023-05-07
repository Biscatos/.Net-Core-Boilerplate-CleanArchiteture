using Core.Application;
using Core.Application.DTOS.System;
using Core.Application.Exceptions;
using Core.Application.Interfaces.Services.System;
using Core.Application.Wrappers;
using Core.Domain.Entities.System;
using Infrastruture.Identity.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Infrastruture.Identity.Services
{
    internal class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly JWTSettings _jwtSettings;

        private readonly IEmailIntegrationService _emailService;

        private readonly IApplicationLogger _logger;
        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IOptions<JWTSettings> jwOption, IEmailIntegrationService emailService, IApplicationLogger logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _jwtSettings = jwOption.Value;
            _emailService = emailService;
            _logger = logger;
        }



        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationDTO request, string ipAddress)
        {

            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null)
                {
                    throw new ApiException($"Unable to find user with this User Name");
                }

                var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    throw new ApiException($"´The username or password is wrong.");
                }
                if (!user.EmailConfirmed)
                {
                    throw new ApiException($"This user account is not confirmed, please confirm your account by following the link that we sent to your email address.");
                }

                JwtSecurityToken jwtSecurityToken = await BuildJWToken(user, ipAddress);
                AuthenticationResponse response = new AuthenticationResponse();

                response.Id = user.Id;
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.Email = user.Email;
                response.UserName = user.UserName;

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                response.Roles = rolesList.ToList();
                response.IsVerified = user.EmailConfirmed;
                response.Claims = await GetClaimsByRoles(response.Roles);

                //response.Company = !user.Company.HasValue ? default : new CompanyResponseDTO
                //{
                //    Id = user.CompanyId.Value,
                //    Name = user.Company?.Name,
                //    Email = user.Company?.Email,
                //    Address = user.Company?.Address,
                //    LogoFileName = user.Company?.LogoFileName,
                //    City = user.Company?.City,
                //    Country = user.Company?.Country,
                //    CreatedAt = user.Company.CreatedAt,
                //    EmisNumber = user.Company?.EmisNumber,
                //    SchoolTypeId = user.Company.SchoolTypeId,
                //    UpdatedAt = user.Company.UpdatedAt,
                //    WebSite = user.Company?.WebSite

                //};

                var refreshToken = BuildRefreshToken(ipAddress);
                response.RefreshToken = refreshToken.Token;

                return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(ex.Message);
            }
        }


        #region USER OPTIONS
        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return new Response<string>(user.Id, message: $"Account Confirmed for {user.Email}. You can now use the /api/Account/authenticate endpoint.");
            }
            else
            {
                throw new ApiException($"An error occured while confirming {user.Email}.");
            }
        }

        public async Task<Response<string>> EnableAsync(UserStateChangeDTO dto, string origin)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId);
            if (user != null)
            {
                user.IsActive = dto.IsActive;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {

                    return new Response<string>($"{user.FirstName}{" "}{user.LastName}", message: $"Utilizador alterado com sucesso!");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Utilizador {dto.UserId} Não encontrado.");
            }
        }

        public async Task ForgotPassword(ForgotPasswordDTO dto, string origin)
        {
            var account = await _userManager.FindByEmailAsync(dto.Email);

            if (account == null) return;

            var code = await _userManager.GeneratePasswordResetTokenAsync(account);
            var route = "api/account/reset-password/";

            var _enpointUri = new Uri(string.Concat($"{origin}/", route));

            var emailRequest = new MailSendDTO()
            {
                Body = $"You reset token is - {code}",
                To = dto.Email,
                Subject = "Reset Password",
            };
            await _emailService.SendAsync(emailRequest);
        }
        #endregion
        #region GETTERS
        public async Task<IQueryable> GetRoles(List<string> userRoles)
        {
            var hasSuperAdminRole = userRoles.Any(x => x == "Super Admin");

            if (hasSuperAdminRole)
                return await Task.FromResult(_roleManager.Roles);
            else
            {
                var query = _roleManager.Roles;
                query = query.Where(x => x.Name == Roles.Teatcher.ToString() || x.Name == Roles.Secretary.ToString() || x.Name == Roles.Admin.ToString());
                return await Task.FromResult(query);

            }
        }

        public async Task<Response<UserResponseDTO>> GetRolesAndPermissions(Guid id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                var response = new List<UserResponseDTO>();

                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                UserResponseDTO u = new UserResponseDTO
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Id = user.Id,
                    UserName = user.UserName,
                    // PeerdID = user.PeerID,
                    //Colaborador = await this._colaboradorRepository.GetColaboradorByUser(user.Id),
                    IsActive = user.IsActive,
                    Roles = rolesList.ToList(),
                    //Permissoes = await this._permissoesUtilizadoresService.GetPermissionsByIdUser(Guid.Parse(user.Id))
                };


                return new Response<UserResponseDTO>(u, $"User");
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(ex.Message);
            }
        }

        public async Task<Response<List<UserResponseDTO>>> GetUsers()
        {
            var useres = _userManager.Users;
            ;
            var response = new List<UserResponseDTO>();
            foreach (var user in useres)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var parsedRoles = roles?.Select(s => s.ToString())?.ToList();
                UserResponseDTO u = new UserResponseDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    CompanyId = user.CompanyId ?? Guid.Empty,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Roles = parsedRoles,
                    IsActive = user.IsActive,
                    //Colaborador = await this._colaboradorRepository.GetColaboradorByUser(user.Id)
                };
                response.Add(u);
            }

            return new Response<List<UserResponseDTO>>(response, $"List of all users");
        }

        public async Task<PagedResponse<List<UserResponseDTO>>> GetUsers(int pageNumber, int pageSize, string orderBy, string search, Guid? CompanyId)
        {
            try
            {

                var queriable = _userManager.Users.AsQueryable();


                if (CompanyId.HasValue && CompanyId != Guid.Empty)
                    queriable = _userManager.Users.Where(x => x.CompanyId == CompanyId.Value);

                if (queriable != null)
                    queriable = string.IsNullOrEmpty(search) ? queriable : queriable.Where(x => x.NormalizedUserName.Contains(search));
                else
                    queriable = string.IsNullOrEmpty(search) ? _userManager.Users : _userManager.Users.Where(x => x.NormalizedUserName == search);

                var countItems = await queriable.CountAsync();

                var skip = (pageNumber - 1) * pageSize;
                if (pageNumber > 0)
                    queriable = queriable.Skip(skip).Take(pageSize);

                var response = new List<UserResponseDTO>();

                var list = await queriable.ToListAsync();

                foreach (var user in list)
                {

                    var roles = await _userManager.GetRolesAsync(user);
                    var parsedRoles = roles?.Select(s => s.ToString()).ToList();

                    UserResponseDTO u = new UserResponseDTO
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        CompanyId = user.CompanyId ?? Guid.Empty,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Roles = parsedRoles,
                        IsActive = user.IsActive,

                    };
                    response.Add(u);
                }
                var totalpage = pageSize == 700 ? 1 : (int)Math.Ceiling(decimal.Divide(countItems, pageSize == 0 ? 1 : pageSize));
                //applying Order By if needed
                var finalData = string.IsNullOrEmpty(orderBy) ? response : response.OrderBy(x => orderBy)?.ToList();

                return new PagedResponse<List<UserResponseDTO>>(finalData, pageNumber, pageSize, totalpage, countItems);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(ex.Message);
            }
        }

        public async Task<Response<List<UserRolesResponseDTO>>> GetUsersAndRoles()
        {


            var Users = _userManager.Users.Select(user => new ApplicationUser
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.UserName

            }).ToList();

            var response = new List<UserRolesResponseDTO>();


            foreach (var user in Users)
            {

                var Roles = await _userManager.GetRolesAsync(user);
                var RoleslIst = new List<RoleDTO>();

                foreach (var role in Roles)
                {
                    var x = _roleManager.Roles.Where(x => x.Name == role).FirstOrDefault();
                    RoleDTO Urole = new RoleDTO
                    {
                        Id = x.Id,
                        Name = x.Name
                    };
                    RoleslIst.Add(Urole);
                }

                UserRolesResponseDTO u = new UserRolesResponseDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = RoleslIst
                };

                response.Add(u);
                //RoleslIst.Clear();
            }


            return new Response<List<UserRolesResponseDTO>>(response, $"List of all users");
        }
        #endregion
        #region CRUD
        public async Task<Response<string>> RegisterAsync(UserRegisterDTO dto, string origin, string CompanyId)
        {



            if (!dto.CompanyId.HasValue && string.IsNullOrEmpty(CompanyId))
                throw new ApiException($"CompanyId não informado");

            var userWithSameUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"This username is not available!");
            }

            dto.UserName = dto.UserName.TrimStart();
            dto.UserName = dto.UserName.TrimEnd();

            if (string.IsNullOrEmpty(dto.UserName))
                throw new ApiException($"User name can not be null!");

            var user = new ApplicationUser
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                IsActive = true,
                EmailConfirmed = true,
                CompanyId = !dto.CompanyId.HasValue || dto.CompanyId == Guid.Empty ? Guid.Parse(CompanyId) : dto.CompanyId,

            };

            var userWithSameEmail = await _userManager.FindByNameAsync(dto.UserName);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, dto.RoleName);

                    return new Response<string>($"Successful registration!");
                }
                else
                {

                    var Erros = result.Errors.FirstOrDefault();

                    switch (Erros.Code)
                    {
                        case "PasswordRequiresUpper":
                            throw new ApiException($"{Strings.PasswordWrongInPatter1}");
                        case "PasswordRequiresNonAlphanumeric":
                            throw new ApiException($"{Strings.PasswordWrongInPatter2}");
                        case "PasswordRequiresDigit":
                            throw new ApiException($"{Strings.PasswordWrongInPatter3}");
                        default:
                            throw new ApiException($"{Erros.Description}" + " " + Erros.Code);
                    }

                    //throw new ApiException($"{Erros.Description}" + " " + Erros.Code);
                }
            }
            else
            {
                throw new ApiException($"This email address is not available!");
            }
        }
        public async Task<Response<string>> RegisterRolesAndPermissionsAsync(RolesOrClaimsUpdateDTO dto)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(dto.IdUser.ToString());
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var AddRolesResult = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());

                    await _userManager.AddToRolesAsync(user, dto.Roles);

                    return new Response<string>($"Permissões e Roles adicionados com sucesso");

                }
                else
                {
                    return new Response<string>($"Usuario não encontrado");

                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
                throw new ApiException(ex.Message);
            }
        }
        public async Task<Response<string>> ResetPassword(ResetPasswordDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
            {
                throw new ApiException($"Nenhuma conta registrada como o username :  {dto.UserName}.");
            }
            var result = await _signInManager.PasswordSignInAsync(user.UserName, dto.LastPassword, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new ApiException($"´credenciais erradas '{dto.UserName}'.");
            }
            if (!user.EmailConfirmed)
            {
                throw new ApiException($"User Account Not Confirmed for '{dto.UserName}'.");
            }


            //var account = await _userManager.FindByNameAsync(model.UserName);
            //if (account == null) throw new ApiException($"No Accounts Registered with {model.UserName}.");

            //Gerar chave de TOKEN para poder fazer RESET da password do utilizador
            string TokenForResetPasswordUser = await _userManager.GeneratePasswordResetTokenAsync(user);
            var Resetresult = await _userManager.ResetPasswordAsync(user, TokenForResetPasswordUser, dto.NewPassword);

            if (Resetresult.Succeeded)
            {
                return new Response<string>(dto.UserName, message: $"Password Resetted.");
            }
            else
            {
                //Se ocorrer um ERRO ao criar a conta de utilizador
                var Erros = Resetresult.Errors.FirstOrDefault(); //Captar a mensagem de erro
                switch (Erros.Code)
                {
                    case "PasswordRequiresUpper":
                        throw new ApiException($"{Strings.PasswordWrongInPatter1}");
                    case "PasswordRequiresNonAlphanumeric":
                        throw new ApiException($"{Strings.PasswordWrongInPatter2}");
                    case "PasswordRequiresDigit":
                        throw new ApiException($"{Strings.PasswordWrongInPatter3}");
                    default:
                        throw new ApiException($"{Erros.Description}" + " " + Erros.Code);
                }

                //throw new ApiException($"Error occured while reseting the password.");
            }
        }
        public async Task<Response<UserUpdateDTO>> UpdateAsync(UserUpdateDTO request, string origin)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null)
            {
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.UserName = request.UserName;
                if (request.CompanyId.HasValue)
                    user.CompanyId = request.CompanyId;
                // user.PeerID = request.PeerID;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new Response<UserUpdateDTO>(request, message: $"Utilizador alterado com sucesso!");
                }
                else
                {
                    throw new ApiException($"{result.Errors}");
                }
            }
            else
            {
                throw new ApiException($"Utilizador {request.Email} Não encontrado.");
            }
        }
        #endregion
        #region RELPERS
        private async Task<JwtSecurityToken> BuildJWToken(ApplicationUser user, string ip)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var CompanyId = user.CompanyId.HasValue ? user.CompanyId.Value.ToString() : string.Empty;

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = ip;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
                new Claim("ip", ipAddress),
                new Claim("companyId", CompanyId)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.DurationInDays),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
        private string BuildRandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
        private RefreshToken BuildRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = BuildRandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        private async Task<List<ClaimDTO>> GetClaimsByRoles(IList<string> roles)
        {
            List<ClaimDTO> claims1 = new List<ClaimDTO>();

            if (roles is null) return claims1;

            if (!roles.Any()) return claims1;

            foreach (var rolee in roles)
            {
                var role = await _roleManager.FindByNameAsync(rolee);
                var claims = await _roleManager.GetClaimsAsync(role);

                if (claims != null)
                    claims1 = claims.Select(x => new ClaimDTO { ClaimType = x.Type, ClaimValue = x.Value }).ToList();

            }

            return claims1;
        }
        #endregion
    }
}
