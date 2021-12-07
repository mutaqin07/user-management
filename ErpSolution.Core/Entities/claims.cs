using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ErpSolution.Domain.Entities
{
    public class claims
    {
        [Key]
        public string claim_name { get; set; }

        public int module_id { get; set; }

        public string description { get; set; }
    }
}
