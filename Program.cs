using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        Console.WriteLine("Enter the program name to set RAM limit:");
        string programName = Console.ReadLine();

        Console.WriteLine("Enter the RAM limit in MB:");
        int ramLimit = Convert.ToInt32(Console.ReadLine());

        Process[] processes = Process.GetProcessesByName(programName);

        foreach (Process process in processes)
        {
            if (process.WorkingSet64 > ramLimit * 1024 * 1024)
            {
                process.Kill();
                Console.WriteLine($"{programName} exceeded the RAM limit of {ramLimit}MB and has been closed.");
            }
        }
    }
}
