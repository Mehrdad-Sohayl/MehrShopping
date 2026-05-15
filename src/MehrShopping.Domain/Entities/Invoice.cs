using MehrShopping.Domain.Errors;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.ValueObjects;

namespace MehrShopping.Domain.Entities
{
    public class Invoice
    {
        public Customer Customer { get; private set; }

        private readonly List<InvoiceItem> _items = new List<InvoiceItem>();

        public IReadOnlyCollection<InvoiceItem> Items
        {
            get
            {
                return _items;
            }
        }


        public Invoice(Customer customer)
        {
            if (customer == null)
            {
                var errors = new List<DomainError>();
                errors.Add(new DomainError(DomainErrorCodes.InvalidCustomer, nameof(customer)));
                throw new DomainException(errors);
            }

            Customer = customer;
        }


        public void AddItem(InvoiceItem item)
        {
            if (item == null)
                throw new DomainException(new DomainError(DomainErrorCodes.InvalidItem, nameof(item)));

            if (_items.Any(i => i.Product.Id == item.Product.Id))
                throw new DomainException(new DomainError(DomainErrorCodes.DuplicateInvoiceItem, nameof(Product)));

            if (item.Quantity.Value <= 0)
                throw new DomainException(new DomainError(DomainErrorCodes.InvalidQuantity, nameof(Quantity)));

            _items.Add(item);
        }

        #region EF

        public int Id { get; private set; }
        private Invoice()
        {
            _items = new List<InvoiceItem>();
        }


        #endregion
    }
}
