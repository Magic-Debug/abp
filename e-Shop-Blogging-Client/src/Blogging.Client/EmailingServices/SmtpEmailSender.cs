using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Blogging.Client.EmailingServices;

public class SmtpEmailSender : EmailSenderBase
{
    public SmtpEmailSender(IOptionsSnapshot<MailConfigOptions> settings)
        : base(settings.Get("Smtp"))
    {
    }

    public Task<SmtpClient> BuildClientAsync()
    {
        string host = ConfigOptions.Host;
        int port = ConfigOptions.Port;

        SmtpClient smtpClient = new SmtpClient(host, port);

        try
        {
            smtpClient.EnableSsl = ConfigOptions.EnableSsl;
            smtpClient.UseDefaultCredentials = ConfigOptions.UseDefaultCredentials;
            string userName = ConfigOptions.UserName;
            if (!string.IsNullOrEmpty(userName))
            {
                string password = ConfigOptions.Password;
                var domain = ConfigOptions.Domain;
                smtpClient.Credentials = !string.IsNullOrEmpty(domain)
                    ? new NetworkCredential(userName, password, domain)
                    : new NetworkCredential(userName, password);
            }

            return Task.FromResult(smtpClient);
        }
        catch
        {
            smtpClient.Dispose();
            throw;
        }
    }

    protected override async Task SendEmailAsync(MailMessage mail)
    {
        using var smtpClient = await BuildClientAsync();
        await smtpClient.SendMailAsync(mail);
    }
}
