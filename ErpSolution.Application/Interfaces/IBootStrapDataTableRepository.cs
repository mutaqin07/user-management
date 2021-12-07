using ErpSolution.Application.DTOs.AuxiliaryModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces
{
    public interface IBootStrapDataTableRepository<T>
    {
        Task<IActionResult> GetData(string condition = null, Dictionary<string,object> parameters = null);
    }
}
