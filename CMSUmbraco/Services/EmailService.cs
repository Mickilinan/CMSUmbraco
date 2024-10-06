using Azure;
using Azure.Communication.Email;
using Umbraco.Cms.Core.Models.Email;

namespace CMSUmbraco.Services;

public class EmailService
{
    public async Task<bool> SendEmailAsync(string email, string recipientName)
    {
        var connectionString = "endpoint=https://emailproviderumbraco.europe.communication.azure.com/;accesskey=CXlxeyy9PnXnSXgzaJQekRRLvI8301PBKC8QSeLWZtv1xP23PuWVJQQJ99AJACULyCpq7IbPAAAAAZCSpLGE";
        var emailClient = new EmailClient(connectionString);
        var emailContent = new EmailContent("Your Subject Here")
        {
            PlainText = "Hej " + recipientName + ",\n\nDetta är ett meddelande från oss.",
            Html = "<p>Hej " + recipientName + ",</p><p>Detta är ett meddelande från oss.</p>"
        };
        var emailMessage = new Azure.Communication.Email.EmailMessage(
            senderAddress: "donotreply@4b5d946f-af3e-41f2-a4c5-0fb24026f0d3.azurecomm.net",
            content: emailContent,
            recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(email) })
        );
        try
        {
            EmailSendOperation emailSendOperation = await emailClient.SendAsync(WaitUntil.Completed, emailMessage);
            return emailSendOperation.HasCompleted;
        }
        catch (RequestFailedException ex)
        {

            Console.WriteLine($"Error occurred: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return false;
        }
    }
}
