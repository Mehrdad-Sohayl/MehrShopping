using MehrShopping.Application.Common;

namespace MehrShopping.Application.Exceptions
{
    public class ApplicationException : Exception
    {
        private readonly ApplicationError _error;

        public ApplicationException(ApplicationError error)
        {
            _error = error;
        }
    }
}
