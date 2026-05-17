namespace MehrShopping.Application.Services.Products.Commands
{
    public class RegisterProductCommand
    {
        public RegisterProductCommand(string name, int quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; init; }
        public int Quantity { get; init; }
    }
}
