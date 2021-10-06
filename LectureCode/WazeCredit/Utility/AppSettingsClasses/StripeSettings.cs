namespace WazeCredit.Utility.AppSettingsClasses
{
    public class StripeSettings
    {
        // The properties names must match the ones on the appsettings.json
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
    }
}
