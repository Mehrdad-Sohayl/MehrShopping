using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;

namespace MehrShopping.Domain.Entities
{
    public class InvoiceItem
    {
        public Product Product { get; private set; }
        public Quantity Quantity { get; private set; }
        public Invoice Invoice { get; private set; }


        private InvoiceItem(Product product, Quantity quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public static InvoiceItem Create(Product product, int quantity)
        {
            var errors = new List<DomainError>();
            var _quantity = Quantity.Create(quantity, errors);

            if (errors.Any() || product == null)
                throw new DomainException(errors);

            return new InvoiceItem(product, _quantity);
        }

        #region

        private InvoiceItem() { }
        public int Id { get; private set; }
        public int ProductId { get; private set; }
        public int InvoiceId { get; private set; }

        #endregion
    }
}