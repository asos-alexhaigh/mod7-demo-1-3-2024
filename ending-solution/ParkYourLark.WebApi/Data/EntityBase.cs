using System.ComponentModel.DataAnnotations;

namespace ParkYourLark.WebApi.Data
{
    public abstract class EntityBase 
    {
        [Key]
        public string Id { get; set; }
    }
}