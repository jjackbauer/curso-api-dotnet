using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Configurations;
using curso.api.Filters;
using curso.api.Infraestructure.Data;
using curso.api.Infraestructure.Repositories;
using curso.api.Models;
using curso.api.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace curso.api.Controllers
{
    [Route("api/v1/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        public UserController(
            IUserRepository userRepository, 
            IConfiguration configuration,
            IAuthenticationService authentication)
        {
            _userRepository = userRepository;
            _authenticationService = authentication;
        }

        /// <summary>
        /// This service Alows a registered user to log in  the system
        /// </summary>
        /// <param name="loginViewModelInput"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 200, description: "Successful authentication",Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 400, description: "Mandatory Fields",Type = typeof(ValidateFieldViewModelOutput))]
        [SwaggerResponse(statusCode: 500, description: "Server Error", Type = typeof(GenericErrorViewModel))]
        [HttpPost]
        [Route("LogIn")]
        [CustomModelStateValidation]
        public async Task<IActionResult> LogIn(LoginViewModelInput loginViewModelInput)
        {
            User user = await _userRepository.GetUserAsync(loginViewModelInput.Login);
            
            if (user == null)
                throw new Exception();

            var token = _authenticationService.GetToken(user);
            var loginViewModelOutput = new LoginViewModelOutput()
            {
                Token = token,
                Login = user.Login,
                Email = user.Email
            };
          

            return Ok(loginViewModelOutput);
        }
        /// <summary>
        /// This service alows the registration of a user in the system
        /// </summary>
        /// <param name="registerViewModelInput"></param>
        /// <returns></returns>
        [SwaggerResponse(statusCode: 201, description: "Successful cadastration", Type = typeof(LoginViewModelInput))]
        [SwaggerResponse(statusCode: 401, description: "Mandatory Fields", Type = typeof(ValidateFieldViewModelOutput))]
        [SwaggerResponse(statusCode: 501, description: "Server Error", Type = typeof(GenericErrorViewModel))]
        [HttpPost]
        [Route("SignUp")]
        [CustomModelStateValidation]
        public async Task<IActionResult> SignUp(RegisterViewModelInput registerViewModelInput)
        {

            //var pendingMigrations = context.Database.GetPendingMigrations();

            //if (pendingMigrations.Count() > 0)
            //    context.Database.Migrate();



            var user = await _userRepository.GetUserAsync(registerViewModelInput.Login);

            if (user != null)
                return BadRequest("User already Signed in the system");

            var newUser = new User();
            newUser.Login = registerViewModelInput.Login;
            newUser.Email = registerViewModelInput.Email;
            newUser.Password = registerViewModelInput.Password;
           
            _userRepository.Add(newUser);
            _userRepository.Commit();

            return Created("", registerViewModelInput);
        }
    }
}

