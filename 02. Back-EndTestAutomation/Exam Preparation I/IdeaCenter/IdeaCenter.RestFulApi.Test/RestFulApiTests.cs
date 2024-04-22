using IdeaCenter.RestFulApi.Test.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace IdeaCenter.RestFulApi.Test
{
    public class RestFulApiTests
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse? _response;
        private ApiResponseDTO? _responseData;
        private IdeaDTO? _body;
        private RequestParameter? _parameters;
        private const string BaseUrl = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:84";
        private const string Email = "jasiel.anvith@dockerbike.com";
        private const string Password = "123456";
        private string? lastCreatedId;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                Authenticator = new JwtAuthenticator(GetAccess(Email, Password))
            };

            _client = new RestClient(options);
        }

        [Test, Order(1)]
        public void CreateNewIdea_WithValidRequiredFields_ShouldReturnStatusCode200()
        {
            _body = new IdeaDTO
            {
                Title = "New Idea",
                Description = "This is my description",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Idea/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Successfully created!"));
        }

        [Test, Order(2)]
        public void TryToCreateNewIdea_WithoutRequiredFields_ShouldReturnStatusCode400()
        {
            _body = new IdeaDTO
            {
                Title = "",
                Description = "",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Idea/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(3)]
        public void GetAllIdeas_ShouldReturnStatusCode200()
        {
            _request = new RestRequest("/api/Idea/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);

            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.Not.Empty);
        }

        [Test, Order(4)]
        public void EditLastIdea_ShouldReturnStatusCode200()
        {
            lastCreatedId = GetLastIdeaId();

            _parameters = new RequestParameter { Name = "ideaId", Value = lastCreatedId };

            _body = new IdeaDTO
            {
                Title = "Edited Idea",
                Description = "This is my edited description",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Idea/Edit", _parameters, _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Edited successfully"));
        }

        [Test, Order(5)]
        public void TryToEditNonExistingIdea_ShouldReturnStatusCode400()
        {
            lastCreatedId = "unexistingId";

            _parameters = new RequestParameter { Name = "ideaId", Value = lastCreatedId };

            _body = new IdeaDTO
            {
                Title = "Edited Idea",
                Description = "This is my edited description",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Idea/Edit", _parameters, _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Content, Does.Contain("There is no such idea!"));
        }

        [Test, Order(6)]
        public void TryToDeleteNonExistingIdea_ShouldReturnStatusCode400()
        {
            lastCreatedId = "unexistingIdea";

            _parameters = new RequestParameter { Name = "ideaId", Value = lastCreatedId };

            _responseData = GetResponseData("/api/Idea/Delete", _parameters, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Content, Does.Contain("There is no such idea!"));
        }

        [Test, Order(7)]
        public void DeleteLastIdea_ShouldReturnStatusCode200()
        {
            lastCreatedId = GetLastIdeaId();

            _parameters = new RequestParameter { Name = "ideaId", Value = lastCreatedId };

            _responseData = GetResponseData("/api/Idea/Delete", _parameters, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Content, Does.Contain("The idea is deleted!"));
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            _client.Dispose();
        }

        private string GetAccess(string email, string password)
        {
            _client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/User/Authentication");
            request.AddJsonBody(new
            {
                email,
                password
            });

            var response = _client.Execute(request, Method.Post);
            var rd = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            return rd.AccessToken;
        }
        private ApiResponseDTO GetResponseData(string endpoint, RequestParameter? parameters = null, IdeaDTO? body = null, Method method = Method.Get)
        {
            _request = new RestRequest(endpoint);
            if (parameters != null)
            {
                _request.AddQueryParameter(parameters.Name, parameters.Value);
            }

            if (body != null)
            {
                _request.AddJsonBody(body);
            }

            _response = _client.Execute(_request, method);

            try
            {
                var rd = JsonSerializer.Deserialize<ApiResponseDTO>(_response.Content);

                rd.StatusCode = _response.StatusCode;

                return rd;
            }

            catch (Exception)
            {
                return new ApiResponseDTO()
                {
                    StatusCode = _response.StatusCode,
                    Content = _response.Content,
                };
            }
        }
        private string GetLastIdeaId()
        {
            _request = new RestRequest("/api/Idea/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);
            return content.LastOrDefault().IdeaId;
        }
    }
}