namespace Catalog.API.Products.UpdateProduct
{
    public record UpdateProductRequest(
        Guid Id,
        string Name,
        string Description,
        List<string> Category,
        string ImageFile,
        decimal Price
    );

    public record UpdateProductResponse(bool IsSuccess);

    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateProductResponse>();

                return Results.Ok(response);
            })
            .WithTags("Product")
            .WithName("UpdateProduct")
            .WithSummary("Update Product")
            .WithDescription("Actualiza un producto existente")
            .Produces<UpdateProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
