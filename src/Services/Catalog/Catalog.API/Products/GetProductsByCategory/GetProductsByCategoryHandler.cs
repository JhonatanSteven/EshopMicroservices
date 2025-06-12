using BuildingBlocks.Pagination;
using Marten.Pagination;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryQuery(string Category, PaginationRequest PaginationRequest) : IQuery<GetProductsByCategoryResult>;

    public record GetProductsByCategoryResult(PaginatedResult<Product> Products);

    internal class GetProductsByCategoryHandler(IDocumentSession session, ILogger<GetProductsByCategoryHandler> logger)
        : IQueryHandler<GetProductsByCategoryQuery, GetProductsByCategoryResult>
    {
        public async Task<GetProductsByCategoryResult> Handle(GetProductsByCategoryQuery request, CancellationToken cancellationToken)
        {
            logger.LogInformation("GetProductsByCategoryHandler.Handle call with category: {Category}", request.Category);

            var pageIndex = request.PaginationRequest.PageIndex;
            var pageSize = request.PaginationRequest.PageSize;

            // Total de productos en la categoría
            var totalCount = await session.Query<Product>()
                .Where(p => p.Category.Contains(request.Category))
                .CountAsync(cancellationToken);

            // Productos paginados
            var products = await session.Query<Product>()
                .Where(p => p.Category.Contains(request.Category))
                .ToPagedListAsync(pageIndex, pageSize, cancellationToken);

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var paginatedResult = new PaginatedResult<Product>(
                totalPages,
                pageIndex,
                pageSize,
                totalCount,
                products
            );

            logger.LogInformation("GetProductsByCategoryHandler.Handle Successfully: PageIndex={PageIndex}, PageSize={PageSize}, TotalPages={TotalPages}, TotalCount={TotalCount}, Returned={ReturnedCount}",
                pageIndex, pageSize, totalPages, totalCount, products.Count);

            return new GetProductsByCategoryResult(paginatedResult);
        }
    }
}
