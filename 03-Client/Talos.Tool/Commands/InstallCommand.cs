using Spectre.Console;
using Spectre.Console.Cli;
using Talos.Tool.Interfaces;
using Talos.Tool.Models;

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
        
        [CommandOption("-y|--yes")]
        public bool AutoYes { get; set; }
        
        [CommandOption("-c|--cwd")]
        public string? WorkingDirectory { get; set; }
    }

    public async Task RunTemplateAsync(
        TemplateJson template,
        Settings settings,
        CancellationToken cancellationToken)
    {
        var os = _osProvider.GetCurrentOSString().ToLower();
        var autoYes = settings.AutoYes ? "--yes" : string.Empty;
        var cwd = settings.WorkingDirectory ?? Directory.GetCurrentDirectory();

        foreach (var dep in template.Dependencies)
        {
            if (!dep.Commands.TryGetValue(os, out var commandsForOS))
            {
                AnsiConsole.MarkupLine($"[yellow]Skipping {dep.Name}: no commands for {os}[/]");
                continue;
            }

            foreach (var rawCmd in commandsForOS)
            {
                var cmd = rawCmd
                    .Replace("{{PROJECT_NAME}}", settings.Template)
                    .Replace("{{AUTO_YES}}", autoYes)
                    .Replace("{{CWD}}", cwd);

                var ok = await _commandExecutor.ExecuteCommandAsync(
                    cmd,
                    cwd,
                    settings.Verbose
                );

                if (!ok)
                    throw new Exception($"Command failed: {cmd}");
            }
        }
    }

    public override async Task<int> ExecuteAsync(
        CommandContext context,
        Settings settings,
        CancellationToken cancellationToken)
    {
        try
        {
            // ─────────────────────────────────────────────
            // VERBOSE
            // ─────────────────────────────────────────────
            if (settings.Verbose)
                PrintVerbose(settings);

            // Título
            AnsiConsole.Write(new Rule("[yellow]Installing Template[/]").RuleStyle("grey"));

            // Estado de “cargando template”
            var template = await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots)
                .SpinnerStyle(Style.Parse("green"))
                .StartAsync("Loading template...", async _ =>
                {
                    await Task.Delay(300); // opcional
                    return await _templateService.GetTemplateAsync(settings.Template);
                });
            
            if (template == null)
            {
                AnsiConsole.MarkupLineInterpolated(
                    $"[red]Template '{Markup.Escape(settings.Template)}' not found.[/]"
                );
                return 1;
            }
            

            // ─────────────────────────────────────────────
            // PANEL DEL TEMPLATE
            // ─────────────────────────────────────────────
            var panel = new Panel(
                    $"[bold]{Markup.Escape(template.Name)}[/]\n" +
                    $"{Markup.Escape(template.Description)}"
                )
                .Header("Template Found 🎉")
                .BorderColor(Color.Green);

            AnsiConsole.Write(panel);

            // ─────────────────────────────────────────────
            // INFO EXTRA
            // ─────────────────────────────────────────────
            AnsiConsole.MarkupLine($"Author: [blue]{Markup.Escape(template.Author)}[/]");
            AnsiConsole.MarkupLine($"Dependencies: [green]{template.Dependencies.Count}[/]");
            AnsiConsole.MarkupLine(
                $"OS Detected: [yellow]{_osProvider.GetCurrentOSString().ToUpper()}[/]"
            );
            
            await RunTemplateAsync(template, settings, cancellationToken);

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
            return 1;
        }
    }

    // ─────────────────────────────────────────────
    // VERBOSE PANEL
    // ─────────────────────────────────────────────
    private void PrintVerbose(Settings settings)
    {
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow("[grey]Command:[/]", "install");
        grid.AddRow("[grey]Template slug:[/]", Markup.Escape(settings.Template));
        grid.AddRow("[grey]Working directory:[/]", Markup.Escape(Directory.GetCurrentDirectory()));
        grid.AddRow("[grey]OS detected:[/]", _osProvider.GetCurrentOSString());
        grid.AddRow("[grey].NET runtime:[/]", Markup.Escape(Environment.Version.ToString()));
        grid.AddRow("[grey]Platform:[/]", Markup.Escape(Environment.OSVersion.ToString()));

        var panel = new Panel(grid)
            .Header("Verbose Mode")
            .BorderColor(Color.Grey);

        AnsiConsole.Write(panel);
    }
}
