namespace Catalog.API.Products.CreateProduct
{
    // Comando para crear un producto, contiene los datos necesarios
    public record CreateProductCommand(
        string Name,
        string Description,
        List<string> Category,
        string ImageFile,
        decimal Price
    ) : ICommand<CreateProductResult>;

    // Resultado devuelto tras crear un producto, contiene el Id generado
    public record CreateProductResult(Guid Id);

    // Handler encargado de procesar el comando de creación de producto
    internal class CreateProductHandler(IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        /// <summary>
        /// Maneja la creación de un nuevo producto.
        /// </summary>
        /// <param name="command">Comando con los datos del producto a crear.</param>
        /// <param name="cancellationToken">Token de cancelación para la operación asíncrona.</param>
        /// <returns>Resultado con el Id del producto creado.</returns>
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Crear la entidad Product a partir de los datos del comando
            var product = new Product
            {
                Name = command.Name,
                Description = command.Description,
                Category = command.Category,
                ImageFile = command.ImageFile,
                Price = command.Price,
            };

            // Almacenar el producto en la base de datos usando Marten
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // Devolver el resultado con el Id del producto creado
            return new CreateProductResult(product.Id);
        }
    }
}
