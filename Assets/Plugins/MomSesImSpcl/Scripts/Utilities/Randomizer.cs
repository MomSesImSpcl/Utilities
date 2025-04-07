using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using MomSesImSpcl.Extensions;
using Random = System.Random;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Helper class to generate random values of primitive <see cref="Type"/>s.
    /// </summary>
    public static class Randomizer
    {
        #region Fields
        /// <summary>
        /// The <see cref="Random"/> <see cref="object"/> that generates the value for the entire <see cref="Randomizer"/>.
        /// </summary>
        private static readonly Random random = new();
        /// <summary>
        /// The <see cref="StringBuilder"/> that generates the <see cref="string"/> for <see cref="GetString"/>.
        /// </summary>
        private static readonly StringBuilder stringBuilder = new();
        /// <summary>
        /// Randomized seed for <see cref="GetFloat"/>.
        /// </summary>
        private static uint floatSeed = (uint)DateTime.UtcNow.Ticks;
        /// <summary>
        /// The reciprocal of a <see cref="uint"/>.
        /// </summary>
        private const float RECIPROCAL_UINT = 1f / (uint.MaxValue + 1f);
        /// <summary>
        /// Randomized seed for <see cref="GetDouble"/>.
        /// </summary>
        private static ulong doubleSeed = (ulong)DateTime.UtcNow.Ticks;
        /// <summary>
        /// The reciprocal of a <see cref="ulong"/>.
        /// </summary>
        private const double RECIPROCAL_ULONG = 1d / (ulong.MaxValue + 1d);
        #endregion

        #region Properties
        /// <summary>
        /// Returns a random <see cref="bool"/>.
        /// </summary>
        public static Func<bool> Bool => GetBool;
        /// <summary>
        /// Returns a random <see cref="byte"/> in the range of <see cref="byte.MinValue"/> to <see cref="byte.MaxValue"/>.
        /// </summary>
        public static Func<byte> Byte => () => GetByte();
        /// <summary>
        /// Returns a random <see cref="sbyte"/> in the range of <see cref="sbyte.MinValue"/> to <see cref="sbyte.MaxValue"/>.
        /// </summary>
        public static Func<sbyte> SByte => () => GetSByte();
        /// <summary>
        /// Returns a random <see cref="short"/> in the range of <see cref="short.MinValue"/> to <see cref="short.MaxValue"/>
        /// </summary>
        public static Func<short> Short => () => GetShort();
        /// <summary>
        /// Returns a random <see cref="ushort"/> in the range of <see cref="ushort.MinValue"/> to <see cref="ushort.MaxValue"/>
        /// </summary>
        public static Func<ushort> UShort => () => GetUShort();
        /// <summary>
        /// Returns a random <see cref="int"/> in the range of <see cref="int.MinValue"/> to <see cref="int.MaxValue"/>
        /// </summary>
        public static Func<int> Int => () => GetInt();
        /// <summary>
        /// Returns a random <see cref="uint"/> in the range of <see cref="uint.MinValue"/> to <see cref="uint.MaxValue"/>
        /// </summary>
        public static Func<uint> UInt => () => GetUInt();
        /// <summary>
        /// Returns a random <see cref="long"/> in the range of <see cref="long.MinValue"/> to <see cref="long.MaxValue"/>
        /// </summary>
        public static Func<long> Long => () => GetLong();
        /// <summary>
        /// Returns a random <see cref="ulong"/> in the range of <see cref="ulong.MinValue"/> to <see cref="ulong.MaxValue"/>
        /// </summary>
        public static Func<ulong> ULong => () => GetULong();
        /// <summary>
        /// Returns a random <see cref="float"/> in the range of <see cref="float.MinValue"/> to <see cref="float.MaxValue"/>
        /// </summary>
        public static Func<float> Float => () => GetFloat();
        /// <summary>
        /// Returns a random <see cref="double"/> in the range of <see cref="double.MinValue"/> to <see cref="double.MaxValue"/>
        /// </summary>
        public static Func<double> Double => () => GetDouble();
        /// <summary>
        /// Returns a random <see cref="decimal"/> in the range of <see cref="decimal.MinValue"/> to <see cref="decimal.MaxValue"/>
        /// </summary>
        public static Func<decimal> Decimal => () => GetDecimal();
        /// <summary>
        /// Returns a random unicode <see cref="char"/>.
        /// </summary>
        public static Func<char> Char => GetChar;
        /// <summary>
        /// Returns a random <c>10</c> <see cref="char"/> long <see cref="string"/>.
        /// </summary>
        public static Func<string> String => () => GetString(10);
        /// <summary>
        /// Contains <c>5</c> specialized prime numbers for each <see cref="PrimeNumberUseCase"/>.
        /// </summary>
        public static ReadOnlyDictionary<PrimeNumberUseCase, int[]> PrimeNumberSeeds = new(new Dictionary<PrimeNumberUseCase, int[]>
        {
            { PrimeNumberUseCase.BitMixing,            new[] { 73856093,   19349663,   83492791,  1299827,   600029     } },
            { PrimeNumberUseCase.DeterministicRandom,  new[] { 198491317,  6542989,    357239,    104729,    49979693   } },
            { PrimeNumberUseCase.Hashing,              new[] { 16777619,   1000003,    314159,    2653589,   1618033    } },
            { PrimeNumberUseCase.PerfectHashing,       new[] { 1000003,    1610612741, 32452843,  179424691, 982451653  } },
            { PrimeNumberUseCase.PRNG,                 new[] { 16807,      2147483647, 48271,     2796203,   12207031   } },
            { PrimeNumberUseCase.ProceduralGeneration, new[] { 1610612741, 12582917,   198491317, 999331,    324161900  } },
            { PrimeNumberUseCase.SpatialHashing,       new[] { 73856093,   19349663,   83492791,  600001,    1500450271 } },
            { PrimeNumberUseCase.UniqueIdGeneration,   new[] { 15485863,   32452843,   49979687,  982451653, 179424673  } }
        });
        #endregion
        
        #region Methods
        /// <summary>
        /// Returns a random <see cref="bool"/>.
        /// </summary>
        /// <returns><c>true</c> or <c>false</c>.</returns>
        public static bool GetBool() => random.Next(2).AsBool();

        /// <summary>
        /// Returns a random <see cref="byte"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="byte"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="byte"/>.</param>
        /// <returns>A random <see cref="byte"/> in the specified range.</returns>
        public static byte GetByte(byte _Min = byte.MinValue, byte _Max = byte.MaxValue) => (byte)random.Next(_Min, _Max + 1);

        /// <summary>
        /// Returns a random <see cref="sbyte"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="sbyte"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="sbyte"/>.</param>
        /// <returns>A random <see cref="sbyte"/> in the specified range.</returns>
        public static sbyte GetSByte(sbyte _Min = sbyte.MinValue, sbyte _Max = sbyte.MaxValue) => (sbyte)random.Next(_Min, _Max + 1);

        /// <summary>
        /// Returns a random <see cref="short"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="short"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="short"/>.</param>
        /// <returns>A random <see cref="short"/> in the specified range.</returns>
        public static short GetShort(short _Min = short.MinValue, short _Max = short.MaxValue) => (short)random.Next(_Min, _Max + 1);

        /// <summary>
        /// Returns a random <see cref="ushort"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="ushort"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="ushort"/>.</param>
        /// <returns>A random <see cref="ushort"/> in the specified range.</returns>
        public static ushort GetUShort(ushort _Min = ushort.MinValue, ushort _Max = ushort.MaxValue) => (ushort)random.Next(_Min, _Max + 1);

        /// <summary>
        /// Returns a random <see cref="int"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="int"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="int"/>.</param>
        /// <returns>A random <see cref="int"/> in the specified range.</returns>
        public static int GetInt(int _Min = int.MinValue, int _Max = int.MaxValue) => (int)GetLong(_Min, _Max); // TODO: Update to use the same approach as the "GetUInt"-method.
        
        /// <summary>
        /// Returns a random <see cref="uint"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="uint"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="uint"/>.</param>
        /// <returns>A random <see cref="uint"/> in the specified range.</returns>
        public static uint GetUInt(uint _Min = uint.MinValue, uint _Max = uint.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }
            
            var _exclusiveRange = _Max - _Min;

            if (_exclusiveRange == 1)
            {
                return _Min;
            }
            
            var _threshold = uint.MaxValue - uint.MaxValue % _exclusiveRange;
            uint _value;
            
            do
            {
                _value = (uint)random.Next(1 << 30) << 2 | (uint)random.Next(1 << 2);
                
            } while (_value >= _threshold);

            return _value % _exclusiveRange + _Min;
        }
        
        /// <summary>
        /// Returns a random <see cref="long"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="long"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="long"/>.</param>
        /// <returns>A random <see cref="long"/> in the specified range.</returns>
        public static long GetLong(long _Min = long.MinValue, long _Max = long.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }
            
            var _exclusiveRange = (ulong)(_Max - _Min);
            
            if (_exclusiveRange == 1)
            {
                return _Min;
            }
            
            var _threshold = ulong.MaxValue - ulong.MaxValue % _exclusiveRange;
            ulong _value;

            do
            {
                _value = ((ulong)random.Next(1 << 30) << 34) | ((ulong)random.Next(1 << 30) << 4) | (uint)random.Next(1 << 4);
                
            } while (_value >= _threshold);
            
            return (long)(_value % _exclusiveRange) + _Min;
        }

        /// <summary>
        /// Returns a random <see cref="ulong"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="ulong"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="ulong"/>.</param>
        /// <returns>A random <see cref="ulong"/> in the specified range.</returns>
        public static ulong GetULong(ulong _Min = ulong.MinValue, ulong _Max = ulong.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }
            
            var _exclusiveRange = _Max - _Min;
            
            if (_exclusiveRange == 1)
            {
                return _Min;
            }
            
            var _threshold = ulong.MaxValue - ulong.MaxValue % _exclusiveRange;
            ulong _value;

            do
            {
                _value = ((ulong)random.Next(1 << 30) << 34) | ((ulong)random.Next(1 << 30) << 4) | (uint)random.Next(1 << 4);
                
            } while (_value >= _threshold);

            return _value % _exclusiveRange + _Min;
        }
        
        /// <summary>
        /// Returns a random <see cref="float"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="float"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="float"/>.</param>
        /// <returns>A random <see cref="float"/> in the specified range.</returns>
        public static float GetFloat(float _Min = float.MinValue, float _Max = float.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }
            
            if (Math.Approximately(_Min, _Max))
            {
                return _Min;
            }
            
            floatSeed ^= floatSeed << 13;
            floatSeed ^= floatSeed >> 17;
            floatSeed ^= floatSeed << 5;
            
            return _Min + (_Max - _Min) * floatSeed * RECIPROCAL_UINT;
        }
        
        /// <summary>
        /// Returns a random <see cref="double"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="double"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="double"/>.</param>
        /// <returns>A random <see cref="double"/> in the specified range.</returns>
        public static double GetDouble(double _Min = double.MinValue, double _Max = double.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }

            if (Math.Approximately(_Min, _Max))
            {
                return _Min;
            }
            
            doubleSeed ^= doubleSeed >> 12;
            doubleSeed ^= doubleSeed << 25;
            doubleSeed ^= doubleSeed >> 27;
            
            var _randomBits = doubleSeed * 0x2545F4914F6CDD1DUL;
            
            if (double.IsInfinity(_Max - _Min))
            {
                const ulong _NAN_MASK = 0x7FF0000000000000UL;
                var _raw = (_randomBits & 0x7FFFFFFFFFFFFFFFUL) | (_randomBits >> 63) << 63;
                var _exponent = _raw & _NAN_MASK;
                
                if (_exponent == _NAN_MASK)
                {
                    _raw = (_raw & ~_NAN_MASK) | (0x7FEUL << 52);
                }
            
                return BitConverter.Int64BitsToDouble((long)_raw);
            }
            
            var _normalized = (_randomBits >> 11) * RECIPROCAL_ULONG;
            var _result = _Min + (_Max - _Min) * _normalized;

            return _result < _Max ? _result : _Max - double.Epsilon;
        }
        
        /// <summary>
        /// Returns a random <see cref="decimal"/> in the specified range.
        /// </summary>
        /// <param name="_Min">The minimum value of the <see cref="decimal"/>.</param>
        /// <param name="_Max">The maximum value of the <see cref="decimal"/>.</param>
        /// <returns>A random <see cref="decimal"/> in the specified range.</returns>
        public static decimal GetDecimal(decimal _Min = decimal.MinValue, decimal _Max = decimal.MaxValue)
        {
            if (_Min > _Max)
            {
                ThrowArgumentOutOfRangeException(_Min, _Max);
            }
            
            if (_Min == decimal.MinValue && _Max == decimal.MaxValue)
            {
                var _scale = (byte)random.Next(29);
                var _sign = random.Next(2) == 0;
                return new decimal(random.Next(), random.Next(), random.Next(), _sign, _scale);
            }
            
            var _sample = (decimal)random.NextDouble();
            return _Min + _sample * (_Max - _Min);
        }
        
        /// <summary>
        /// Returns a random <see cref="char"/>.
        /// </summary>
        /// <returns>A random unicode <see cref="char"/>.</returns>
        public static char GetChar() => (char)GetUShort(); // TODO: Add overload to get a char from a specific array.

        /// <summary>
        /// Returns a random <see cref="string"/> of the given <see cref="string.Length"/>.
        /// </summary>
        /// <param name="_Length">How many <see cref="char"/>s the <see cref="string"/> should contain.</param>
        /// <returns>A <see cref="string"/> containing random character.</returns>
        public static string GetString(int _Length) // TODO: Add overload to get a string from a specific char-array.
        {
            var _length = MathF.Abs(_Length);
            
            // ReSharper disable once InconsistentNaming
            for (var i = 0; i < _length; i++)
            {
                stringBuilder.Append(GetChar());
            }
            
            var _string = stringBuilder.ToString();
            stringBuilder.Clear();
            return _string;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <c>_Min</c> is greater than <c>_Max</c>.
        /// </summary>
        /// <typeparam name="T">Should be a numeric <see cref="Type"/>.</typeparam>
        /// <exception cref="ArgumentOutOfRangeException">When <c>_Min</c> is greater than <c>_Max</c></exception>
        private static void ThrowArgumentOutOfRangeException<T>(T _Min, T _Max) where T : unmanaged, IFormattable
        {
            throw new ArgumentOutOfRangeException($"{nameof(_Min).Bold()} cannot be greater than {nameof(_Max).Bold()}.\nMin: [{_Min.ToString().Bold()}] | Max: [{_Max.ToString().Bold()}]");
        }

        /// <summary>
        /// Generates a deterministic seed, based on the given <see cref="PrimeNumberUseCase"/> and up to five seed values.
        /// </summary>
        /// <param name="_PrimeNumberUseCase"><see cref="PrimeNumberUseCase"/>.</param>
        /// <param name="_Seed1">Typically a game state, player ID, or other random input.</param>
        /// <param name="_Seed2">Typically a game state, player ID, or other random input.</param>
        /// <param name="_Seed3">Typically a game state, player ID, or other random input.</param>
        /// <param name="_Seed4">Typically a game state, player ID, or other random input.</param>
        /// <param name="_Seed5">Typically a game state, player ID, or other random input.</param>
        /// <returns>A deterministic <see cref="int"/> seed that can be used in random number generation or other game systems requiring randomness.</returns>
        public static int GetSeed(PrimeNumberUseCase _PrimeNumberUseCase, int _Seed1 = 1, int _Seed2 = 1, int _Seed3 = 1, int _Seed4 = 1, int _Seed5 = 1)
        {
            var _primeNumbers = PrimeNumberSeeds[_PrimeNumberUseCase];
            return (_primeNumbers[0] * _Seed1) ^ (_primeNumbers[1] * _Seed2) ^ (_primeNumbers[2] * _Seed3) ^ (_primeNumbers[3] * _Seed4) ^ (_primeNumbers[4] * _Seed5);
        }
        #endregion
    }
}
