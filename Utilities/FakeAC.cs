using System.Diagnostics;
using Spectre.Console;
using static Utilities.Freeze;

namespace Utilities;

public class FakeAC
{
    public static Process FNLauncherProcess;
    public static Process FNAntiCheatProcess;

    public static void Start(string FNPath, string FileName, string args = "", string t = "r")
    {
        try
        {
            FNLauncherProcess = new Process(); // Initialize FNLauncherProcess

            if (File.Exists(Path.Combine(FNPath, "FortniteGame\\Binaries\\Win64\\", FileName)))
            {
                ProcessStartInfo process = new ProcessStartInfo()
                {
                    FileName = Path.Combine(FNPath, "FortniteGame\\Binaries\\Win64\\", FileName),
                    Arguments = args,
                    CreateNoWindow = true,
                };

                if (t == "r")
                {
                    FNAntiCheatProcess = Process.Start(process);

                    if (FNAntiCheatProcess.Id == 0)
                    {
                        AnsiConsole.MarkupLine("[red]FAILED TO START[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[green]SUCCESSFULLY STARTED[/]");
                        Freezeproc(FNAntiCheatProcess);
                        AnsiConsole.MarkupLine("[green]SUCCESSFULLY FROZEN[/]");
                    }
                }
                else
                {
                    FNLauncherProcess = Process.Start(process);

                    if (FNLauncherProcess.Id == 0)
                    {
                        AnsiConsole.MarkupLine("[red]FAILED TO START[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[green]SUCCESSFULLY STARTED[/]");
                        Freezeproc(FNLauncherProcess);
                        AnsiConsole.MarkupLine("[green]SUCCESSFULLY FROZEN[/]");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine("[red]ERROR: " + ex.Message + "[/]");
            Console.ReadKey();
        }
    }


}