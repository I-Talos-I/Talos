namespace Talos.Tool.Interfaces;

public interface ICommandExecutor
{
    Task<bool> ExecuteCommandAsync(string command, string? workingDirectory = null, bool verbose = false);

    Task<bool> ExecuteCommandsAsync(IEnumerable<string> commands, string? workingDirectory = null,
        bool verbose = false);
}