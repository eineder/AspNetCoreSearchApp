using System.ComponentModel.DataAnnotations;

namespace AspNetCoreSearchApp.Models
{
    public class JsonSchema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string SchemaName { get; set; } // A name to identify the schema

        [Required]
        public string SchemaData { get; set; } // Store JSON schema as a string
    }
}
