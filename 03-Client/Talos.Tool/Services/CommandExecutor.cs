using CliWrap;
using Spectre.Console;
using Talos.Tool.Interfaces;

namespace Talos.Tool.Services;

public class CommandExecutor : ICommandExecutor
{
    public async Task<bool> ExecuteCommandAsync(string command, string? workingDirectory = null, bool verbose = false)
    {
        try
        {
            if (verbose)
            {
                AnsiConsole.MarkupLineInterpolated($"[grey]Executing: {command}[/]");
            }

            var result = await Cli.Wrap("sh")
                .WithArguments(["-c", command])
                .WithWorkingDirectory(workingDirectory ?? Directory.GetCurrentDirectory())
                .ExecuteAsync();

            return result.ExitCode == 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error executing command: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ExecuteCommandsAsync(IEnumerable<string> commands, string? workingDirectory = null, bool verbose = false)
    {
        foreach (var command in commands)
        {
            var success = await ExecuteCommandAsync(command, workingDirectory, verbose);

            if (!success)
            {
                return false;
            }
        }

        return true;
    }
}