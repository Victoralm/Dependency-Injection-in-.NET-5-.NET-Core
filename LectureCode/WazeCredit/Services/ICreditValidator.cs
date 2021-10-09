using System.Collections.Generic;
using System.Threading.Tasks;
using WazeCredit.Models;

namespace WazeCredit.Services
{
    public interface ICreditValidator
    {
        Task<(bool, IEnumerable<string>)> PassAllValidations(CreditApplication model);
    }
}
