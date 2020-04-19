using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ChromaSchemeColorizer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            if (args.Length > 0)
            {
                int finishedFiles = 0;
                foreach (string dat in args)
                {
                    string file = Path.GetFileName(dat);
                    dynamic _beatmapdata;
                    try
                    {
                        _beatmapdata = JsonConvert.DeserializeObject(File.ReadAllText(dat));
                    }
                    catch
                    {
                        Console.WriteLine($"{file} is not a valid JSON, skipping.\n");
                        continue;
                    }

                    
                    List<ColorScheme> colorSchemes = BookmarkReader.ReadBookmarks(_beatmapdata);
                    if (colorSchemes.Count == 0)
                    {
                        Console.WriteLine($"{file} has no valid bookmarks and will be skipped.\n");
                        continue;
                    }

                    Colorizer.Colorize(_beatmapdata, colorSchemes);

                    string path = Path.GetDirectoryName(dat);
                    file = file.Insert(file.LastIndexOf("."), "_new");
                    File.WriteAllText($"{path}\\{file}", JsonConvert.SerializeObject(_beatmapdata));

                    finishedFiles++;
                    Console.WriteLine($"Created file: {file}\n");
                }

                if (finishedFiles > 0) Console.WriteLine($"Successfully colorized {finishedFiles} maps.");
                else Console.WriteLine($"No files written to.");
            }
            else Console.WriteLine("No arguments detected.");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }
    }
}