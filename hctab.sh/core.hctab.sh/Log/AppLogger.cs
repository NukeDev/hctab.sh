using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace core.hctab.sh.Log
{
    public class AppLogger
    {
        private string _File { get; set; }

        public AppLogger()
        {

            try
            {
                if (!Directory.Exists("logs"))
                {
                    Directory.CreateDirectory("logs");
                }

                this._File = GenerateLogFile();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string GenerateLogFile()
        {
            return "logs/" + DateTime.Now.ToFileTime() + ".log";
        }

        public void WriteInformation(string data)
        {
            try
            {
                var row = GenerateRow(data, LogType.INFO);
                File.AppendAllLines(_File, new string[] { row });
                WriteToConsoleWithColor(row, ConsoleColor.DarkGreen);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void WriteWarning(string data)
        {
            try
            {
                var row = GenerateRow(data, LogType.WARNING);
                File.AppendAllLines(_File, new string[] { row });

                WriteToConsoleWithColor(row, ConsoleColor.Yellow);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void WriteError(string data)
        {
            try
            {
                var row = GenerateRow(data, LogType.ERROR);
                File.AppendAllLines(_File, new string[] { row });
                WriteToConsoleWithColor(row, ConsoleColor.Red);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private string GenerateRow(string text, LogType log)
        {
            return DateTimeOffset.Now.ToString() + " " + (log.ToString()) + " --> " + text;
        }

        enum LogType
        {
            INFO = 1,
            ERROR = 2, 
            WARNING = 3
        }

        private void WriteToConsoleWithColor(string data, ConsoleColor color)
        {
            var _color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(data);
            Console.ForegroundColor = _color;
        }

    }
}
