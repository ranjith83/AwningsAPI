namespace AwningsEmailFunction.Interfaces;

public interface IEmailProcessorService
{
    Task ProcessEmailAsync(int incomingEmailId);
}
