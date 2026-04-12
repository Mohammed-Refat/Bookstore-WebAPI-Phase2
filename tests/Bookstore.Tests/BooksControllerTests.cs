using System.Net;
using System.Net.Http.Json;
using Bookstore.Application.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Bookstore.Tests;

// WebApplicationFactory spins up the real app in memory
public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        // CreateClient gives us an HttpClient wired directly to the in-memory app
        // No real HTTP port — requests go through the pipeline directly
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        // Act — send a real GET request through the full pipeline
        var response = await _client.GetAsync("/api/books");

        // Assert — verify the status code
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Create_ValidBook_Returns201()
    {
        // Arrange — build a valid request
        var request = new CreateBookRequest("The Pragmatic Programmer", "David Thomas", 35.00m, 50);

        // Act — POST it to the real endpoint
        var response = await _client.PostAsJsonAsync("/api/books", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var book = await response.Content.ReadFromJsonAsync<BookDto>();
        book.Should().NotBeNull();
        book!.Title.Should().Be("The Pragmatic Programmer");
        book.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Create_InvalidBook_Returns400()
    {
        // Arrange — invalid request: empty title, negative price
        var request = new CreateBookRequest("", "Someone", -5m, 10);

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", request);

        // Assert — validator should reject it before reaching the service
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetById_NonExistentId_Returns404()
    {
        // Arrange — a guid that doesn't exist
        var fakeId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/books/{fakeId}");

        // Assert — should hit the null check in the controller
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_ExistingBook_Returns204()
    {
        // Arrange — first create a book so we have something to delete
        var request = new CreateBookRequest("Book To Delete", "Author", 10m, 5);
        var createResponse = await _client.PostAsJsonAsync("/api/books", request);
        var created = await createResponse.Content.ReadFromJsonAsync<BookDto>();

        // Act — delete it
        var response = await _client.DeleteAsync($"/api/books/{created!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}