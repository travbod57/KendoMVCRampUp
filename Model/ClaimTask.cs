using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ClaimTask
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Owner { get; set; }
    }
}
