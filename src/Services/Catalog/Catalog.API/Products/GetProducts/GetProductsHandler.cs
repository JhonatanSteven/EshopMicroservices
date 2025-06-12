using BuildingBlocks.Pagination;
using Marten.Pagination;
using MediatR;
using System.Text.Json;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery(PaginationRequest PaginationRequest) : IQuery<GetProductsResult>;
public record GetProductsResult(PaginatedResult<Product> Products);

internal class GetProductsHandler
    (IDocumentSession session, ILogger<GetProductsHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductsHandler.Handle call with query: {@query}", query.PaginationRequest);
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;

        // Total de productos en la base de datos
        var totalCount = await session.Query<Product>().CountAsync(cancellationToken);

        // Productos paginados
        var products = await session.Query<Product>()
            .ToPagedListAsync(pageIndex, pageSize, cancellationToken);

        var pages = (int)Math.Ceiling((double)totalCount / pageSize);
        var results = new PaginatedResult<Product>(
                pages,
                pageIndex,
                pageSize,
                totalCount,
                products
                );
        logger.LogInformation(
            "GetProductsHandler.Handle Successfully: pageIndex={PageIndex}, pageSize={PageSize}, totalCount={TotalCount}, pageCount={PageCount}",
            pageIndex,
            pageSize,
            totalCount,
            pages);

        return new GetProductsResult(results);
    }
}
