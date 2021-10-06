namespace WazeCredit.Utility.AppSettingsClasses
{
    public class TwilioSettings
    {
        // The properties names must match the ones on the appsettings.json
        public string PhoneNumber { get; set; }
        public string AuthToken { get; set; }
        public string AccountSid { get; set; }
    }
}
