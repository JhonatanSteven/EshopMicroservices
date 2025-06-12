using BuildingBlocks.Pagination;

namespace Catalog.API.Products.GetProducts
{
    public record GetProductsResponse(PaginatedResult<Product> Products);

    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var query = new GetProductsQuery(request);

                var result = await sender.Send(query);

                var response = result.Adapt<GetProductsResponse>();

                return Results.Ok(response);
            })
            .WithTags("Product")
            .WithName("GetProducts")
            .WithSummary("Get Products")
            .WithDescription("Get Products")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
