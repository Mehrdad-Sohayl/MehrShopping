using System.ComponentModel.DataAnnotations;

namespace MehrShopping.Api.Requests
{
    public class RegisterCustomerRequest
    {
        [Required]
        public string NationalCode { get; set; }
    }
}
