using Bookstore.Application.DTOs;
using Bookstore.Application.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Bookstore.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("per-user")] // applies sliding window to ALL endpoints in this controller
public class BooksController : ControllerBase
{
    private readonly IBookService _service;
    private readonly IValidator<CreateBookRequest> _createValidator;
    private readonly IValidator<UpdateBookRequest> _updateValidator;

    // DI container supplies all three — you don't create them yourself
    public BooksController(
        IBookService service,
        IValidator<CreateBookRequest> createValidator,
        IValidator<UpdateBookRequest> updateValidator)
    {
        _service = service;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET api/books
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var books = await _service.GetAllAsync(ct);
        return Ok(books);
    }

    // GET api/books/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var book = await _service.GetByIdAsync(id, ct);

        // If service returns null, the book doesn't exist → 404
        if (book is null)
            return NotFound();

        return Ok(book);
    }

    // POST api/books
    [HttpPost]
    public async Task<IActionResult> Create(CreateBookRequest request, CancellationToken ct)
    {
        // Run the validator — if it fails, return 400 with validation errors
        var validation = await _createValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var book = await _service.CreateAsync(request, ct);

        // 201 Created — with a Location header pointing to the new resource
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT api/books/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateBookRequest request, CancellationToken ct)
    {
        var validation = await _updateValidator.ValidateAsync(request, ct);
        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var book = await _service.UpdateAsync(id, request, ct);
        return Ok(book);
    }

    // DELETE api/books/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return NoContent(); // 204 — success, nothing to return
    }
}