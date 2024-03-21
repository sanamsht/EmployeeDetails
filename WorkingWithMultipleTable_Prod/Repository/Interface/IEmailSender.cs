namespace WorkingWithMultipleTable_Prod.Repository.Interface
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string email, string Subject, string message);
        string GetEmailBody(string? user, string? EmailTemplateName, string? callBackUrl, string? title);
       
    }
}
