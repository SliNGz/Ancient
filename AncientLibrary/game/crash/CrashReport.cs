using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.log
{
    public class CrashReport
    {
        private static string path = Environment.CurrentDirectory + "/crash/";
        private static string default_name = "crash_report";

        public static void CreateCrashReport(Exception exception)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            DateTime date = DateTime.Now;
            string dateString = date.Year + "_" + date.Month + "_" + date.Day + "_" + date.Hour + "-" + date.Minute + "-" + date.Second;
            StreamWriter writer = File.CreateText(path + default_name + "_" + dateString + ".txt");
            writer.Write(exception.ToString());
            writer.Close();
        }
    }
}
