using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Domain.Entities
{
    public class policy_mst
    {
        [Key]
        public int policy_id { get; set; }
        public string policy_name { get; set; }
        public string description { get; set; }
    }
}
