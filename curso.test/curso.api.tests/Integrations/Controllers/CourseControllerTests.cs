using AutoBogus;
using curso.api.Models.Courses;
using curso.api.Models.Users;
using curso.api.tests.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace curso.api.tests.Integrations.Controllers
{
    public class CourseControllerTests : UserControllerTests
    {
        protected CourseViewModelInput courseViewModelInput;
        public CourseControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output) : base(factory,output)
        { }

        [Fact]
        public async Task Register_ProvidingValidCourseDataAndAuthenticatedUser_ShoudldReturnCreated()
        {
            //AAA
            //ARRANGE
            courseViewModelInput  = new AutoFaker<CourseViewModelInput>(AutoBogusConfiguration.LOCALE);
            StringContent content = new StringContent(JsonConvert.SerializeObject(courseViewModelInput), Encoding.UTF8, "application/json");

            //ACT
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginViewModelOutput.Token);
            var httpClientRequest = await _httpClient.PostAsync("api/v1/Courses/Register", content);
            var courseViewModelOutput = JsonConvert.DeserializeObject<CourseViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());

            //ASSERT
           Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
           Assert.Equal(courseViewModelInput.Name, courseViewModelOutput.Name);
           Assert.Equal(courseViewModelInput.Description, courseViewModelOutput.Descritpion);
           _output.WriteLine($"{nameof(CourseControllerTests)}_{nameof(Register_ProvidingValidCourseDataAndAuthenticatedUser_ShoudldReturnCreated)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task Register_ProvidingValidCourseDataAndNoAuthenticatedUser_ShoudldReturnUnauthorized()
        {
            //AAA
            //ARRANGE
            courseViewModelInput = new AutoFaker<CourseViewModelInput>();
            StringContent content = new StringContent(JsonConvert.SerializeObject(courseViewModelInput), Encoding.UTF8, "application/json");

            //ACT
            var httpClientRequest = await _httpClient.PostAsync("api/v1/Courses/Register", content);
            
            //ASSERT
            Assert.Equal(HttpStatusCode.Unauthorized, httpClientRequest.StatusCode);
            _output.WriteLine($"{nameof(CourseControllerTests)}_{nameof(Register_ProvidingValidCourseDataAndNoAuthenticatedUser_ShoudldReturnUnauthorized)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }

        [Fact]
        public async Task View_WhenAuthorizedAsValidUser_ShoudldReturnOKandList()
        {
            //AAA
            //ARRANGE
            await Register_ProvidingValidCourseDataAndAuthenticatedUser_ShoudldReturnCreated();
            
            //ACT
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginViewModelOutput.Token);
            var httpClientRequest = await _httpClient.GetAsync("api/v1/Courses/View");
            var courseViewModelOutput = JsonConvert.DeserializeObject<IList<CourseViewModelOutput>>(await httpClientRequest.Content.ReadAsStringAsync());

            //ASSERT
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            _output.WriteLine($"{nameof(CourseControllerTests)}_{nameof(View_WhenAuthorizedAsValidUser_ShoudldReturnOKandList)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }
    }
}
