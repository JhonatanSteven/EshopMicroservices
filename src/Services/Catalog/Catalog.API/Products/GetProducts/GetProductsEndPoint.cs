namespace Catalog.API.Products.GetProducts
{
    public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);
    public record GetProductsResponse(
        int TotalCount,
        IEnumerable<Product> Products,
        int CurrentPage,
        int PageSize,
        int TotalPages,
        bool HasNextPage,
        bool HasPreviousPage
    );

    public class GetProductsEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
            {
                var query = new GetProductsQuery(request.PageNumber, request.PageSize);

                var result = await sender.Send(query);

                // Mapeo explícito para asegurar que todos los campos se transfieren correctamente
                var response = new GetProductsResponse(
                    result.TotalCount,
                    result.Products,
                    result.CurrentPage,
                    result.PageSize,
                    result.TotalPages,
                    result.HasNextPage,
                    result.HasPreviousPage
                );

                return Results.Ok(response);
            })
            .WithName("GetProducts")
            .WithTags("Product")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
        }
    }
}
