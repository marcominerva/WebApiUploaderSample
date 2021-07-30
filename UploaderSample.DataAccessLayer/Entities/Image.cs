using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UploaderSample.DataAccessLayer.Entities
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(255)]
        [Required]
        public string Path { get; set; }

        public int Length { get; set; }
    }
}
