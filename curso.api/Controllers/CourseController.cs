using curso.api.Business.Entities;
using curso.api.Business.Repositories;
using curso.api.Models.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace curso.api.Controllers
{   
    [Route("api/v1/courses")]
    [ApiController]
    [Authorize]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        ///<summary>
        ///This service alows an authenticated user to register a course
        ///</summary>
        ///<returns> It return status 201 and the user course data </returns>
        [SwaggerResponse(statusCode: 201, description: "Course cadastration sucessfull")]
        [SwaggerResponse(statusCode: 401, description: "Not Authorized")]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Post(CourseViewModelInput courseViewModelInput)
        {
            var userCode = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            Course course = new Course();
            course.Name = courseViewModelInput.Name;
            course.Description = courseViewModelInput.description;
            course.UserCode = userCode;
            _courseRepository.Add(course);
            _courseRepository.Commit();

            return Created("", courseViewModelInput);
        }

        ///<summary>
        ///This service alows an authenticated user to register a course
        ///</summary>
        ///<returns> It return status 201 and the user course data </returns>
        [SwaggerResponse(statusCode: 200, description: "Course obtainment sucessfull")]
        [SwaggerResponse(statusCode: 401, description: "Not Authorized")]
        [HttpGet]
        [Route("")]

        public async Task<IActionResult> Get()
        {
            var userCode = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var courses = _courseRepository.GetUserCourses(userCode)
                .Select(s=> new CourseViewModelOutput()
                {
                    Name = s.Name,
                    Descritpion = s.Description,
                    Login = s.User.Login
                });
          
            return Ok(courses);
        }


    }
}
