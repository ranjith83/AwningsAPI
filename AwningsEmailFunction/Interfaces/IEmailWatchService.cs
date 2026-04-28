namespace AwningsEmailFunction.Interfaces;

public interface IEmailWatchService
{
    Task SaveEmailAsync(string messageId);
}
