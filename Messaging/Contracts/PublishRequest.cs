using System.ComponentModel.DataAnnotations;

namespace webapi2.Controllers
{
    public class PublishRequest
    {
        [Required]
        public string Message { get; set; }
    }
}
