using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Domain.Entities
{
    public class modules
    {
        [Key]
        public int module_id { get; set; }
        public string module_name { get; set; }
    }
}
