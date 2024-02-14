using System.Diagnostics;
using Spectre.Console;

namespace Utilities;

public class Client
{
    public static void Start(string path, bool server, string appdata)
    {

        string Arguments;
        if(server)
        {
            Arguments = $"-log -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fromfl=eac -fltoken=3db3ba5dcbd2e16703f3978d -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiYmU5ZGE1YzJmYmVhNDQwN2IyZjQwZWJhYWQ4NTlhZDQiLCJnZW5lcmF0ZWQiOjE2Mzg3MTcyNzgsImNhbGRlcmFHdWlkIjoiMzgxMGI4NjMtMmE2NS00NDU3LTliNTgtNGRhYjNiNDgyYTg2IiwiYWNQcm92aWRlciI6IkVhc3lBbnRpQ2hlYXQiLCJub3RlcyI6IiIsImZhbGxiYWNrIjpmYWxzZX0.VAWQB67RTxhiWOxx7DBjnzDnXyyEnX7OljJm-j2d88G_WgwQ9wrE6lwMEHZHjBd1ISJdUO1UVUqkfLdU5nofBQ -AUTH_LOGIN=your@email.com -AUTH_PASSWORD=yourpasswordhere -AUTH_TYPE=epic -nullrhi -nosplash -nosound";
        } else
        {
            Arguments = $"-log -epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -skippatchcheck -nobe -fromfl=eac -fltoken=3db3ba5dcbd2e16703f3978d -caldera=eyJhbGciOiJFUzI1NiIsInR5cCI6IkpXVCJ9.eyJhY2NvdW50X2lkIjoiYmU5ZGE1YzJmYmVhNDQwN2IyZjQwZWJhYWQ4NTlhZDQiLCJnZW5lcmF0ZWQiOjE2Mzg3MTcyNzgsImNhbGRlcmFHdWlkIjoiMzgxMGI4NjMtMmE2NS00NDU3LTliNTgtNGRhYjNiNDgyYTg2IiwiYWNQcm92aWRlciI6IkVhc3lBbnRpQ2hlYXQiLCJub3RlcyI6IiIsImZhbGxiYWNrIjpmYWxzZX0.VAWQB67RTxhiWOxx7DBjnzDnXyyEnX7OljJm-j2d88G_WgwQ9wrE6lwMEHZHjBd1ISJdUO1UVUqkfLdU5nofBQ -AUTH_LOGIN=your@email.com -AUTH_PASSWORD=yourpasswordhere AUTH_TYPE=epic -nosplash";
        }

        Process shipping = new Process();
        shipping.StartInfo.FileName = path + "\\FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe";
        shipping.StartInfo.Arguments = Arguments;
        shipping.StartInfo.UseShellExecute = false;
        shipping.StartInfo.RedirectStandardOutput = true;
        shipping.StartInfo.RedirectStandardError = true;

        if(!server)
        {
            foreach (var process in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
            {
                process.Kill();
            }
        }

        shipping.ErrorDataReceived += (sender, args) => Console.WriteLine(args.Data);
        
        shipping.Start();

        FakeAC.Start(path, "FortniteClient-Win64-Shipping_BE.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "r");
        FakeAC.Start(path, "FortniteLauncher.exe", $"-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");

        AnsiConsole.MarkupLine("[deepskyblue1]Injected Car.[/]");
        Injector.Inject(shipping.Id, appdata + "\\Car.dll");
        
        AnsiConsole.MarkupLine("[deepskyblue1]Waiting for exit to cleanup.[/]");

        //Read output 

        bool injectedS = false;
        bool injectedC = false;

        while (!shipping.StandardOutput.EndOfStream)
        {
            string line = shipping.StandardOutput.ReadLine();

            if (line.ToLower().Contains("region") && injectedS == false && injectedC == false)
            {
                AnsiConsole.MarkupLine($"{line}");
                if(server == true)
                {
                    Injector.Inject(shipping.Id, appdata + "\\reboot.dll");
                    AnsiConsole.MarkupLine("[deepskyblue1]Injected reboot[/]");
                    injectedS = true;
                } else
                {
                    Injector.Inject(shipping.Id, appdata + "\\console.dll");
                    AnsiConsole.MarkupLine("[deepskyblue1]Injected console[/]");
                    injectedC = true;
                }
            } else if(injectedS && injectedC)
            {
                Console.WriteLine(line);
            }
        }

        shipping.WaitForExit();

        foreach(var process in Process.GetProcesses())
        {
            if(process.ProcessName.ToLower().Contains("battleye"))
            {
                process.Kill();
            }
            if (process.ProcessName.ToLower().Contains("fortnite"))
            {
                process.Kill();
            }
            if (process.ProcessName.ToLower().Contains("easyanticheat"))
            {
                process.Kill();
            }
            if (process.ProcessName.ToLower().Contains("eac"))
            {
                process.Kill();
            }
        }

    }
}