using Spectre.Console;
using Spectre.Console.Cli;

namespace Talos.Tool.Commands;

public class HelloCommand : Command<HelloCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("-n|--name")]
        public string? Name { get; set; }
    }

    public override int Execute(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var name = Markup.Escape(settings.Name ?? "World");
        
        AnsiConsole.Write(new Rule($"[yellow]Talos CLI[/]").RuleStyle("grey"));

        var panel = new Panel($"[green]Hello {name}![/]\nThis is the Talos CLI tool.")
            .Header("Welcome")
            .BorderColor(Color.Yellow);
        
        AnsiConsole.Write(panel);
        
        // Show system information
        var grid = new Grid();
        grid.AddColumn();
        grid.AddColumn();

        grid.AddRow("[bold]Version:[/]", "1.0.0");
        grid.AddRow("[bold].NET Version:[/]", Markup.Escape(Environment.Version.ToString()));
        grid.AddRow("[bold]OS:[/]", $"{Markup.Escape(Environment.OSVersion.Platform.ToString())} {Markup.Escape(Environment.OSVersion.Version.ToString())}");
        
        AnsiConsole.Write(grid);
        
        return 0;
    }
}