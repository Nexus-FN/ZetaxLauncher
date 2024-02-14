using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Utilities;

public class Freeze
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

    [DllImport("kernel32.dll")]
    private static extern uint SuspendThread(IntPtr hThread);

    public static void Freezeproc(Process process)
    {

        foreach (object obj in process.Threads)
        {
            ProcessThread thread = (ProcessThread)obj;
            var Thread = OpenThread(2, false, (uint)thread.Id);
            if (Thread == IntPtr.Zero)
            {
                break;
            }
            SuspendThread(Thread);
        }
    }
}