namespace AwningsAPI.Interfaces
{
    public interface IEmailProcessorService
    {
        Task ProcessIncomingEmailsAsync();
        Task ProcessSingleEmailAsync(string emailId);
    }
}
