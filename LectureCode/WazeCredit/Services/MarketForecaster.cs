using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class MarketForecaster
    {
        public MarketResult GetMarketPrediction()
        {
            // Call API to do some complex calculations and current stock market forecast

            return new MarketResult
            {
                MarketCondition = MarketCondition.StableUp,
            };
        }
    }

    public class MarketResult
    {
        public MarketCondition MarketCondition {  get; set;}
    }
}
