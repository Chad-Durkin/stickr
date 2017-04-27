using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stickr.Models
{
    [Table("Accounts")]
    public class Account
    {
        [Key]
        public int Id { get; set; }
        public string Biography { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Image> Images { get; set; }

    }
}
