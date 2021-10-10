using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditApprovedHigh : ICreditApproved
    {
        double ICreditApproved.GetCreditApproved(CreditApplication creditApplication)
        {
            // have a different logic to calculate approval limit
            // we'll hardcode to 30% of salary
            return creditApplication.Salary * .3;
        }
    }
}