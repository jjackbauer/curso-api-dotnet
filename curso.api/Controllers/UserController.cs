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
        public IActionResult LogIn(LoginViewModelInput loginViewModelInput)
        {
            User user = _userRepository.GetUser(loginViewModelInput.Login);
            var logInModelViewOutput = new LogInModelViewOutput()
            {
                code =user.Code,
                login = user.Login,
                email = user.Email
            };
            
            
            var token = _authenticationService.GetToken(logInModelViewOutput);

            return Ok(new  
            {
                Token = token, 
                User = logInModelViewOutput
            });
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
        public IActionResult SignUp(RegisterViewModelInput registerViewModelInput)
        {
           
            //var pendingMigrations = context.Database.GetPendingMigrations();

            //if (pendingMigrations.Count() > 0)
            //    context.Database.Migrate();

            var user = new User();
            user.Login = registerViewModelInput.Login;
            user.Email = registerViewModelInput.Email;
            user.Password = registerViewModelInput.Password;
            //context.User.Add(user);
            //context.SaveChanges();
            _userRepository.Add(user);
            _userRepository.Commit();

            

            return Created("", registerViewModelInput);
        }
    }
}

