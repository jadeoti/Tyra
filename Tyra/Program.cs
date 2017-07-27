using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Tyra
{
    internal class Program
    {
        private const long _batchSize = 10000;
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





            try
            {
                using (var sr = new StreamReader(pathToFile))
                {
                    List<string> subscribers = new List<String>();
                    String line;

                    int lineCount = 0;
                    int batch = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCount++;
                        string pattern = @"(\d+)(,)(.+)";
                        foreach (Match m in Regex.Matches(line, pattern))
                        {
                            Person person = new Person();
                            person.Msisdn = decimal.Parse(m.Groups[1].Value);
                            person.Name = m.Groups[3].Value;
                            String subscriber = string.Format(@"{0},{1}", person.Msisdn, person.Name);
                            subscribers.Add(subscriber);
                        }

                        if (lineCount >= _batchSize)
                        {
                            String directoryName = Path.GetDirectoryName(pathToFile);
                            String destinationPath = Path.Combine(directoryName, "fixed_batch_" + ++batch + ".csv");
                            File.WriteAllLines(destinationPath, subscribers);

                            // reset line count
                            lineCount = 0;
                            subscribers.Clear();

                        }
                    }
                    String dirName = Path.GetDirectoryName(pathToFile);
                    String destPath = Path.Combine(dirName, "fixed_batch_" + ++batch + ".csv");
                    File.WriteAllLines(destPath, subscribers);
                    // commit the remainng tofile 



                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

          
            // Suspend the screen.
            Console.ReadLine();
        }
    }
}