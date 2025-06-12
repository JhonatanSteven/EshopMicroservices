
using Catalog.API.Products.GetProducts;

namespace Catalog.API.Products.GetProductById
{
    public record GetProductByIdResponse(Product Product);
    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id:guid}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);
                var response = new GetProductByIdResponse(result.Product);
                return Results.Ok(response);
            })
            .WithTags("Product")
            .WithName("GetProductById")
            .WithSummary("Get Products By Id")
            .WithDescription("Get Products By Id")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest); ;
        }
    }
}
