using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChromaSchemeColorizer
{
    internal static class Colorizer
    {
        internal static void Colorize(dynamic data, List<ColorScheme> schemes)
        {
            ApplyScheme(data._events, schemes, new Action<dynamic, ColorScheme>(EventColor));
            ApplyScheme(data._notes, schemes, new Action<dynamic, ColorScheme>(NoteColor));
            ApplyScheme(data._obstacles, schemes, new Action<dynamic, ColorScheme>(ObstacleColor));
        }

        private static void ApplyScheme(dynamic array, List<ColorScheme> schemes, Action<dynamic, ColorScheme> action)
        {
            foreach (dynamic @object in array)
            {
                ColorScheme scheme = schemes.GetColorScheme((float)@object._time);
                action(@object, scheme);
            }
        }

        private static void EventColor(dynamic _event, ColorScheme scheme)
        {
            if (_event._value == 0) return;
            ApplyColor(_event, IsRedEvent((int)_event._value) ? scheme.envColorLeft : scheme.envColorRight);
        }

        private static void NoteColor(dynamic _note, ColorScheme scheme)
        {
            if (_note._type == 3) ApplyColor(_note, scheme.bombColor);
            else ApplyColor(_note, IsRedNote((int)_note._type) ? scheme.colorLeft : scheme.colorRight);
        }

        private static void ObstacleColor(dynamic _obstacle, ColorScheme scheme)
        {
            ApplyColor(_obstacle, scheme.obstacleColor);
        }

        private static void ApplyColor(dynamic _event, float[] color)
        {
            if (color == null) return;
            if (!_event.ContainsKey("_customData")) _event._customData = new JObject();
            _event._customData._color = new JArray(color);
        }

        private static ColorScheme GetColorScheme(this List<ColorScheme> schemes, float time)
        {
            return schemes.Where(n => n.time <= time).LastOrDefault();
        }

        private static bool IsRedNote(int type)
        {
            switch (type)
            {
                case 1:
                    return false;

                case 0:
                default:
                    return true;
            }
        }

        private static bool IsRedEvent(int val)
        {
            switch (val)
            {
                case 1:
                case 2:
                case 3:
                    return false;

                case 5:
                case 6:
                case 7:
                default:
                    return true;
            }
        }
    }
}