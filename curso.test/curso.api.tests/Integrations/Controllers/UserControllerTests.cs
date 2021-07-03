using AutoBogus;
using curso.api.Models.Users;
using curso.api.tests.Configurations;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace curso.api.tests.Integrations.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        protected readonly WebApplicationFactory<Startup> _factory;
        protected readonly ITestOutputHelper _output;
        protected readonly HttpClient _httpClient;
        protected RegisterViewModelInput registerViewModelInput;
        protected LoginViewModelOutput loginViewModelOutput;
        public UserControllerTests(WebApplicationFactory<Startup> factory, ITestOutputHelper output)
        {
            _factory = factory;
            _output = output;
            _httpClient = _factory.CreateClient();

        }

        public async Task DisposeAsync()
        {
            _httpClient.Dispose();
        }

        public async Task InitializeAsync()
        {
            await SignUp_ProvidingNonExistingUserAndPassword_ShoudldReturnCreated();
            await LogIn_ProvidingExistingUserAndPassword_ShouldReturnOk();
        }

        [Fact]
        //When_Given_Then
        public async Task LogIn_ProvidingExistingUserAndPassword_ShouldReturnOk()
        {
            //AAA
            //ARRANGE
            var loginViewModelInput = new LoginViewModelInput
            {
                Login = registerViewModelInput.Login,
                Password = registerViewModelInput.Password
            };

            StringContent content = new StringContent(JsonConvert.SerializeObject(loginViewModelInput),Encoding.UTF8,"application/json");

            //ACT
            var httpClientRequest = await _httpClient.PostAsync("api/v1/User/LogIn",content);
            loginViewModelOutput = JsonConvert.DeserializeObject<LoginViewModelOutput>(await httpClientRequest.Content.ReadAsStringAsync());


            //ASSERT
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(loginViewModelOutput.Token);
            Assert.Equal(loginViewModelInput.Login, loginViewModelOutput.Login);
            Assert.NotNull(loginViewModelOutput.Email);
            _output.WriteLine($"{nameof(UserControllerTests)}_{nameof(LogIn_ProvidingExistingUserAndPassword_ShouldReturnOk)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }
        [Fact]
        public async Task SignUp_ProvidingNonExistingUserAndPassword_ShoudldReturnCreated()
        {
            //AAA
            //ARRANGE
            registerViewModelInput = new AutoFaker<RegisterViewModelInput>(AutoBogusConfiguration.LOCALE)
                .RuleFor(p => p.Email, faker => faker.Person.Email);
            
            StringContent content = new StringContent(JsonConvert.SerializeObject(registerViewModelInput), Encoding.UTF8, "application/json");

            //ACT
            var httpClientRequest = await _httpClient.PostAsync("api/v1/User/SignUp", content);
            var registerViewModelOutput = JsonConvert.DeserializeObject<RegisterViewModelInput>(await httpClientRequest.Content.ReadAsStringAsync());

            //ASSERT
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
            Assert.Equal(registerViewModelInput.Password, registerViewModelOutput.Password);
            Assert.Equal(registerViewModelInput.Login, registerViewModelOutput.Login);
            Assert.Equal(registerViewModelInput.Email, registerViewModelOutput.Email);
            _output.WriteLine($"{nameof(UserControllerTests)}_{nameof(SignUp_ProvidingNonExistingUserAndPassword_ShoudldReturnCreated)} = {await httpClientRequest.Content.ReadAsStringAsync()}");
        }
    }
}
