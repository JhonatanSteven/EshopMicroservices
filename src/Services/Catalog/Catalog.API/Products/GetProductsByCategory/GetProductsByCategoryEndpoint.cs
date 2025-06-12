using BuildingBlocks.Pagination;

namespace Catalog.API.Products.GetProductsByCategory
{
    public record GetProductsByCategoryResponse(PaginatedResult<Product> Products);

    public class GetProductsByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (
                string category,
                [AsParameters] PaginationRequest request,
                ISender sender) =>
            {
                var query = new GetProductsByCategoryQuery(category, request);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsByCategoryResponse>();

                return Results.Ok(response);
            })
            .WithTags("Product")
            .WithName("GetProductsByCategory")
            .WithSummary("Get Products By Category")
            .WithDescription("Obtiene productos filtrados por categoría con paginación")
            .Produces<GetProductsByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
