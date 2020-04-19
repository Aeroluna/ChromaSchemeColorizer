using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChromaSchemeColorizer
{
    internal static class BookmarkReader
    {
        private static Regex rx = new Regex(@"^([^=\d]*(colorLeft|colorRight|envColorLeft|envColorRight|obstacleColor|bombColor)=\d(\.\d+)?,\d(\.\d+)?,\d(\.\d+)?(,\d(\.\d+)?)?)+$", RegexOptions.Compiled);

        internal static List<ColorScheme> ReadBookmarks(dynamic beatmapData)
        {
            List<ColorScheme> list = new List<ColorScheme>();
            foreach (dynamic _bookmark in beatmapData._customData._bookmarks)
            {
                string name = _bookmark._name;

                if (!rx.IsMatch(name))
                {
                    Console.WriteLine($"Invalid bookmark at time: \"{_bookmark._time}\" with name: \"{name}\", skipping.");
                    continue;
                }

                float[] left = name.ReadColor("colorLeft");
                CheckValues(left, _bookmark);

                float[] right = name.ReadColor("colorRight");
                CheckValues(right, _bookmark);

                float[] envleft = name.ReadColor("envColorLeft");
                CheckValues(envleft, _bookmark);

                float[] envright = name.ReadColor("envColorRight");
                CheckValues(envright, _bookmark);

                float[] obstacle = name.ReadColor("obstacleColor");
                CheckValues(obstacle, _bookmark);

                float[] bomb = name.ReadColor("bombColor");
                CheckValues(bomb, _bookmark);

                list.Add(new ColorScheme((float)_bookmark._time, left, right, envleft, envright, obstacle, bomb));
            }
            return list;
        }

        private static void CheckValues(this float[] array, dynamic _bookmark)
        {
            if (array == null) return;
            foreach (float n in array)
            {
                if (n > 1) Console.WriteLine($"Bookmark at time: \"{_bookmark._time}\" with name: \"{_bookmark._name}\" has a value of over 1, is this intentional?");
            }
        }

        private static float[] ReadColor(this string name, string flag)
        {
            int flagIndex = name.IndexOf($"{flag}=");
            if (flagIndex == -1) return null;
            flagIndex += (flag.Length + 1);
            int endOfColor = -1;

            int nextFlag = name.IndexOf("=", flagIndex);
            if (nextFlag != -1)
            {
                for (int i = nextFlag; i >= flagIndex; i--)
                {
                    if (char.IsDigit(name, i))
                    {
                        endOfColor = i + 1;
                        break;
                    }
                }
            }
            else endOfColor = name.Length;

            return name.Substring(flagIndex, endOfColor - flagIndex).Split(',').Select(float.Parse).ToArray();
        }
    }
}