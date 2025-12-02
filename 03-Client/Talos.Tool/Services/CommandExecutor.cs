using CliWrap;
using CliWrap.EventStream;
using Spectre.Console;
using Talos.Tool.Interfaces;

namespace Talos.Tool.Services;

public class CommandExecutor : ICommandExecutor
{
    public async Task<bool> ExecuteCommandAsync(string command, string? workingDirectory = null, bool verbose = false)
    {
        // Mantener un estado del working directory entre llamadas
        static string GetUpdatedWorkingDirectory(string currentCommand, string currentWorkingDir)
        {
            // Si el comando comienza con "cd ", extraer el nuevo directorio
            if (currentCommand.StartsWith("cd "))
            {
                var parts = currentCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    var newDir = parts[1];
                    
                    // Manejar cd .. y directorios relativos/absolutos
                    if (newDir == "..")
                    {
                        return Directory.GetParent(currentWorkingDir)?.FullName ?? currentWorkingDir;
                    }
                    else if (Path.IsPathRooted(newDir))
                    {
                        return newDir;
                    }
                    else
                    {
                        return Path.Combine(currentWorkingDir, newDir);
                    }
                }
            }
            
            // Si el comando tiene &&, buscar comandos cd dentro
            if (currentCommand.Contains("&&"))
            {
                var subCommands = currentCommand.Split("&&", StringSplitOptions.TrimEntries);
                foreach (var subCmd in subCommands)
                {
                    if (subCmd.StartsWith("cd "))
                    {
                        return GetUpdatedWorkingDirectory(subCmd, currentWorkingDir);
                    }
                }
            }
            
            return currentWorkingDir;
        }

        try
        {
            var actualWorkingDirectory = workingDirectory ?? Directory.GetCurrentDirectory();
            
            if (verbose)
            {
                AnsiConsole.MarkupLineInterpolated($"[grey]$ {Markup.Escape(command)} (in: {actualWorkingDirectory})[/]");
            }

            var cts = new CancellationTokenSource();
            
            var cmd = Cli.Wrap("bash")
                .WithArguments(["-c", command])
                .WithWorkingDirectory(actualWorkingDirectory)
                .WithValidation(CommandResultValidation.None);

            var exitCode = 0;
            var output = new System.Text.StringBuilder();
            var error = new System.Text.StringBuilder();

            await foreach (var cmdEvent in cmd.ListenAsync(cts.Token))
            {
                switch (cmdEvent)
                {
                    case StartedCommandEvent started:
                        break;
                        
                    case StandardOutputCommandEvent stdOut:
                        Console.Write(stdOut.Text);
                        output.Append(stdOut.Text);
                        break;
                        
                    case StandardErrorCommandEvent stdErr:
                        if (!string.IsNullOrWhiteSpace(stdErr.Text))
                        {
                            Console.Write(stdErr.Text);
                            error.Append(stdErr.Text);
                        }
                        break;
                        
                    case ExitedCommandEvent exited:
                        exitCode = exited.ExitCode;
                        break;
                }
            }

            // Actualizar el working directory para el próximo comando
            var newWorkingDir = GetUpdatedWorkingDirectory(command, actualWorkingDirectory);
            
            if (verbose && newWorkingDir != actualWorkingDirectory)
            {
                AnsiConsole.MarkupLineInterpolated($"[grey]Working directory changed to: {newWorkingDir}[/]");
            }

            if (exitCode == 0)
            {
                AnsiConsole.MarkupLine("[green]✓ Success[/]");
                return true;
            }
            else
            {
                AnsiConsole.MarkupLineInterpolated($"[red]✗ Failed (exit code: {exitCode})[/]");
                return false;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Error: {Markup.Escape(ex.Message)}[/]");
            return false;
        }
    }

    public async Task<bool> ExecuteCommandsAsync(IEnumerable<string> commands, string? workingDirectory = null, bool verbose = false)
    {
        var currentWorkingDir = workingDirectory ?? Directory.GetCurrentDirectory();
        
        foreach (var command in commands)
        {
            var success = await ExecuteCommandAsync(command, currentWorkingDir, verbose);
            
            // Actualizar el working directory después de cada comando
            currentWorkingDir = GetUpdatedWorkingDirectory(command, currentWorkingDir);
            
            if (!success)
            {
                return false;
            }
        }
        return true;
    }
    
    private static string GetUpdatedWorkingDirectory(string command, string currentWorkingDir)
    {
        // Extraer el nuevo directorio de los comandos cd
        if (command.StartsWith("cd "))
        {
            var parts = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
            {
                var newDir = parts[1];
                return UpdatePath(currentWorkingDir, newDir);
            }
        }
        
        // Manejar comandos compuestos con &&
        if (command.Contains("&&"))
        {
            var subCommands = command.Split("&&", StringSplitOptions.TrimEntries);
            foreach (var subCmd in subCommands)
            {
                if (subCmd.StartsWith("cd "))
                {
                    var parts = subCmd.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        var newDir = parts[1];
                        return UpdatePath(currentWorkingDir, newDir);
                    }
                }
            }
        }
        
        return currentWorkingDir;
    }
    
    private static string UpdatePath(string currentDir, string newPath)
    {
        if (newPath == "..")
        {
            return Directory.GetParent(currentDir)?.FullName ?? currentDir;
        }
        else if (Path.IsPathRooted(newPath))
        {
            return newPath;
        }
        else
        {
            return Path.Combine(currentDir, newPath);
        }
    }
}