namespace AwningsEmailFunction.Interfaces;

public interface IEmailAutoReplyService
{
    Task GenerateDraftReplyAsync(int taskId);
}
