using System.Collections.Generic;
using UnityEngine;

namespace StationDefense
{
    public static class TeamColorStorage
    {
        private static readonly Dictionary<ColorTeam, Color32> _colors = new()
        {
            [ColorTeam.Red] = Color.red,
            [ColorTeam.Yellow] = Color.yellow,
            [ColorTeam.Green] = Color.green,
            [ColorTeam.Blue] = Color.blue,
            [ColorTeam.None] = Color.black
        };

        public static Color32 GetByTeam(ColorTeam team) => _colors[team];
    }
}