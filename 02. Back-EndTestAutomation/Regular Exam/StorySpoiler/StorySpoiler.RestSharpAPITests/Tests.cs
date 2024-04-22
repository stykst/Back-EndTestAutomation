using RestSharp;
using RestSharp.Authenticators;
using StorySpoiler.RestSharpAPITests.DTOs;
using System.Net;
using System.Text.Json;

namespace StorySpoiler.RestSharpAPITests
{
    public class Tests
    {
        private RestClient _client;
        private RestRequest _request;
        private RestResponse? _response;
        private ApiResponseDTO? _responseData;
        private StoryDTO? _body;
        private const string BASE_URL = "https://d5wfqm7y6yb3q.cloudfront.net";
        private const string USERNAME = "siddarth";
        private const string PASSWORD = "123456";
        private string? storyTitle;
        private string? storyId;

        [OneTimeSetUp]
        public void Setup()
        {
            var accessToken = GetAccess(USERNAME, PASSWORD);

            var options = new RestClientOptions(BASE_URL)
            {
                Authenticator = new JwtAuthenticator(accessToken)
            };

            _client = new RestClient(options);

            storyTitle = "Story Name_" + DateTime.Now.Ticks;
        }

        [Test, Order(1)]
        public void CreateNewStorySpoiler_WithValidRequiredFields_ShouldReturnStatusCode201()
        {
            _body = new StoryDTO
            {
                Title = storyTitle,
                Description = "This is my description",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Story/Create", _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(_responseData.StoryId, Is.Not.Null);
            Assert.That(_responseData.Message, Is.EqualTo("Successfully created!"));
        }

        [Test, Order(2)]
        public void TryToCreateNewStorySpoiler_WithoutRequiredFields_ShouldReturnStatusCode400()
        {
            _body = new StoryDTO
            {
                Title = "",
                Description = "",
                Url = string.Empty,
            };

            _responseData = GetResponseData("/api/Story/Create", _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(3)]
        public void EditStorySpoiler_ShouldReturnStatusCode200()
        {
            storyId = GetStoryId();

            _body = new StoryDTO
            {
                Title = "Edited " + storyTitle,
                Description = "This is my edited description",
                Url = string.Empty,
            };

            _responseData = GetResponseData($"/api/Story/Edit/{storyId}", _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Successfully edited"));
            Assert.That(_responseData.StoryId, Is.EqualTo(storyId));
        }

        [Test, Order(4)]
        public void TryToEditNonExistingStory_ShouldReturnStatusCode404()
        {
            storyId = "unexistingId";

            _body = new StoryDTO
            {
                Title = storyTitle,
                Description = "This is my edited description",
                Url = string.Empty,
            };

            _responseData = GetResponseData($"/api/Story/Edit/{storyId}", _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(_responseData.Message, Is.EqualTo("No spoilers..."));
        }

        [Test, Order(5)]
        public void TryToDeleteNonExistingStory_ShouldReturnStatusCode400()
        {
            storyId = "unexistingIdea";

            _responseData = GetResponseData($"/api/Story/Delete/{storyId}", null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Message, Is.EqualTo("Unable to delete this story spoiler!"));
        }

        [Test, Order(6)]
        public void DeleteLastStory_ShouldReturnStatusCode200()
        {
            storyId = GetStoryId();

            _responseData = GetResponseData($"/api/Story/Delete/{storyId}", null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Deleted successfully!"));
        }


        [OneTimeTearDown]
        public void ShutDown()
        {
            _client.Dispose();
        }

        private string GetAccess(string username, string password)
        {
            _client = new RestClient(BASE_URL);

            _body = new StoryDTO
            {
                UserName = username,
                Password = password
            };

            _responseData = GetResponseData("/api/User/Authentication", _body, Method.Post);

            return _responseData.AccessToken;
        }
        private ApiResponseDTO GetResponseData(string endpoint, StoryDTO? body = null, Method method = Method.Get)
        {
            _request = new RestRequest(endpoint);
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
        private string GetStoryId()
        {
            _request = new RestRequest($"/api/Story/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);
            return content.LastOrDefault().Id;
        }
    }
}