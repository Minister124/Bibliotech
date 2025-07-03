using Bibliotech.Core.Abstractions;

namespace Bibliotech.Modules.Discovery.Application.Commands.Books;

public record SearchBooksCommand(
    string Query,
    int Page = 1,
    int PageSize = 20,
    string? Genre = null,
    string? Author = null) : IQuery<Result<SearchBooksResponse>>;

public record SearchBooksResponse(
    List<BookDto> Books,
    int TotalCount,
    int Page,
    int PageSize);

public record BookDto(
    string Id,
    string Title,
    string Author,
    string Genre,
    decimal Price,
    string CoverImageUrl);