using System.ComponentModel.DataAnnotations;

namespace MehrShopping.Api.Requests
{
    public class DeleteProductRequest
    {
        [Required]
        public int Id { get; set; }
    }
}
