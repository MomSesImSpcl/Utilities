using System;
using System.Collections.Generic;

namespace MomSesImSpcl.Utilities.Pooling
{
    /// <summary>
    /// Contains methods to initializes the <see cref="Array"/> elements in one method call.
    /// </summary>
    public static partial class ArrayPool<T> where T : unmanaged
    {
        #region Methods
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] Get(int _Length, T _Index0Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(arrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_Index1Value">The value to assign to the element at index <c>1</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] Get(int _Length, T _Index0Value, T _Index1Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(arrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            _array[1] = _Index1Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_Index1Value">The value to assign to the element at index <c>1</c>.</param>
        /// <param name="_Index2Value">The value to assign to the element at index <c>2</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] Get(int _Length, T _Index0Value, T _Index1Value, T _Index2Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(arrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            _array[1] = _Index1Value;
            _array[2] = _Index2Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Not thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Values">
        /// The values to assign to the <see cref="Array"/>. <br/>
        /// <b>The length of the <see cref="IEnumerable{T}"/> must be the same as <c>_Length</c>.</b>
        /// </param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] Get(int _Length, IEnumerable<T> _Values, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _index = 0;
            var _array = Get(arrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            using var _enumerator = _Values.GetEnumerator();
            
            while (_enumerator.MoveNext())
            {
                _array[_index++] = _enumerator.Current;
            }
            
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param> <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] GetConcurrent(int _Length, T _Index0Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(concurrentArrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_Index1Value">The value to assign to the element at index <c>1</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] GetConcurrent(int _Length, T _Index0Value, T _Index1Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(concurrentArrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            _array[1] = _Index1Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Index0Value">The value to assign to the element at index <c>0</c>.</param>
        /// <param name="_Index1Value">The value to assign to the element at index <c>1</c>.</param>
        /// <param name="_Index2Value">The value to assign to the element at index <c>2</c>.</param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] GetConcurrent(int _Length, T _Index0Value, T _Index1Value, T _Index2Value, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _array = Get(concurrentArrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            _array[0] = _Index0Value;
            _array[1] = _Index1Value;
            _array[2] = _Index2Value;
            return _array;
        }
        
        /// <summary>
        /// Retrieves an <see cref="Array"/> of the given <see cref="Array.Length"/> from the <see cref="arrayPool"/>, or creates a new one if none are available. <br/>
        /// <b>Thread safe.</b>
        /// </summary>
        /// <param name="_Length">The desired <see cref="Array.Length"/>.</param>
        /// <param name="_Values">
        /// The values to assign to the <see cref="Array"/>. <br/>
        /// <b>The length of the <see cref="IEnumerable{T}"/> must be the same as <c>_Length</c>.</b>
        /// </param>
        /// <param name="_LogArrayBucket">
        /// If <c>true</c>, info about the <see cref="ArrayBucket"/> will be printed to the console. <br/>
        /// <i>Parameter will be ignored if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</i>
        /// </param>
        /// <param name="_ForceStopLogging">Set this to <c>true</c> to prevent logging, even if <see cref="LogArrayBuckets"/> is set to <c>true</c>.</param>
        /// <returns>An <see cref="Array"/> of type <c>T</c> with the specified <see cref="Array.Length"/>.</returns>
        public static T[] GetConcurrent(int _Length, IEnumerable<T> _Values, bool _LogArrayBucket = false, bool _ForceStopLogging = false)
        {
            var _index = 0;
            var _array = Get(concurrentArrayPool, _Length, _LogArrayBucket, _ForceStopLogging);
            using var _enumerator = _Values.GetEnumerator();
            
            while (_enumerator.MoveNext())
            {
                _array[_index++] = _enumerator.Current;
            }
            
            return _array;
        }
        #endregion
    }
}
