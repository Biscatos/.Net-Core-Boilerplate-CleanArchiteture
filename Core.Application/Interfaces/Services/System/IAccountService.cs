using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Application.DTOS.System;
using Core.Application.Wrappers;

namespace Core.Application.Interfaces.Services.System
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationDTO request, string ipAddress);

        Task<Response<string>> RegisterAsync(UserRegisterDTO dto, string origin, string companyId);
        Task<Response<UserUpdateDTO>> UpdateAsync(UserUpdateDTO request, string origin);
        Task<Response<string>> EnableAsync(UserStateChangeDTO dto, string origin);

        Task<Response<List<UserResponseDTO>>> GetUsers();
        Task<PagedResponse<List<UserResponseDTO>>> GetUsers(int pageNumber, int pageSize, string orderBy, string search, Guid? companyId);
        Task<Response<List<UserRolesResponseDTO>>> GetUsersAndRoles();

        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPassword(ForgotPasswordDTO dto, string origin);
        Task<Response<string>> ResetPassword(ResetPasswordDTO dto);


        Task<IQueryable> GetRoles(List<string> userRoles);

        Task<Response<string>> RegisterRolesAndPermissionsAsync(RolesOrClaimsUpdateDTO dto);

        Task<Response<UserResponseDTO>> GetRolesAndPermissions(Guid id);
    }
}
