using System.Diagnostics.CodeAnalysis;
using Spectre.Console;
using Spectre.Console.Cli;
using Talos.Tool.Interfaces;

namespace Talos.Tool.Commands;

public class InstallCommand : AsyncCommand<InstallCommand.Settings>
{
    private readonly ITemplateService _templateService;
    private readonly ICommandExecutor _commandExecutor;
    private readonly IOperatingSystemProvider _osProvider;

    public InstallCommand(
        ITemplateService templateService,
        ICommandExecutor commandExecutor,
        IOperatingSystemProvider osProvider)
    {
        _templateService = templateService;
        _commandExecutor = commandExecutor;
        _osProvider = osProvider;
    }

    public class Settings : CommandSettings
    {
        [CommandArgument(0, "<TEMPLATE>")]
        public string Template { get; set; } = string.Empty;
        
        [CommandOption("-v|--verbose")]
        public bool Verbose { get; set; }
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        try
        {
            AnsiConsole.Write(new Rule($"[yellow]Installing Template[/]").RuleStyle("grey"));

            await AnsiConsole.Status()
                .StartAsync("Loading template...", async ctx =>
                {
                    ctx.Spinner(Spinner.Known.Dots);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    await Task.Delay(2000);
                });

            var template = await _templateService.GetTemplateAsync(settings.Template);

            if (template == null)
            {
                AnsiConsole.MarkupLineInterpolated($"[red]Template: '{settings.Template} not found.[/]");
                return 1;
            }
            
            // Show panel with information about the template.
            var panel = new Panel($"[bold]{template.Name}[/]\n{template.Description}")
                .Header("Template Found 🎉")
                .BorderColor(Color.Green);
                
            AnsiConsole.Write(panel);
            
            AnsiConsole.MarkupLineInterpolated($"Author: [blue]{template.Author}[/]");
            AnsiConsole.MarkupLineInterpolated($"Dependencies: [green]{template.Dependencies.Count}[/]");
            AnsiConsole.MarkupLineInterpolated($"OS Detected: [yellow]{_osProvider.GetCurrentOSString().ToUpper()}[/]");
            
            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return 1;
        }
    }
}