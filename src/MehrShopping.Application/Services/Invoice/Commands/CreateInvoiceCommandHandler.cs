using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Exceptions;
using MehrShopping.Domain.Interfaces.Repositories;
using System.Text.RegularExpressions;

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
            var errors = new List<DomainError>();

            var customer = await _customerRepository.GetByIdAsync(command.CustomerId);
            if (customer == null)
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.CustomerNotFound, nameof(customer)));

            if (!command.Items.Any())
                return Result<Domain.Entities.Invoice>.Failure(new ApplicationError(ApplicationErrorCodes.EmptyInvoiceItem, nameof(command.Items)));

            var grouped = command.Items
                .GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    x.Key,
                    Qty = x.Sum(q => q.Quantity)
                })
                .ToList();

            foreach (var item in grouped)
            {
                var success = await _productRepository
                    .DecreaseStockIfAvailable(item.Key, item.Qty, errors);

                if (!success)
                {
                    return Result<Domain.Entities.Invoice>.Failure(
                        new ApplicationError("OutOfStock", item.Key.ToString()));
                }
            }

            var invoice = Domain.Entities.Invoice.Create(customer);

            foreach (var item in grouped)
            {
                var product = await _productRepository.GetByIdAsync(item.Key);
                invoice.AddItem(InvoiceItem.Create(product!, item.Qty));
            }

            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return Result<Domain.Entities.Invoice>.Success(invoice);
        }
    }
}