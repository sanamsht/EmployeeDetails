namespace WorkingWithMultipleTable_Prod.Models.ViewModel.Email
{
    public class GetEmailSetting
    {
        public string EmailKey { get; set; } = default!;
        public string From { get; set; } = default!;
        public string SmtpServer { get; set; } = default!;

        public int  Port { get; set; }
        public bool EnableSSL { get; set; }
    }
}
