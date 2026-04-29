namespace AwningsEmailFunction.Interfaces;

public interface IEmailProcessorService
{
    Task ProcessIncomingEmailAsync(string messageId, string mailboxEmail);
}
