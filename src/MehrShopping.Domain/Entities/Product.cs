using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace MehrShopping.Domain.Entities
{
    public class Product
    {
        public Name Name { get; private set; }
        public Quantity StockQuantity { get; private set; }


        private Product(Name name, Quantity stockQuantity)
        {
            Name = name;
            StockQuantity = stockQuantity;
        }

        public static Product Create(string name, int quantity)
        {
            var errors = new List<DomainError>();

            var _name = Name.Create(name, errors);
            var _stockQuantity = Quantity.Create(quantity, errors);

            if (errors.Any())
                throw new DomainException(errors);

            return new Product(_name, _stockQuantity);
        }

        public void DeleteProduct(int id)
        {
            if (StockQuantity.Value > 0)
                throw new DomainException(new DomainError(DomainErrorCodes.ItemsRemainInStock, nameof(DeleteProduct)));
        }

        public void DecreaseStock(int quantity)
        {
            if (StockQuantity.Value < quantity)
                throw new DomainException(new DomainError(DomainErrorCodes.ProductOutOfStock, nameof(DecreaseStock)));

            var errors = new List<DomainError>();

            int _quantity = StockQuantity.Value - quantity;

            var newQuantity = Quantity.Create(_quantity, errors);
            if (errors.Any())
                throw new DomainException(errors);

            StockQuantity = newQuantity;
        }

        #region EF

        public int Id { get; private set; }
        private Product() { }

        private List<InvoiceItem> _items = new List<InvoiceItem>();

        public ICollection<InvoiceItem> Items
        {
            get
            {
                return _items;
            }
        }

        [Timestamp]
        public byte[] RowVersion { get; private set; }

        #endregion
    }
}
