using System;
using System.Diagnostics;

namespace CBuildSystem.Helpers
{
    static class ExternalPrograms
    {
        internal static void RunCompiler(string args)
        {
                ProcessStartInfo info = new ProcessStartInfo("gcc");
                info.Arguments = args;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;

                Console.WriteLine(args);

                Process proc = Process.Start(info);

                proc.WaitForExit();

                Console.Write(proc.StandardError.ReadToEnd());
        }

        internal static string RunPkgConfig(string pkgName, bool include)
        {
            ProcessStartInfo info = new ProcessStartInfo("pkg-config");

            if(include)
            {
                info.Arguments = $" --cflags {pkgName}";
            }
            else
            {
                info.Arguments = $" --libs {pkgName}";
            }
            
            info.RedirectStandardOutput = true;
            info.RedirectStandardError = true;

            Process proc = Process.Start(info);

            proc.WaitForExit();

            string toRet = proc.StandardOutput.ReadToEnd();

            if(string.IsNullOrEmpty(toRet) || toRet.Contains("not found"))
                throw new Exception($"Could not locate {pkgName} in current system");

            return toRet;
        }
    }
}