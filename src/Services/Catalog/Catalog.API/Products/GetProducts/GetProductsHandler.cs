using BuildingBlocks.Pagination;
using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedResult<Product> Products);

internal class GetProductsQueryHandler
    (IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        // Total de productos en la base de datos
        var totalCount = await session.Query<Product>().CountAsync(cancellationToken);

        // Productos paginados
        var products = await session.Query<Product>()
            .ToPagedListAsync(pageIndex, pageSize, cancellationToken);

        var pages = (int)Math.Ceiling((double)totalCount / pageSize);

        return new GetProductsResult(
            new PaginatedResult<Product>(
                pages,
                pageIndex,
                pageSize,
                totalCount,
                products
                ));
    }
}
