using RestSharp.Authenticators;
using RestSharp;
using RevueCrafters.RestSharpAPI.Test.DTOs;
using System.Text.Json;

namespace RevueCrafters.RestSharpAPI.Test
{
    public class BaseTest
    {
        protected RestClient _client;
        protected RestRequest _request;
        protected RestResponse? _response;
        protected ApiResponseDTO? _responseData;
        protected RevueDTO? _body;
        protected RequestParameter? _parameters;
        protected string? _lastCreatedId;
        protected const string BaseUrl = "https://d2925tksfvgq8c.cloudfront.net";
        protected const string Email = "wolfgang.neo@dockerbike.com";
        protected const string Password = "123456";

        [OneTimeSetUp]
        public void Setup()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                Authenticator = new JwtAuthenticator(GetAccess(Email, Password))
            };

            _client = new RestClient(options);
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
        protected ApiResponseDTO GetResponseData(string endpoint, RequestParameter? parameters = null, RevueDTO? body = null, Method method = Method.Get)
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
        protected string GetLastRevueId()
        {
            _request = new RestRequest("/api/Revue/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);
            return content.LastOrDefault().RevueId;
        }
    }
}
