using System.ComponentModel.DataAnnotations;

namespace MehrShopping.Api.Requests
{
    public class RegisterProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
