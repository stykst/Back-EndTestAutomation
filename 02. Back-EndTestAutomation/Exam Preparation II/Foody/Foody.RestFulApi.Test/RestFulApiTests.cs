using Foody.RestFulApi.Test.DTOs;
using RestSharp;
using RestSharp.Authenticators;
using System.Net;
using System.Text.Json;

namespace Foody.RestFulApi.Test
{
    public class RestFulApiTests
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse? _response;
        private ApiResponseDTO? _responseData;
        private FoodDTO? _body;
        private RequestParameter _parameters;
        private const string BaseUrl = "http://softuni-qa-loadbalancer-2137572849.eu-north-1.elb.amazonaws.com:86";
        private const string UserName = "kitt";
        private const string Password = "123456";
        private string? lastId;

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                Authenticator = new JwtAuthenticator(GetAccess(UserName, Password))
            };

            _client = new RestClient(options);
        }

        [Test, Order(1)]
        public void CreateNewFood_WithValidRequiredFields_ShouldReturnStatusCode201()
        {
            _body = new FoodDTO
            {
                Name = "New Food",
                Description = "This is my description",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Food/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }

        [Test, Order(2)]
        public void TryToCreateNewFood_WithoutRequiredFields_ShouldReturnStatusCode400()
        {
            _body = new FoodDTO
            {
                Name = "",
                Description = "",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Food/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(3)]
        public void EditLastFood_ShouldReturnStatusCode200()
        {
            lastId = GetLastId();

            var body = new EditBody[]
            {
                new EditBody
                {
                    path = "/name",
                    op = "replace",
                    value = "Edited Food"
                }
            };

            _request = new RestRequest($"/api/Food/Edit/{lastId}");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request, Method.Patch);
            _responseData = JsonSerializer.Deserialize<ApiResponseDTO>(_response.Content);

            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Successfully edited"));
        }

        [Test, Order(4)]
        public void GetAllFoods_ShouldReturnStatusCode200()
        {
            _request = new RestRequest("/api/Food/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);

            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.Not.Empty);
        }

        [Test, Order(5)]
        public void TryToEditNonExistingFood_ShouldReturnStatusCode404()
        {
            lastId = "unexistingId";

            var body = new EditBody[]
            {
                new EditBody
                {
                    path = "/name",
                    op = "replace",
                    value = "Edited Food"
                }
            };

            _request = new RestRequest($"/api/Food/Edit/{lastId}");
            _request.AddJsonBody(body);

            _response = _client.Execute(_request, Method.Patch);
            _responseData = JsonSerializer.Deserialize<ApiResponseDTO>(_response.Content);

            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(_responseData.Message, Is.EqualTo("No food revues..."));
        }

        [Test, Order(6)]
        public void TryToDeleteNonExistingIdea_ShouldReturnStatusCode400()
        {
            lastId = "unexistingId";

            _responseData = GetResponseData($"/api/Food/Delete/{lastId}", null, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Message, Is.EqualTo("Unable to delete this food revue!"));
        }

        [Test, Order(7)]
        public void DeleteLastIdea_ShouldReturnStatusCode200()
        {
            lastId = GetLastId();

            _responseData = GetResponseData($"/api/Food/Delete/{lastId}", null, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Deleted successfully!"));
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            _client.Dispose();
        }

        private string GetAccess(string username, string passw)
        {
            _client = new RestClient(BaseUrl);
            var request = new RestRequest("/api/User/Authentication");
            request.AddJsonBody(new
            {
                userName = username,
                password = passw
            });

            var response = _client.Execute(request, Method.Post);
            var rd = JsonSerializer.Deserialize<ApiResponseDTO>(response.Content);

            return rd.AccessToken;
        }
        private ApiResponseDTO GetResponseData(string endpoint, RequestParameter? parameters = null, FoodDTO? body = null, Method method = Method.Get)
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
        private string GetLastId()
        {
            _request = new RestRequest("/api/Food/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);
            return content.LastOrDefault().FoodId;
        }
    }
}