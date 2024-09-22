using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSearchApp.Models
{
    public class JsonDoc
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string DocumentName { get; set; }

        [Required]
        public string JsonData { get; set; } // Store JSON data as string
    }
}
