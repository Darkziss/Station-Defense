using UnityEngine;

namespace StationDefense
{
    public static class RandomUtility
    {
        public static bool GetRandomBool() => Random.Range(0, 2) != 0;
    }
}