using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text;
using Spectre.Console;
using Utilities;
using static Utilities.Client;

public static class Launcher
{
    public static void Main(string[] args)
    {
        string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\simplyzetax";
        if (!Directory.Exists(appdata)) Directory.CreateDirectory(appdata);

        if (!File.Exists(appdata + "\\8bitsquirtle.png"))
            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri("https://cdn.nexusfn.net/file/2023/07/8bitsquirtle.png"),
                    appdata + "\\8bitsquirtle.png");
            }

        var image = new CanvasImage(appdata + @"8bitsquirtle.png");
        image.MaxWidth(8);
        AnsiConsole.Write(image);
        AnsiConsole.WriteLine();

        var option = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[deepskyblue1 bold]What do you want to do?[/]")
                .PageSize(4)
                .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                .AddChoices(new[]
                {
                    "Start client", "Start headless server", "Change credentials", "Change path"
                }));

        switch (option)
        {
            case "Start client":

                
                if (!File.Exists(appdata + @"\path.txt"))
                {
                    string path = ChangePath(appdata, args);
                    Start(path, false, appdata);
                }
                else
                {
                    var path = File.ReadAllText(appdata + "\\path.txt");
                    Start(path, false, appdata);
                }

                break;
            case "Start headless server":
                File.Exists(appdata + @"\path.txt");
                if (!File.Exists(appdata + @"\path.txt"))
                {
                    string path = ChangePath(appdata, args);
                    Start(path, true, appdata);
                }
                else
                {
                    var path = File.ReadAllText(appdata + "\\path.txt");
                    Start(path, true, appdata);
                }
                break;
            case "Change credentials":
                ChangeCredentials(appdata, args);
                Main(args);
                break;
            case "Change path":
                ChangePath(appdata, args);
                Main(args);
                break;
        }
    }

    private static string ChangePath(string appdata, string[] args)
    {
        if (!File.Exists(appdata + "\\path.txt")) File.Create(appdata + "\\path.txt");

        AnsiConsole.MarkupLine("[deepskyblue1]Please enter the path to your Fortnite installation.[/]");
        var path = AnsiConsole.Ask<string>("Path: ");
        if (!File.Exists(path + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe"))
        {
            AnsiConsole.MarkupLine("[deepskyblue1]The path you entered does not contain Fortnite[/]");
            ChangePath(appdata, args);
        }

        File.WriteAllText(appdata + "\\path.txt", path);
        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[deepskyblue1]Path changed![/]");
        return path;
    }

    private static string[] ChangeCredentials(string appdata, string[] args)
    {
        AnsiConsole.MarkupLine("[deepskyblue1]Please enter the path to your Fortnite installation.[/]");
        string email = AnsiConsole.Ask<string>("Email: ");
        string password = AnsiConsole.Ask<string>("Password: ");

        using (FileStream emailFile = File.OpenWrite(appdata + "\\email.txt"))
        {
            byte[] emailBytes = Encoding.UTF8.GetBytes(email);
            emailFile.Write(emailBytes, 0, emailBytes.Length);
        }

        using (FileStream passwordFile = File.OpenWrite(appdata + "\\password.txt"))
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordFile.Write(passwordBytes, 0, passwordBytes.Length);
        }

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine("[deepskyblue1]Credentials changed![/]");
        return new[] { email, password };
    }
}