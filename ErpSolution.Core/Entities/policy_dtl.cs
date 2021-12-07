using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Domain.Entities
{
    public class policy_dtl
    {
        [Key]
        public int policy_id { get; set; }
        [Key]
        public string claim_name { get; set; }
    }
}
