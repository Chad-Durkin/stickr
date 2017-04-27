using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stickr.Models
{
    [Table("Images")]
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public virtual Account Account { get; set; }
    }
}
