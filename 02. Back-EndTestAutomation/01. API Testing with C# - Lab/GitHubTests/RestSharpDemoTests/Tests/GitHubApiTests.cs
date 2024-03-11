using RestSharp;
using RestSharp.Authenticators;
using RestSharpDemoTests.Models;
using System.Net;
using System.Text.Json;

namespace RestSharpDemoTests.Tests
{
    public class GitHubApiTests
    {
        private RestClient _client;

        [SetUp]
        public void Setup()
        {
            var options = new RestClientOptions("https://api.github.com")
            {
                Authenticator = new HttpBasicAuthenticator("user", "token")
            };

            _client = new RestClient(options);
        }

        [Test]
        public void Test_GetAllIssuesFromRepo()
        {
            // Arrange
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues");

            // Act
            var response = _client.Execute(request);
            var issues = JsonSerializer.Deserialize <List<Issue>>(response.Content);

            // Assert
            Assert.That(issues.Count > 1);

            foreach (var issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
            }
        }

        [Test]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            var title = "This is a Demo Issue";
            var body = "QA Back-End Automation Course March 2024";

            // Act
            var issue = CreateIssue(title, body);

            // Assert
            Assert.That(issue.Id, Is.GreaterThan(0));
            Assert.That(issue.Number, Is.GreaterThan(0));
            Assert.That(issue.Title, Is.Not.Empty);
        }

        [Test]
        public void Test_EditIssue()
        {
            // Arrange
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues/5267");
            request.AddBody(new { title = "Changing the name of the issue that I created" });

            // Act
            var response = _client.Execute(request, Method.Patch);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.Not.Empty, "The response content should be not empty.");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID should br greater than 0.");
            Assert.That(issue.Number, Is.GreaterThan(0), "Issue number should be greater than 0.");
            Assert.That(issue.Title, Is.EqualTo("Changing the name of the issue that I created"));
        }

        private Issue CreateIssue(string title, string body)
        {
            var request = new RestRequest("/repos/testnakov/test-nakov-repo/issues");
            request.AddBody(new { body, title  });

            var response = _client.Execute(request, Method.Post);
            var issue = JsonSerializer.Deserialize<Issue>(response.Content);

            return issue;
        }
    }
}