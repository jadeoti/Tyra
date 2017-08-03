using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Tyra
{
    internal class Program
    {
        private const long _batchSize = 300000;

        private static void Main(string[] args)
        {
            Console.WriteLine("..............................................");
            Console.WriteLine("I am Tyra, I will work hard to clean this file.");
            Console.WriteLine("..............................................");
            Console.WriteLine("May I have the file you want to clean?");
            Console.Write("Path to file:");


            String pathToFile = Console.ReadLine();
            while (string.IsNullOrEmpty(pathToFile))
            {
                Console.Write("Path to file:");
                pathToFile = Console.ReadLine();
            }

            // delete any previous file
            String destination = Path.GetDirectoryName(pathToFile);
            File.Delete(Path.Combine(destination, "fixed.csv"));

            try
            {
                using (var sr = new StreamReader(pathToFile))
                {
                    List<string> subscribers = new List<String>();
                    String line;

                    int lineCount = 0;
                    //int batch = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;
                        string pattern = @"(\d+)(,)(.+)";
                        foreach (Match m in Regex.Matches(line, pattern))
                        {
                            decimal msidsdn = decimal.Parse(m.Groups[1].Value);

                            string name = m.Groups[3].Value
                                .Replace(",", " ")
                                .Replace("  ", " ")
                                .Replace("\"", "");
                            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name);

                            string subscriber = string.Format(@"{0},{1}", msidsdn, name);
                            subscribers.Add(subscriber);
                        }

                        if (lineCount >= _batchSize)
                        {
                            String directoryName = Path.GetDirectoryName(pathToFile);
                            String destinationPath = Path.Combine(directoryName, "fixed.csv");
                            File.AppendAllLines(destinationPath, subscribers);
                            Console.Write("!");


                            // reset line count
                            lineCount = 0;
                            subscribers.Clear();
                        }
                    }
                    String dirName = Path.GetDirectoryName(pathToFile);
                    String destPath = Path.Combine(dirName, "fixed.csv");
                    File.AppendAllLines(destPath, subscribers);
                    Console.WriteLine();
                    // commit the remainng tofile 
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }


            // Suspend the screen.
            Console.WriteLine("Done");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}