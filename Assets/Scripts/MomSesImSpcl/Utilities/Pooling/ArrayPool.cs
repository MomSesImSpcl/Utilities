using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MomSesImSpcl.Extensions;

namespace MomSesImSpcl.Utilities.Pooling
{
    /// <summary>
    /// Generic <see cref="arrayPool"/> for primitive <see cref="Type"/>s.
    /// </summary>
    /// <typeparam name="T">Must be a primitive <see cref="Type"/>.</typeparam>
    public static partial class ArrayPool<T> where T : struct
    {
        #region Fields
        /// <summary>
        /// Each <see cref="ArrayBucket"/> holds <see cref="Array"/>s of a specific <see cref="Array.Length"/>. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        private static readonly Dictionary<int, ArrayBucket> arrayPool = new();
        /// <summary>
        /// Each <see cref="ArrayBucket"/> holds <see cref="Array"/>s of a specific <see cref="Array.Length"/>. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        private static readonly ConcurrentDictionary<int, ArrayBucket> concurrentArrayPool = new();
        #endregion
        
#if UNITY_EDITOR
        #region Properties
        /// <summary>
        /// If <c>true</c> the total number of created <see cref="Array"/>s will be printed on every <c>Get()</c>.
        /// </summary>
        // ReSharper disable once StaticMemberInGenericType
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static bool PrintNewArrayCount { get; set; }
        #endregion
#endif
        #region Methods
        /// <summary>
        /// Sets the maximum number of <see cref="Array"/>s that will be allowed in the <see cref="arrayPool"/> for the given <see cref="Array.Length"/>. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array"/> <see cref="Array.Length"/>.</param>
        /// <param name="_MaxAmount"><see cref="ArrayBucket.MaxAmount"/>.</param>
        public static void SetAmount(int _Length, uint _MaxAmount)
        {
            SetAmount(arrayPool, _Length, _MaxAmount);
        }

        /// <summary>
        /// Sets the maximum number of <see cref="Array"/>s that will be allowed in the <see cref="concurrentArrayPool"/> for the given <see cref="Array.Length"/>. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array"/> <see cref="Array.Length"/>.</param>
        /// <param name="_MaxAmount"><see cref="ArrayBucket.MaxAmount"/>.</param>
        public static void SetAmountConcurrent(int _Length, uint _MaxAmount)
        {
            SetAmount(concurrentArrayPool, _Length, _MaxAmount);
        }
        
