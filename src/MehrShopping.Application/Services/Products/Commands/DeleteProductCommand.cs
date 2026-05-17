namespace MehrShopping.Application.Services.Products.Commands
{
    public class DeleteProductCommand
    {
        public int Id { get; init; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }
}
