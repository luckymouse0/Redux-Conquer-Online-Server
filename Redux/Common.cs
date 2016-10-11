using System;
using System.Text;
using Redux.Utility;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TinyMap;

namespace Redux
{
    public static class Common
    {
        static Common()
        {
            UnixEpoch = new DateTime(1970, 1, 1);
            Random = new ThreadSafeRandom();
            _clock = Stopwatch.StartNew();
            ENCRYPTION_KEY = Encoding.ASCII.GetBytes("DR654dt34trg4UI6");
            SERVER_SEAL = Encoding.ASCII.GetBytes("TQServer");
            ValidChars = new Regex("^[a-zA-Z0-9]{4,16}$");
            DeltaX = new sbyte[] { 0, -1, -1, -1, 0, 1, 1, 1, 0 };
            DeltaY = new sbyte[] { 1, 1, 0, -1, -1, -1, 0, 1, 0 };
            _trojanLifeBonus = new[] { 100, 105, 108, 110, 112, 115 };
            _taoistManaBonus = new[] { 100, 100, 300, 400, 500, 600 };
            MapService = new TinyMapServer
            {
                ConquerDirectory = "",
                ExtractDMaps = false,
                LoadPortals = true,
                LoadHeight = true,
                Threading = true,
            };
            MapService.Load();
        }

        #region Variables
        public static ThreadSafeCounter ItemGenerator;
        public static int offset = 40;
        public static int value = 5;
        public static TinyMapServer MapService;
        public static List<ushort> ValidCharacterMeshes = new List<ushort> { 1003, 1004, 2001, 2002 };
        public static List<ushort> ValidBaseProfessions = new List<ushort> { 10, 20, 30, 40, 100};
        public static Dictionary<ushort, ushort> WeaponSkills = new Dictionary<ushort, ushort>
        { 
            {420,5030},
            {421,5030},
            {430,7000},
            {440,7040},
            {450,7010},
            {460,5040},
            {480,7020},
            {481,7030},
            {490,1290},
            {500,5000},
            {510,1250},
            {530,5050},
            {540,1300},
            {560,1260},
            {561,5010},
            {580,5020},
        };


        public static readonly sbyte[] DeltaX, DeltaY;
        private static readonly int[] _trojanLifeBonus;
        private static readonly int[] _taoistManaBonus;
        public static Regex ValidChars;
        public const int MINIMUM_THREAD_SLEEP_MS = 1;
        public const int MS_PER_SECOND = 1000;
        public const int MS_PER_MINUTE = 60000;
        public const int MS_PER_HOUR = 3600000;
        public static readonly DateTime UnixEpoch;
        public static ThreadSafeRandom Random;
        public static readonly byte[] ENCRYPTION_KEY;
        public static readonly byte[] SERVER_SEAL;
        private static readonly Stopwatch _clock;
        #endregion
        #region Helper Methods
        /// <summary>
        /// Returns MS since the server has loaded. Use for all timing needs.
        /// </summary>
        public static long Clock
        {
            get
            {
                lock (_clock)
                    return _clock.ElapsedMilliseconds;
            }
        }
        public static uint SecondsServerOnline
        {
            get { return (uint)(_clock.ElapsedMilliseconds / MS_PER_SECOND); }
        }
        public static uint MinutesServerOnline
        {
            get { return (uint)(_clock.ElapsedMilliseconds / MS_PER_MINUTE); }
        }
        public static uint HoursServerOnline
        {
            get { return (uint)(_clock.ElapsedMilliseconds / MS_PER_HOUR); }
        }

        public static long SecondsFromNow(DateTime end)
        {
            long remaining = 0;
            if (end < DateTime.Now)
                return remaining;
            var diff = end.Subtract(DateTime.Now);
            remaining += diff.Seconds;
            remaining += diff.Minutes * 60;
            remaining += diff.Hours * 3600;
            remaining += diff.Days * 86400;
            return remaining;
        }
        public static long MsFromNow(DateTime end)
        {
            long remaining = 0;
            if (end < DateTime.Now)
                return remaining;
            var diff = end.Subtract(DateTime.Now);
            remaining += diff.Seconds * 1000;
            remaining += diff.Minutes * 60000;
            remaining += diff.Hours * 3600000;
            return remaining;
        }

        public static int MulDiv(int number, int numerator, int denominator)
        {
            if (denominator < 1)
                denominator = 1;
            return number * numerator / denominator;
        }

        public static uint MulDiv(uint number, uint numerator, uint denominator)
        {
            if (denominator < 1)
                denominator = 1;
            return number * numerator / denominator;
        }

        public static long MulDiv(long number, long numerator, long denominator)
        {
            if (denominator < 1)
                denominator = 1;
            return number * numerator / denominator;
        }
        public static double GetRadian(int sourceX, int sourceY, int targetX, int targetY)
        {
            if (!(sourceX != targetX || sourceY != targetY)) return 0f;

            var deltaX = targetX - sourceX;
            var deltaY = targetY - sourceY;
            var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

            if (!(deltaX <= distance && distance > 0)) return 0f;
            var radian = Math.Asin(deltaX / distance);

            return deltaY > 0 ? (Math.PI / 2 - radian) : (Math.PI + radian + Math.PI / 2);
        }
        public static int GetTrojanLifeBonus(int index)
        {
            return index < 0 || index > _trojanLifeBonus.Length ? _trojanLifeBonus[0] : _trojanLifeBonus[index];
        }

        public static int GetTaoistManaBonus(int index)
        {
            return index < 0 || index > _taoistManaBonus.Length ? _taoistManaBonus[0] : _taoistManaBonus[index];
        }
        public static byte ExchangeBits(byte data, int bits)
        {
            return (byte)((data << bits) | (data >> bits));
        }

        public static uint ExchangeShortBits(uint data, int bits)
        {
            data &= 0xffff;
            return ((data >> bits) | (data << (16 - bits))) & 0xffff;
        }

        public static uint ExchangeLongBits(uint data, int bits)
        {
            return (data >> bits) | (data << (32 - bits));
        }
        public static bool PercentSuccess(double _chance)
        {
            return Random.NextDouble() * 100 < _chance;
        }
        public static bool PercentSuccess(int _chance)
        {
            return Random.NextDouble() * 100 < _chance;
        }
        #endregion

    }
}
