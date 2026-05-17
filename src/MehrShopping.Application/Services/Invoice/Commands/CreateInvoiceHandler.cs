using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceHandler(
            ICustomerRepository customerRepository,
            IProductRepository productRepository,
            IInvoiceRepository invoiceRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _invoiceRepository = invoiceRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Domain.Entities.Invoice>> Handle(CreateInvoiceCommand command)
        {
            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer == null)
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerNotFound, nameof(customer)));

            if (!command.Items.Any())
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.EmptyInvoiceItem, nameof(command.Items)));

            var invoice = Domain.Entities.Invoice.Create(customer);

            foreach (var item in command.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                    return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.ProductNotFound, nameof(product)));

                if (product.StockQuantity.Value <= 0 || product.StockQuantity.Value < item.Quantity)
                    return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.ProductOutOfStock, nameof(product)));

                product.DecreaseStock(item.Quantity);
                _productRepository.Update(product);

                var invoiceItem = InvoiceItem.Create(product, item.Quantity);

                invoice.AddItem(invoiceItem);
            }

            if (!invoice.Items.Any())
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.EmptyInvoiceItem, nameof(invoice)));

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Invoice>.Success(invoice);

        }
    }
}
