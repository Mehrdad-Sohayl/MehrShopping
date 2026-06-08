using MehrShopping.Application.Common;
using MehrShopping.Application.Interfaces;
using MehrShopping.Domain.Entities;
using MehrShopping.Domain.Interfaces.Repositories;

namespace MehrShopping.Application.Services.Products.Commands
{
    public class DeleteProductCommandHandler
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Product>> Handle(DeleteProductCommand command)
        {
            var product = await _productRepository.GetByIdAsync(command.Id);
            if (product == null)
                return Result<Product>.Failure(new ApplicationError(ApplicationErrorCodes.ProductNotFound, nameof(product)));

            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();

            return Result<Product>.Success(product);
        }
    }
}
