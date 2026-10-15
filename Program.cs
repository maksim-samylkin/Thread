using System;
using System.Diagnostics;
using System.Threading;

namespace thread
{
    internal class ConsoleApp1
    {
        static void Main(string[] args)
        {
      
            Thread thread1 = new Thread(() =>
            {
                Console.WriteLine("Поток 1 Запуск потока.");
                Console.WriteLine("Поток 1 Запуск Notepad.");

                Process process = Process.Start("notepad.exe");
                if (process == null)
                {
                    Console.WriteLine("Поток 1 Не удалось запустить Блокнот.");
                    return;
                }

                Console.WriteLine("Поток 1 Ожидание завершения Notepad.");
                Thread.Sleep(3000);

                if (!process.HasExited)
                {
                    process.Kill();
                    Console.WriteLine("Поток 1 Notepad закрыт.");
                }

                process.Dispose();
                Console.WriteLine("Поток 1 Завершение.");
            });

            Thread thread2 = new Thread(() =>
            {
                Console.WriteLine("Поток 2 Запуск потока.");
                Console.WriteLine("Поток 2 Запуск cmd.exe с командой dir.");

                var psi = new ProcessStartInfo("cmd.exe", "/c dir")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = Process.Start(psi);
                if (process == null)
                {
                    Console.WriteLine("Поток 2 Не удалось запустить cmd.");
                    return;
                }

                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine("Поток 2 Результат команды dir:");
                Console.WriteLine(output);
                Console.WriteLine("Поток 2 Завершение.");
            });

          
            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

      
            Console.ReadLine();
        }
    }
}