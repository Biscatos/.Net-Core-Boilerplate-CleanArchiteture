using Core.Application.DTOS.System;
using Core.Application.Interfaces.Services.System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Extensions;

namespace WebApi.Controllers.v1
{

    [ApiVersion("1.0")]
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        readonly ILogger<AccountController> _log;
        private IApplicationLogger logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> log, IApplicationLogger logger)
        {
            _accountService = accountService;
            _log = log;
            this.logger = logger;
        }

        [HttpGet("users")]

        public async Task<IActionResult> GetUsers([FromQuery] DefaultQueryParameters query)
        {
            var companyId = this.GetLogedUserCompanyId();
            return Ok(await _accountService.GetUsers(query.PageNumber, query.PageSize, query.OrderBy, query.Search, companyId));
        }

        [HttpGet("company/users")]

        public async Task<IActionResult> GetCompanyUsers([FromQuery] DefaultQueryParameters query)
        {
            var companyId = this.GetLogedUserCompanyId();
            return Ok(await _accountService.GetUsers(query.PageNumber, query.PageSize, query.OrderBy, query.Search, companyId));
        }


        [HttpGet("usersAndRoles")]

        public async Task<IActionResult> GetUsersAndRoles()
        {
            return Ok(await _accountService.GetUsersAndRoles());
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationDTO request)
        {
            return Ok(await _accountService.AuthenticateAsync(request, GetClientIpAddress()));
        }

        [HttpPost("register")]

        public async Task<IActionResult> RegisterAsync(UserRegisterDTO request)
        {
            var origin = Request.Headers["origin"];
            var companyId = HttpContext.GetComanyId();
            return Ok(await _accountService.RegisterAsync(request, origin, companyId));
        }


        [HttpPut("register")]

        public async Task<IActionResult> UpdateUserAsync(UserUpdateDTO request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.UpdateAsync(request, origin));
        }

        [HttpPut("enable")]

        public async Task<IActionResult> EnableUserAsync(UserStateChangeDTO request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _accountService.EnableAsync(request, origin));
        }


        [HttpGet("roles")]

        public async Task<IActionResult> GetRolesAsync()
        {
            var userRoles = HttpContext.GetUserRoles();
            return Ok(await _accountService.GetRoles(userRoles));
        }

        //[HttpGet("confirm-email")]
        //public async Task<IActionResult> ConfirmEmailAsync([FromQuery]string userId, [FromQuery]string code)
        //{
        //    var origin = Request.Headers["origin"];
        //    return Ok(await _accountService.ConfirmEmailAsync(userId, code));
        //}
        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest model)
        //{
        //    await _accountService.ForgotPassword(model, Request.Headers["origin"]);
        //    return Ok();
        //}
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO model)
        {
            return Ok(await _accountService.ResetPassword(model));
        }
        private string GetClientIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        [HttpPost("usersAddRolesAndPermissions")]

        public async Task<IActionResult> AddPermissionsAndRoles(RolesOrClaimsUpdateDTO rolesPermissionsUpdateDTO)
        {
            return Ok(await _accountService.RegisterRolesAndPermissionsAsync(rolesPermissionsUpdateDTO));
        }

        [HttpGet("RolesAndPermissions/{IdUser}")]

        public async Task<IActionResult> GetRolesAndPermisionsAsync(Guid IdUser)
        {
            //var origin = Request.Headers["origin"];
            return Ok(await _accountService.GetRolesAndPermissions(IdUser));
        }

    }
}
