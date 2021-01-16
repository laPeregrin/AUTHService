using AUTHENTICATIONService.Models;
using AUTHENTICATIONService.Models.Wraps;
using AUTHENTICATIONService.Services;
using AUTHENTICATIONService.Services.Extensions;
using AUTHENTICATIONService.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AUTHENTICATIONService.Services.Implementations;
using AUTHENTICATIONService.Services.TokenValidators;
using System.Collections.Concurrent;
using Microsoft.Extensions.Caching.Memory;
using DataBaseStaff;
using DataBaseStaff.Service;
using DataBaseStaff.Models;

namespace AUTHENTICATIONService.Controllers
{
    [Route("API")]
    public class AutneticationController : Controller
    {
        private static readonly ConcurrentDictionary<Guid, string> _cache = new ConcurrentDictionary<Guid, string>();

        private readonly BllLayer _dataService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly AuthenticateService authenticateService;
        public AutneticationController(BllLayer repository,
            IPasswordHasher passwordHasher,
            AuthenticateService authenticateService,
            RefreshTokenValidator refreshTokenValidator)
        {
            _refreshTokenValidator = refreshTokenValidator;
            _dataService = repository;
            _passwordHasher = passwordHasher;
            this.authenticateService = authenticateService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not match confirm password."));
            }
            var CheckuserName = await _dataService.GetUserByExpression(x=>x.UserName == registerRequest.UserName);
            if (CheckuserName != null)
            {
                return BadRequest(new ErrorResponse("username has used already"));
            }
            var CheckuserEmail = await _dataService.GetUserByExpression(x => x.Email == registerRequest.Email);
            if (CheckuserEmail != null)
            {
                return BadRequest(new ErrorResponse("Email has used already"));
            }
            try
            {
                if (Convert.ToInt32(registerRequest.IsAuthor) > 1)
                {
                    return BadRequest(new ErrorResponse("parameter IsAuthor can be just 0(true) or 1(false)"));
                }
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse("parameter IsAuthor can be just 0(true) or 1(false)!"));
            }
            var hashPassword = _passwordHasher.HashPassword(registerRequest.Password);
            var user = registerRequest.ConvertToUser(hashPassword);
            await _dataService.AddUser(user);

            return Ok(user.Id);
        }



        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var IsValidRefreshToken = _refreshTokenValidator.Validate(refreshRequest.RefreshToken);

            if (!IsValidRefreshToken)
            {
                return BadRequest(new ErrorResponse("Invalid refresh token"));
            }
            var id = refreshRequest.RequestId;

            if (_cache.TryGetValue(new Guid(id), out string refreshToken))
            {

                if (refreshToken == refreshRequest.RefreshToken)
                {
                    var user = (User)await _dataService.GetById<User>(new Guid(id));

                    if (user == null)
                        return NotFound("user not found");


                    AuthenticatedUserResponce responce = authenticateService.Authenticate(user);

                    _cache.TryUpdate(user.Id, responce.RefreshToken, refreshToken);

                    return Ok(responce);
                }

                return BadRequest("Wrong id in request");

            }
            return BadRequest("Wrong refresh token");
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }

            var user = await _dataService.GetUserByExpression(x=>x.UserName == loginRequest.UserName);

            if (user == null)
            {
                return Unauthorized();
            }

            bool IsConfirmedPassword = _passwordHasher.isConfirmed(user.PasswordHash, loginRequest.Password);

            if (!IsConfirmedPassword)
            {
                return Unauthorized();
            }

            AuthenticatedUserResponce responce = authenticateService.Authenticate(user);


            _cache.AddOrUpdate(user.Id, responce.RefreshToken, (id, Token) => Token = responce.RefreshToken);


            return Ok(responce);
        }


        [HttpPost("registrationByAdmin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestModelState();
            }
            if (registerRequest.Password != registerRequest.ConfirmPassword)
            {
                return BadRequest(new ErrorResponse("Password does not match confirm password."));
            }
            var CheckuserName = await _dataService.GetUserByExpression(x => x.UserName == registerRequest.UserName);
            if (CheckuserName != null)
            {
                return BadRequest(new ErrorResponse("username has used already"));
            }
            var CheckuserEmail = await _dataService.GetUserByExpression(x => x.Email == registerRequest.Email);
            if (CheckuserEmail != null)
            {
                return BadRequest(new ErrorResponse("Email has used already"));
            }
            var hashPassword = _passwordHasher.HashPassword(registerRequest.Password);
            var user = registerRequest.ConvertToUser(hashPassword);
            await _dataService.AddUser(user);

            return Ok(user.Id);
        }



        #region Underface
        private IActionResult BadRequestModelState()
        {
            IEnumerable<string> errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));

            return BadRequest(new ErrorResponse(errorMessages));
        }
        #endregion
    }
}
