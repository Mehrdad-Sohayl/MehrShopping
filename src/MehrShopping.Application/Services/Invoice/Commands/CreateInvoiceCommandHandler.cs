using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Application.Services.Invoice.Commands
{
    public class CreateInvoiceCommandHandler
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceCommandHandler(
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
            if (command is null)
                return Result<Domain.Entities.Invoice>.Failure(
                    new ApplicationError(ApplicationErrorCodes.RequestValidation, nameof(command)));

            if (command.Items is null || command.Items.Count == 0)
                return Result<Domain.Entities.Invoice>.Failure(
                    new ApplicationError(ApplicationErrorCodes.EmptyInvoiceItem, nameof(command.Items)));

            if (command.Items.Any(i => i.Quantity <= 0))
                return Result<Domain.Entities.Invoice>.Failure(
                    new ApplicationError(ApplicationErrorCodes.RequestValidation, nameof(command.Items)));

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer is null)
                return Result<Domain.Entities.Invoice>.Failure(
                    new ApplicationError(ApplicationErrorCodes.CustomerNotFound, nameof(command.CustomerId)));

            var groupedItems = command.Items
                .GroupBy(i => i.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(i => i.Quantity)
                })
                .ToList();

            var products = await _productRepository
                .GetByIdsAsync(groupedItems.Select(i => i.ProductId).ToList());

            var productsById = products.ToDictionary(p => p.Id);

            var errors = new List<ApplicationError>();

            foreach (var item in groupedItems)
            {
                if (!productsById.TryGetValue(item.ProductId, out var product))
                {
                    errors.Add(new ApplicationError(
                        ApplicationErrorCodes.ProductNotFound, item.ProductId.ToString()));
                    continue;
                }

                if (product.StockQuantity.Value < item.Quantity)
                {
                    errors.Add(new ApplicationError(
                        ApplicationErrorCodes.ProductOutOfStock, item.ProductId.ToString()));
                }
            }

            if (errors.Count > 0)
                return Result<Domain.Entities.Invoice>.Failure(errors);

            var invoice = Domain.Entities.Invoice.Create(customer);

            foreach (var item in groupedItems)
            {
                var product = productsById[item.ProductId];

                product.DecreaseStock(item.Quantity);
                invoice.AddItem(InvoiceItem.Create(product, item.Quantity));
            }

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Invoice>.Success(invoice);
        }
    }
}
