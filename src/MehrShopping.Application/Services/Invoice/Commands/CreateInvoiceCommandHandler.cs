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
            var errors = new List<ApplicationError>();

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer == null)
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerNotFound, nameof(customer)));

            if (!command.Items.Any())
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.EmptyInvoiceItem, nameof(command.Items)));


            var groupedProducts = command.Items
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    Quantity = x.Sum(q => q.Quantity)
                }).ToList();

            var products = await _productRepository.GetByIdsAsync(groupedProducts.Select(i => i.ProductId).ToList());

            var productsDictionary = products.ToDictionary(d => d.Id);

            foreach (var item in groupedProducts)
            {
                if (!productsDictionary.TryGetValue(item.ProductId, out var product))
                {
                    errors.Add(new ApplicationError(ApplicationErrorCodes.ProductNotFound, item.ProductId.ToString()));
                    continue;
                }

                if (product!.StockQuantity.Value < item.Quantity)
                {
                    errors.Add(new ApplicationError(ApplicationErrorCodes.ProductOutOfStock, item.ProductId.ToString()));
                }
            }

            if (errors.Any())
                return Result<Domain.Entities.Invoice>.Failure(errors);

            var invoice = Domain.Entities.Invoice.Create(customer);

            foreach (var item in groupedProducts)
            {
                var product = productsDictionary[item.ProductId];
                product.DecreaseStock(item.Quantity);
                var invoiceItem = InvoiceItem.Create(product, item.Quantity);
                invoice.AddItem(invoiceItem);
            }

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Invoice>.Success(invoice);
        }
    }
}