        /// <summary>
        /// Sets the maximum number of <see cref="Array"/>s that will be allowed in the <c>_ArrayPool</c> for the given <see cref="Array.Length"/>.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        /// <param name="_Length">The desired <see cref="Array"/> <see cref="Array.Length"/>.</param>
        /// <param name="_MaxAmount"><see cref="ArrayBucket.MaxAmount"/>.</param>
        private static void SetAmount(IDictionary<int, ArrayBucket> _ArrayPool, int _Length, uint _MaxAmount)
        {
            if (!_ArrayPool.TryGetValue(_Length, out var _bucket))
            {
                _bucket = new ArrayBucket();
                _ArrayPool[_Length] = _bucket;
            }
            
            _bucket.MaxAmount = _MaxAmount;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_PrintNewArrayCount">
        /// If <c>true</c> the total number of created <see cref="Array"/>s of this <see cref="Type"/> and <see cref="Array.Length"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</i>
        /// </param>
        /// /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent the total number of created <see cref="Array"/>s from being printed, even if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] Get(int _Length, bool _PrintNewArrayCount = false, bool _ForceStopLogging = false)
        {
            return Get(arrayPool, _Length, _PrintNewArrayCount, _ForceStopLogging);
        }

        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="concurrentArrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_PrintNewArrayCount">
        /// If <c>true</c> the total number of created <see cref="Array"/>s of this <see cref="Type"/> and <see cref="Array.Length"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</i>
        /// </param>
        /// /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent the total number of created <see cref="Array"/>s from being printed, even if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] GetConcurrent(int _Length, bool _PrintNewArrayCount = false, bool _ForceStopLogging = false)
        {
            return Get(concurrentArrayPool, _Length, _PrintNewArrayCount, _ForceStopLogging);
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <c>_ArrayPool</c>, or creates a new one if none are available.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_PrintNewArrayCount">
        /// If <c>true</c> the total number of created <see cref="Array"/>s of this <see cref="Type"/> and <see cref="Array.Length"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent the total number of created <see cref="Array"/>s from being printed, even if <see cref="PrintNewArrayCount"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        private static T[] Get(IDictionary<int, ArrayBucket> _ArrayPool, int _Length, bool _PrintNewArrayCount, bool _ForceStopLogging)
        {
            if (!_ArrayPool.TryGetValue(_Length, out var _bucket))
            {
                _bucket = new ArrayBucket();
                _ArrayPool[_Length] = _bucket;
            }

            var _array = _bucket.Count > 0 ? _bucket.Dequeue() : new T[_Length];

            _bucket.RentedCount++;
            
#if UNITY_EDITOR
            if ((_PrintNewArrayCount || PrintNewArrayCount) && !_ForceStopLogging)
            {
                var _poolType = $"{nameof(ArrayPool<T>)}<{typeof(T).Name}>".Bold();
                UnityEngine.Debug.Log($"{_poolType}\nLength: [{_Length.Bold()}] | {nameof(_bucket.MaxAmount)}: [{_bucket.MaxAmount.Bold()}] | {nameof(_bucket.RentedCount)}: [{_bucket.RentedCount.Bold()}] | {nameof(_bucket.PeakRentedCount)}: [{_bucket.PeakRentedCount.Bold()}]");
            }
#endif
            return _array;
        }
        
        /// <summary>
        /// Returns the given <see cref="Array"/> to the <see cref="arrayPool"/> after clearing its contents. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to return.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayBucket.MaxAmount"/>.</param>
        public static void Return(T[] _Array, uint _NewMaxAmount = 1)
        {
            Return(arrayPool, _Array, _NewMaxAmount);
        }

        /// <summary>
        /// Returns the given <see cref="Array"/> to the <see cref="concurrentArrayPool"/> after clearing its contents. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Array">The <see cref="Array"/> to return.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayBucket.MaxAmount"/>.</param>
        public static void ReturnConcurrent(T[] _Array, uint _NewMaxAmount = 1)
        {
            Return(concurrentArrayPool, _Array, _NewMaxAmount);
        }
        
        /// <summary>
        /// Returns the given <see cref="Array"/> to the <c>_ArrayPool</c> after clearing its contents.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        /// <param name="_Array">The <see cref="Array"/> to return.</param>
        /// <param name="_NewMaxAmount">The new <see cref="ArrayBucket.MaxAmount"/>.</param>
        private static void Return(IDictionary<int, ArrayBucket> _ArrayPool, T[] _Array, uint _NewMaxAmount = 1)
        {
            if (!_ArrayPool.TryGetValue(_Array.Length, out var _bucket))
            {
                _bucket = new ArrayBucket();
                _ArrayPool[_Array.Length] = _bucket;
            }

            _bucket.MaxAmount = _NewMaxAmount;
            
            if (_bucket.Count >= _bucket.MaxAmount)
            {
                return;
            }

            Array.Clear(_Array, 0, _Array.Length);
            _bucket.Enqueue(_Array);
        }
        
        /// <summary>
        /// Clears the <see cref="arrayPool"/> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the given <see cref="Array.Length"/>. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The <see cref="Array.Length"/> of the <see cref="Array"/>s to clear.</param>
        public static void ClearLength(int _Length)
        {
            ClearLength(arrayPool, _Length);
        }

        /// <summary>
        /// Clears the <see cref="concurrentArrayPool"/> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the given <see cref="Array.Length"/>. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The <see cref="Array.Length"/> of the <see cref="Array"/>s to clear.</param>
        public static void ClearLengthConcurrent(int _Length)
        {
            ClearLength(concurrentArrayPool, _Length);
        }
        
        /// <summary>
        /// Clears the <c>_ArrayPool</c> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the given <see cref="Array.Length"/>.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        /// <param name="_Length">The <see cref="Array.Length"/> of the <see cref="Array"/>s to clear.</param>
        private static void ClearLength(IDictionary<int, ArrayBucket> _ArrayPool, int _Length)
        {
            if (_ArrayPool.TryGetValue(_Length, out var _bucket))
            {
                _bucket.Clear();
            }
        }
        
        /// <summary>
        /// Clears the <see cref="arrayPool"/> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the specified <see cref="Array.Length"/>s. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Lengths">The <see cref="Array.Length"/>s of the <see cref="Array"/>s to clear.</param>
        public static void ClearLengths(params int[] _Lengths)
        {
            ClearLengths(arrayPool, _Lengths);
        }

        /// <summary>
        /// Clears the <see cref="concurrentArrayPool"/> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the specified <see cref="Array.Length"/>s. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Lengths">The <see cref="Array.Length"/>s of the <see cref="Array"/>s to clear.</param>
        public static void ClearLengthsConcurrent(params int[] _Lengths)
        {
            ClearLengths(concurrentArrayPool, _Lengths);
        }
        
        /// <summary>
        /// Clears the <c>_ArrayPool</c> of <see cref="Array"/>s of <see cref="Type"/> <c>T</c> for the specified <see cref="Array.Length"/>s.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        /// <param name="_Lengths">The <see cref="Array.Length"/>s of the <see cref="Array"/>s to clear.</param>
        private static void ClearLengths(IDictionary<int, ArrayBucket> _ArrayPool, params int[] _Lengths)
        {
            foreach (var _length in _Lengths)
            {
                if (_ArrayPool.TryGetValue(_length, out var _bucket))
                {
                    _bucket.Clear();
                }
            }
        }

        /// <summary>
        /// Clears the entire <see cref="arrayPool"/>. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        public static void ClearAll()
        {
            ClearAll(arrayPool);
        }
        
        /// <summary>
        /// Clears the entire <see cref="concurrentArrayPool"/>. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        public static void ClearAllConcurrent()
        {
            ClearAll(concurrentArrayPool);
        }
        
        /// <summary>
        /// Clears the entire <c>_ArrayPool</c>.
        /// </summary>
        /// <param name="_ArrayPool">Must be <see cref="arrayPool"/> or <see cref="concurrentArrayPool"/>.</param>
        private static void ClearAll(IDictionary<int, ArrayBucket> _ArrayPool)
        {
            _ArrayPool.Clear();
        }
        #endregion

        /// <summary>
        /// Holds a <see cref="Queue{T}"/> of <see cref="Array"/>s of a specific <see cref="Array.Length"/>.
        /// </summary>
        private class ArrayBucket
        {
            #region Fields
            /// <summary>
            /// Contains every <see cref="Array"/> of a specific length.
            /// </summary>
            private readonly Queue<T[]> arrays = new();
            /// <summary>
            /// The number of <see cref="Array"/>s that are currently being rented from this <see cref="ArrayBucket"/>.
            /// </summary>
            private uint rentedCount;
            #endregion
            
            #region Properties
            /// <summary>
            /// The maximum number of <see cref="Array"/>s to retain in this bucket.
            /// </summary>
            public uint MaxAmount { get; set; } = 1;
            /// <summary>
            /// Gets the current number of <see cref="Array"/>s in this bucket.
            /// </summary>
            public int Count => this.arrays.Count;
            /// <summary>
            /// <see cref="rentedCount"/>.
            /// </summary>
            public uint RentedCount
            {
                get => this.rentedCount;
                set
                {
                    this.rentedCount = System.Math.Clamp(value, 0, uint.MaxValue);
                    this.PeakRentedCount = this.rentedCount > this.PeakRentedCount ? this.rentedCount : this.PeakRentedCount;
                }
            }
            /// <summary>
            /// Max amount of concurrently rented <see cref="Array"/>s from this <see cref="ArrayBucket"/>.
            /// </summary>
            public uint PeakRentedCount { get; private set; }
            #endregion
            
            #region Methods
            /// <summary>
            /// Enqueues an <see cref="Array"/> into the bucket.
            /// </summary>
            public void Enqueue(T[] _Array)
            {
                this.RentedCount -= 1;
                this.arrays.Enqueue(_Array);
            }

            /// <summary>
            /// Dequeues an <see cref="Array"/> from the bucket.
            /// </summary>
            public T[] Dequeue()
            {
                return this.arrays.Dequeue();
            }
            
            /// <summary>
            /// Clears every <see cref="Array"/> from the bucket.
            /// </summary>
            public void Clear() => this.arrays.Clear();
            #endregion
        }
    }
}