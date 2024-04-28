using RestSharp;
using RevueCrafters.RestSharpAPI.Test.DTOs;
using System.Net;
using System.Text.Json;

namespace RevueCrafters.RestSharpAPI.Test
{
    public class RestSharpAPITests : BaseTest
    {
        [Test, Order(1)]
        public void CreateNewRevue_WithRequiredFields_ShouldReturnStatusCode200()
        {
            _body = new RevueDTO
            {
                Title = "New Revue",
                Url = string.Empty,
                Description = "This is my description",
            };

            _responseData = GetResponseData("/api/Revue/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Successfully created!"));
        }

        [Test, Order(2)]
        public void GetAllRevues_WithRequiredFields_ShouldReturnStatusCode200()
        {
            _request = new RestRequest("/api/Revue/All");
            _response = _client.Execute(_request);

            var content = JsonSerializer.Deserialize<List<ApiResponseDTO>>(_response.Content);

            Assert.That(_response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(content, Is.Not.Null);
            Assert.That(content, Is.Not.Empty);
        }

        [Test, Order(3)]
        public void EditLastRevue_WithValidParameters_ShouldReturnStatusCode200()
        {
            _lastCreatedId = GetLastRevueId();

            _parameters = new RequestParameter { Name = "revueId", Value = _lastCreatedId };

            _body = new RevueDTO
            {
                Title = "Edited Revue",
                Url = string.Empty,
                Description = "This is my edited description",
            };

            _responseData = GetResponseData("/api/Revue/Edit", _parameters, _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("Edited successfully"));
        }

        [Test, Order(4)]
        public void DeleteLastRevue_WithValidParameters_ShouldReturnStatusCode200()
        {
            _lastCreatedId = GetLastRevueId();

            _parameters = new RequestParameter { Name = "revueId", Value = _lastCreatedId };

            _responseData = GetResponseData("/api/Revue/Delete", _parameters, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(_responseData.Message, Is.EqualTo("The revue is deleted!"));
        }

        [Test, Order(5)]
        public void TryToCreateNewRevue_WithoutRequiredFields_ShouldReturnStatusCode400()
        {
            _body = new RevueDTO
            {
                Title = string.Empty,
                Url = string.Empty,
                Description = string.Empty,
            };

            _responseData = GetResponseData("/api/Revue/Create", null, _body, Method.Post);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test, Order(6)]
        public void TryToEditNonExistingRevue_ShouldReturnStatusCode400()
        {
            _lastCreatedId = "GetLastRevueId()";

            _parameters = new RequestParameter { Name = "revueId", Value = _lastCreatedId };

            _body = new RevueDTO
            {
                Title = "Edited Revue",
                Url = string.Empty,
                Description = "This is my edited description",
            };

            _responseData = GetResponseData("/api/Revue/Edit", _parameters, _body, Method.Put);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Message, Is.EqualTo("There is no such revue!"));
        }

        [Test, Order(7)]
        public void TryToDeleteNonExistingRevue_ShouldReturnStatusCode400()
        {
            _lastCreatedId = "GetLastRevueId()";

            _parameters = new RequestParameter { Name = "revueId", Value = _lastCreatedId };

            _responseData = GetResponseData("/api/Revue/Delete", _parameters, null, Method.Delete);

            Assert.That(_responseData.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_responseData.Message, Is.EqualTo("There is no such revue!"));
        }
    }
}