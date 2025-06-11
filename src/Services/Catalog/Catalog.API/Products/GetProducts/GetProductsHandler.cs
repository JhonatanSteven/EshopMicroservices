using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductsResult>;
public record GetProductsResult(
    int TotalCount,
    IEnumerable<Product> Products,
    int CurrentPage,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);

internal class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageNumber = query.PageNumber ?? 1;
        var pageSize = query.PageSize ?? 10;

        // Total de productos en la base de datos
        var totalCount = await session.Query<Product>().CountAsync(cancellationToken);

        // Productos paginados
        var products = await session.Query<Product>()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new GetProductsResult(
            totalCount,
            products,
            pageNumber,
            pageSize,
            totalPages,
            pageNumber < totalPages,
            pageNumber > 1
        );
    }
}
