using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpSolution.Application.Interfaces
{
    public interface IUserInformationRepository
    {
        Task<string> NIK();

        Task<string> FullName();
    }
}
