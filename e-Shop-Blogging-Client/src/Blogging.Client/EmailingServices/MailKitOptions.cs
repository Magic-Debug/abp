using MailKit.Security;

namespace Blogging.Client.EmailingServices;

public class MailKitOptions
{
    public SecureSocketOptions? SecureSocketOption { get; set; }
}
