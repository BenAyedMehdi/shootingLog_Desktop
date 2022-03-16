using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShootingLog.Model
{
    class FileController
    {
        internal static void WriteToFile(List<DistanceResult> newList)
        {
            string fileName = @"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\distance.txt";
            if (File.Exists(fileName))
            {
                StreamWriter writer = new StreamWriter(fileName,true);
                foreach (var item in newList)
                {
                    writer.WriteLine(item.ToLine());
                }
                writer.Close();
            }
        }

        internal static List<DistanceResult> ReadFile()
        {
            List<DistanceResult> list = new List<DistanceResult>();
            String line;
            string fileName = @"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\distance.txt";
            StreamReader sr = new StreamReader(fileName);
            line = sr.ReadLine();
            while (line != null)
            {
                list.Add(LineToDistance(line));

                line = sr.ReadLine();
            }
            sr.Close();
            return list;
        }

        private static DistanceResult LineToDistance(string line)
        {
            string[] parts = line.Split(';');
            return new DistanceResult(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[3]));
        }

        internal static void WriteLog(string msg)
        {
            string fileName = @"E:\Work\INTERNSHIP\2\new\shootinglog\ShootingLog\bin\Debug\log.txt";
            StreamWriter writer = new StreamWriter(fileName, true);
            writer.Write(msg);
            writer.Close();
        }
    }
}
