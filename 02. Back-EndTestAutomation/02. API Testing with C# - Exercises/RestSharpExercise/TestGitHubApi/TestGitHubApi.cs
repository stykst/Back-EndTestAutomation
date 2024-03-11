using RestSharpServices;
using System.Net;
using System.Reflection.Emit;
using System.Text.Json;
using RestSharp;
using RestSharp.Authenticators;
using NUnit.Framework.Internal;
using RestSharpServices.Models;
using System;

namespace TestGitHubApi
{
    public class TestGitHubApi
    {
        private GitHubApiClient _client;
        private static int _lastCreatedIssueNumber;
        private static int _lastCreatedCommentId;
        private static string _lastCreatedCommentBody;

        [SetUp]
        public void Setup()
        {            
            _client = new GitHubApiClient("https://api.github.com/repos/testnakov/", "user", "token");
        }


        [Test, Order (1)]
        public void Test_GetAllIssuesFromARepo()
        {
            // Arrange
            var repo = "test-nakov-repo";

            // Act
            var issues = _client.GetAllIssues(repo);

            // Assert
            Assert.That(issues, Has.Count.GreaterThan(1), "There should be more than one issue");

            foreach (var issue in issues)
            {
                Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID shoul be greater than 0.");
                Assert.That(issue.Number, Is.GreaterThan(0), "Issue Number shoul be greater than 0.");
                Assert.That(issue.Title, Is.Not.Empty, "Issue Title shoul be not empty.");
            }
        }

        [Test, Order (2)]
        public void Test_GetIssueByValidNumber()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var issueNumber = 1;

            // Act
            var issue = _client.GetIssueByNumber(repo, issueNumber);

            // Assert
            Assert.IsNotNull(issue, "The response should contain issue data");
            Assert.That(issue.Id, Is.GreaterThan(0), "Issue ID shoul be a positive integer.");
            Assert.That(issue.Number, Is.EqualTo(issueNumber), "The issue number shoul match the requested number.");
        }
        
        [Test, Order (3)]
        public void Test_GetAllLabelsForIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var issueNumber = 6;

            // Act
            var labels = _client.GetAllLabelsForIssue(repo, issueNumber);

            // Assert
            Assert.That(labels.Count, Is.GreaterThan(0), "There should be more than 0 label");

            foreach (var label in labels)
            {
                Assert.That(label.Id, Is.GreaterThan(0), "Label ID shoul be greater than 0.");
                Assert.That(label.Name, Is.Not.Empty, "Label Name shoul be not empty.");

                Console.WriteLine($"Label: {label.Id} - Name: {label.Name}");
            }
        }

        [Test, Order (4)]
        public void Test_GetAllCommentsForIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var issueNumber = 6;

            // Act
            var comments = _client.GetAllCommentsForIssue(repo, issueNumber);

            // Assert
            Assert.That(comments.Count, Is.GreaterThan(0), "There should be more than 0 comment");

            foreach (var comment in comments)
            {
                Assert.That(comment.Id, Is.GreaterThan(0), "Comment ID shoul be greater than 0.");
                Assert.That(comment.Body, Is.Not.Empty, "Comment Body shoul be not empty.");

                Console.WriteLine($"Comment: {comment.Id} - Body: {comment.Body}");
            }
        }

        [Test, Order(5)]
        public void Test_CreateGitHubIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var expectedTitle = "Create Your Own Title";
            var body = "Give Some Description";

            // Act
            var issue = _client.CreateIssue(repo, expectedTitle, body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(issue.Id, Is.GreaterThan(0));
                Assert.That(issue.Number, Is.GreaterThan(0));
                Assert.That(issue.Title, Is.Not.Empty);
                Assert.That(issue.Title, Is.EqualTo(expectedTitle));
            });

            Console.WriteLine(issue.Number);
            _lastCreatedIssueNumber = issue.Number;
        }

        [Test, Order (6)]
        public void Test_CreateCommentOnGitHubIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var issueNumber = _lastCreatedIssueNumber;
            var body = "Give Some Description";

            // Act
            var comment = _client.CreateCommentOnGitHubIssue(repo, issueNumber, body);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(comment.Id, Is.GreaterThan(0));
                Assert.That(comment.Body, Is.EqualTo(body));
            });

            Console.WriteLine(comment.Id);
            _lastCreatedCommentId = comment.Id;
            _lastCreatedCommentBody = comment.Body;
        }

        [Test, Order (7)]
        public void Test_GetCommentById()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var commentId = _lastCreatedCommentId;

            // Act
            var comment = _client.GetCommentById(repo, commentId);

            // Assert
            Assert.IsNotNull(comment, "Expected to retrieve a comment, but got null.");
            Assert.That(comment.Id, Is.EqualTo(commentId), "The retrieved comment ID should match the requested comment ID.");
            Assert.That(comment.Body, Is.EqualTo(_lastCreatedCommentBody), "The retrieved comment body should match the requested comment body.");
        }


        [Test, Order (8)]
        public void Test_EditCommentOnGitHubIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var commentId = _lastCreatedCommentId;
            var newBody = "This is the updated text of the comment";

            // Act
            var updatedComment = _client.EditCommentOnGitHubIssue(repo, commentId, newBody);

            // Assert
            Assert.IsNotNull(updatedComment, "The updated comment should be not null.");
            Assert.That(updatedComment.Id, Is.EqualTo(commentId), "The updated comment ID should match the original comment ID.");
            Assert.That(updatedComment.Body, Is.EqualTo(newBody), "The updated comment text should match the new body text.");
        }

        [Test, Order (9)]
        public void Test_DeleteCommentOnGitHubIssue()
        {
            // Arrange
            var repo = "test-nakov-repo";
            var commentId = _lastCreatedCommentId;

            // Act
            var result = _client.DeleteCommentOnGitHubIssue(repo, commentId);

            // Assert
            Assert.IsTrue(result, "The comment should be successfully deleted.");
        }
    }
}

