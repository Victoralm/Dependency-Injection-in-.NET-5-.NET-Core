using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditApprovedLow : ICreditApproved
    {
        double ICreditApproved.GetCreditApproved(CreditApplication creditApplication)
        {
            // have a different logic to calculate approval limit
            // we'll hardcode to 50% of salary
            return creditApplication.Salary * .5;
        }
    }
}