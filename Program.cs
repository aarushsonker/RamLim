using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

class Program
{
    static void Main()
    {
        while (true)
        {
            Console.Clear();
            PrintHeader();
            Console.WriteLine("\nEnter the program name to reduce RAM usage:");

            string processName;
            while (true)
            {
                processName = Console.ReadLine()?.Trim();
                if (Process.GetProcessesByName(processName).Length > 0)
                    break;

                Console.WriteLine($"Error: No running process found with the name '{processName}'. Try again.");
                Console.Write("Enter a valid program name: ");
            }

            while (true)
            {
                Console.Clear();
                PrintHeader();
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. Refresh memory at intervals");
                Console.WriteLine("2. Exit");
                Console.WriteLine("3. About this Program");

                string choice = Console.ReadLine()?.Trim();

                if (choice == "1")
                {
                    Console.Write("\nEnter refresh interval (in minutes): ");
                    if (int.TryParse(Console.ReadLine(), out int interval) && interval > 0)
                    {
                        Console.Clear();
                        PrintHeader();
                        Console.WriteLine($"\n{processName}: RAM usage will be reduced every {interval} minute(s).");
                        Console.WriteLine("Press 'X' to exit at any time.\n");

                        while (true)
                        {
                            if (Process.GetProcessesByName(processName).Length == 0)
                            {
                                Console.WriteLine($"Warning: '{processName}' is no longer running. Exiting...");
                                break;
                            }

                            FreeMemory(processName);
                            Console.WriteLine($"{DateTime.Now}: Memory refreshed for {processName}.");

                            for (int i = 0; i < interval * 60; i++)
                            {
                                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.X)
                                {
                                    Console.WriteLine("\nExiting...");
                                    return;
                                }
                                Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid interval. Please enter a valid number.");
                        Thread.Sleep(1500);
                    }
                }
                else if (choice == "2")
                {
                    Console.WriteLine("Exiting...");
                    Environment.Exit(0);
                }
                else if (choice == "3")
                {
                    ShowAboutInfo();
                }
                else
                {
                    Console.WriteLine("Invalid choice, try again.");
                    Thread.Sleep(1500);
                }
            }
        }
    }

    static void FreeMemory(string processName)
    {
        try
        {
            foreach (Process process in Process.GetProcessesByName(processName))
            {
                SetProcessWorkingSetSize(process.Handle, -1, -1);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static void ShowAboutInfo()
    {
        Console.Clear();
        PrintHeader();
        Console.WriteLine("\n=== About this Program ===");
        Console.WriteLine("Kotetsu Labs - Efficient Software Solutions.");
        Console.WriteLine("GitHub Repository: https://github.com/aarushsonker/RamLim");
        Console.WriteLine("\nTechnical Information:");
        Console.WriteLine($"- Built with: .NET Framework {Environment.Version}");
        Console.WriteLine($"- OS: {RuntimeInformation.OSDescription}");
        Console.WriteLine($"- CPU Architecture: {RuntimeInformation.OSArchitecture}");
        Console.WriteLine("\nPress any key to return...");
        Console.ReadKey();
    }

    static void PrintHeader()
    {
        Console.WriteLine("Developed by Kotetsu Labs".PadLeft(Console.WindowWidth - 1));
    }

    [DllImport("kernel32.dll")]
    static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
}
