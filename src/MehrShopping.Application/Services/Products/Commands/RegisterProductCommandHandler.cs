using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Application.Services.Products.Commands
{
    public class RegisterProductCommandHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Product>> Handle(RegisterProductCommand command)
        {
            if (command is null) 
                return Result<Product>.Failure(new ApplicationError(ApplicationErrorCodes.RequestValidation, nameof(command)));

            var product = await _productRepository.GetByNameAsync(command.Name);
            if (product != null)
                return Result<Product>.Failure(new ApplicationError(ApplicationErrorCodes.ProductAlreadyExists, nameof(product)));

            var newProduct = Product.Create(command.Name, command.Quantity);
            await _productRepository.AddAsync(newProduct);
            await _unitOfWork.SaveChangesAsync();

            return Result<Product>.Success(newProduct);
        }
    }
}