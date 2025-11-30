using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;
using Talos.Tool.Commands;
using Talos.Tool.Interfaces;
using Talos.Tool.Services;
using Talos.Tool.Utilities;

var services = new ServiceCollection();

// Register Services
services.AddHttpClient();
services.AddSingleton<ITemplateService, TemplateService>();
services.AddSingleton<ICommandExecutor, CommandExecutor>();
services.AddSingleton<IOperatingSystemProvider, OperatingSystemProvider>();

var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);

app.Configure(config =>
{
    config.SetApplicationName("talos");
    config.SetExceptionHandler((ex, context) =>
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
        return -99;
    });
    
    // Commands
    config.AddCommand<HelloCommand>("hello")
        .WithDescription("Say hello and test the CLI")
        .WithExample(["hello", "--name", "Juan"]);

    config.AddCommand<InstallCommand>("install")
        .WithDescription("Install a template from the registry.")
        .WithExample(["install", "user/template-name"])
        .WithExample(["install", "react-starter", "--verbose"]);
});

return app.Run(args);