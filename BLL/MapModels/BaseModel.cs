using System.ComponentModel.DataAnnotations;

namespace Business.MapModels
{
    public class BaseModel
    {
        [Required]
        public int Id { get; set; }
    }
}
