using System.ComponentModel.DataAnnotations;

namespace webapi2.Controllers
{
    public class SetCacheRequest
    {
        [Required]
        public string Value { get; set; }
    }
}
