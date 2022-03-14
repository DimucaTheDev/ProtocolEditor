/*
 * © DimucaTheDev, 2022 
 * 
 * Edit code, and publish uhhhhhhhhhhh yeah
 * Idk what i have to paste here, idk, i dont know to use licenses
 * https://vk.com/dimdima09 - u can msg me there, i hope vk.com isnt blocked in ur country (cuz of 24.02.2022)
 * also if you found this useful (nevermind), you can star this repo on github!!1
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DimucaTheDev.ProtocolEditor
{
    internal class Program
    {
        static bool dev = false;
        public static string dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\\ProtocolEditor";
        public static string file = $"{dir}\\regs.json";
        
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
                if (args[0] == "--enable-debugging")
                {
                    dev = true;
                    Debugger.Launch();
                }

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            else
                if(!File.Exists(file))
                    File.Create(file);
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
            
        }
        #region "Useless" Sh.. oh i mean logs ummm yeah
        [Obsolete("Damn its not logger its just WriteLine in console huh, use another Log method to LOG (!!1) in selected debugger")]
        public static void ConsoleLog(object msg) { if (dev) { Console.WriteLine($"[{DateTime.Now.ToString("g")}] : {msg}"); } }
        public static void Warn(object msg) { if (dev) { Console.BackgroundColor = ConsoleColor.Yellow; Console.WriteLine($"[{DateTime.Now.ToString("g")}] : {msg}"); Console.ResetColor(); } }
        public static void Shit(object msg) { if (dev) { Console.BackgroundColor = ConsoleColor.Red; Console.WriteLine($"[{DateTime.Now.ToString("g")}] : {msg}"); Console.ResetColor(); } }
        public static void Success(object msg) { if (dev) { Console.BackgroundColor = ConsoleColor.Green; Console.WriteLine($"[{DateTime.Now.ToString("g")}] : {msg}"); Console.ResetColor(); } }
        public static void Fail/*oh no*/(object msg) => Shit(msg);
        public static void Log/*in debug*/(object msg) => Debug.WriteLine(msg);
        #endregion
    }
}
