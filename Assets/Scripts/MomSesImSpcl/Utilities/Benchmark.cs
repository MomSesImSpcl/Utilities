using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MomSesImSpcl.Extensions;
using Debug = UnityEngine.Debug;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// Call one of the properties with/inside a <c>using</c>-statement/block and the elapsed time and used memory will be printed to the console on <see cref="Dispose"/>. <br/>
    /// Or call a <c>Run</c>-method to measure the median time and memory.
    /// </summary>
    public readonly struct Benchmark : IDisposable
    {
        #region Constants
        /// <summary>
        /// The default warmup iteration count for the <c>Run</c>-methods.
        /// </summary>
        private const uint DEFAULT_WARMUP_ITERATION = 100_000;
        /// <summary>
        /// The default iteration count for the <c>Run</c>-methods.
        /// </summary>
        private const uint DEFAULT_ITERATIONS = 1_000_000;
        #endregion
        
        #region Fields
        /// <summary>
        /// The <see cref="TimeResolution"/> to measure in.
        /// </summary>
        private readonly TimeResolution timeResolution;
        /// <summary>
        /// The starting timestamp.
        /// </summary>
        private readonly long startTimeStamp;
        /// <summary>
        /// The memory at the start of the measuring process.
        /// </summary>
        private readonly long? startMemory;
        /// <summary>
        /// Will be <c>true</c> if this <see cref="Benchmark"/> <see cref="object"/> is measuring the time for a benchmark. <br/>
        /// <i>Can be <c>false</c> even if <see cref="benchmarkIsRunning"/> is <c>true</c>.</i>
        /// </summary>
        private readonly bool isBenchmark;
        /// <summary>
        /// Lock <see cref="object"/> for the benchmark.
        /// </summary>
        private static readonly object benchmarkLock = new();
        /// <summary>
        /// Will be <c>true</c> if a benchmark is currently running.
        /// </summary>
        private static bool benchmarkIsRunning;
        /// <summary>
        /// <see cref="CancellationTokenSource"/> to cancel the currently running benchmark.
        /// </summary>
        private static CancellationTokenSource cancelBenchmark;
        #endregion
        
        #region Properties
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Auto"/>.
        /// </summary>
        public static Benchmark GetTime => new(TimeResolution.Auto);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Auto"/> + the memory usage.
        /// </summary>
        public static Benchmark GetTimeAndMemory => new(true);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Seconds"/>.
        /// </summary>
        public static Benchmark GetSeconds => new(TimeResolution.Seconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.MilliSeconds"/>.
        /// </summary>
        public static Benchmark GetMilliSeconds => new(TimeResolution.MilliSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.MicroSeconds"/>.
        /// </summary>
        public static Benchmark GetMicroSeconds => new(TimeResolution.MicroSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.NanoSeconds"/>.
        /// </summary>
        public static Benchmark GetNanoSeconds => new(TimeResolution.NanoSeconds);
        /// <summary>
        /// Sets the starting timestamp from where to measure in <see cref="TimeResolution.Ticks"/>.
        /// </summary>
        public static Benchmark GetTicks => new(TimeResolution.Ticks);
        #endregion
        
        #region Constructors
        /// <summary>
        /// Assigns the starting timestamp to <see cref="startTimeStamp"/>.
        /// </summary>
        /// <param name="_TimeResolution"><see cref="timeResolution"/>.</param>
        private Benchmark(TimeResolution _TimeResolution) : this()
        {
            this.timeResolution = _TimeResolution;
            this.startTimeStamp = Stopwatch.GetTimestamp();
        }
        
        /// <summary>
        /// Assigns the starting timestamp to <see cref="startTimeStamp"/>.
        /// </summary>
        /// <param name="_MeasureMemory">Also measures the memory.</param>
        // ReSharper disable once UnusedParameter.Local
        private Benchmark(bool _MeasureMemory) : this()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();

            this.startMemory = GC.GetTotalMemory(false);
            this.timeResolution = TimeResolution.Auto;
            this.startTimeStamp = Stopwatch.GetTimestamp();
        }
        #endregion
        
        #region Methods
        public void Dispose()
        {
            var _endTimeStamp = Stopwatch.GetTimestamp();
            var _elapsedTicks = _endTimeStamp - this.startTimeStamp;
            var _timeResolution = this.timeResolution;
            var _usedMemory = 0m;
            var _memoryUnity = string.Empty;
            
            if (this.startMemory.HasValue)
            {
                _usedMemory = GC.GetTotalMemory(false) - this.startMemory.Value;
                _usedMemory = GetAppropriateMemoryUnit(_usedMemory, out _memoryUnity);
            }
            
#pragma warning disable CS8524
            var _elapsedTime = this.timeResolution switch
#pragma warning restore CS8524
            {
                TimeResolution.Auto => GetAppropriateTimeResolution(_elapsedTicks, Stopwatch.Frequency, out _timeResolution),
                TimeResolution.Seconds => CalculateElapsedTime(TimeResolution.Seconds, _elapsedTicks, Stopwatch.Frequency),
                TimeResolution.MilliSeconds => CalculateElapsedTime(TimeResolution.MilliSeconds, _elapsedTicks, Stopwatch.Frequency),
                TimeResolution.MicroSeconds => CalculateElapsedTime(TimeResolution.MicroSeconds, _elapsedTicks, Stopwatch.Frequency),
                TimeResolution.NanoSeconds => CalculateElapsedTime(TimeResolution.NanoSeconds, _elapsedTicks, Stopwatch.Frequency),
                TimeResolution.Ticks => _elapsedTicks,
            };
            
            var _outputString = GetOutputString(_elapsedTime, _timeResolution, _usedMemory, _memoryUnity);
            
            Debug.Log(_outputString);
        }
        
        /// <summary>
        /// Calculates the appropriate <see cref="TimeResolution"/> based on the given <c>_ElapsedTicks</c>.
        /// </summary>
        /// <param name="_ElapsedTicks">The ticks to calculate the <see cref="TimeResolution"/> for.</param>
        /// <param name="_Frequency"><see cref="Stopwatch.Frequency"/>.</param>
        /// <param name="_TimeResolution">The <see cref="TimeResolution"/> that was calculated with.</param>
        /// <returns><c>_ElapsedTicks</c> converted to the appropriate <see cref="TimeResolution"/>.</returns>
        private static decimal GetAppropriateTimeResolution(decimal _ElapsedTicks, decimal _Frequency, out TimeResolution _TimeResolution)
        {
            var _elapsedSeconds = _ElapsedTicks / _Frequency;
            
            if (_elapsedSeconds >= 1)
            {
                _TimeResolution = TimeResolution.Seconds;
                return _elapsedSeconds;
            }
            if (_elapsedSeconds >= 0.001m)
            {
                _TimeResolution = TimeResolution.MilliSeconds;
                return CalculateElapsedTime(TimeResolution.MilliSeconds, _ElapsedTicks, _Frequency);
            }
            if (_elapsedSeconds >= 0.000_001m)
            {
                _TimeResolution = TimeResolution.MicroSeconds;
                return CalculateElapsedTime(TimeResolution.MicroSeconds, _ElapsedTicks, _Frequency);
            }
            if (_elapsedSeconds >= 0.000_000_001m)
            {
                _TimeResolution = TimeResolution.NanoSeconds;
                return CalculateElapsedTime(TimeResolution.NanoSeconds, _ElapsedTicks, _Frequency);
            }

            _TimeResolution = TimeResolution.Ticks;
            return _ElapsedTicks;
        }

        /// <summary>
        /// Calculates the elapsed time based on the given <see cref="TimeResolution"/>.
        /// </summary>
        /// <param name="_TimeResolution">The <see cref="TimeResolution"/> to calculate with.</param>
        /// <param name="_ElapsedTicks">The ticks to convert to the given <see cref="TimeResolution"/>.</param>
        /// <param name="_Frequency"><see cref="Stopwatch.Frequency"/>.</param>
        /// <returns><c>_ElapsedTicks</c> converted to the given <see cref="TimeResolution"/>.</returns>
        private static decimal CalculateElapsedTime(TimeResolution _TimeResolution, decimal _ElapsedTicks, decimal _Frequency)
        {
            var _elapsedSeconds = _ElapsedTicks / _Frequency;
            
            return _TimeResolution switch
            {
                TimeResolution.Seconds => _elapsedSeconds,
                TimeResolution.MilliSeconds => _elapsedSeconds * 1_000m,
                TimeResolution.MicroSeconds => _elapsedSeconds * 1_000_000m,
                TimeResolution.NanoSeconds => _elapsedSeconds * 1_000_000_000m,
                _ => _ElapsedTicks
            };
        }

        /// <summary>
        /// Calculates the appropriate memory unit based on the given <c>_MemoryUsed</c>.
        /// </summary>
        /// <param name="_UsedMemory">The used memory in bytes.</param>
        /// <param name="_MemoryUnit">The unity the memory usage is measured in.</param>
        /// <returns>The used memory converted to the appropriate unit.</returns>
        private static decimal GetAppropriateMemoryUnit(decimal _UsedMemory, out string _MemoryUnit)
        {
            const decimal _KB = 1024m;
            const decimal _MB = 1024m * _KB;
            const decimal _GB = 1024m * _MB;
            decimal _usedMemory;
            
            switch (_UsedMemory)
            {
                case >= _GB:
                    _usedMemory = _UsedMemory / _GB;
                    _MemoryUnit = "GB";
                    break;
                case >= _MB:
                    _usedMemory = _UsedMemory / _MB;
                    _MemoryUnit = "MB";
                    break;
                case >= _KB:
                    _usedMemory = _UsedMemory / _KB;
                    _MemoryUnit = "KB";
                    break;
                default:
                    _usedMemory = _UsedMemory;
                    _MemoryUnit = "B";
                    break;
            }

            return _usedMemory;
        }
        
        /// <summary>
        /// Creates the output <see cref="string"/> that displays the elapsed time and memory usage.
        /// </summary>
        /// <param name="_ElapsedTime">The elapsed time to display.</param>
        /// <param name="_TimeResolution">The <see cref="TimeResolution"/> in which the <c>_ElapsedTime</c> was calculated in.</param>
        /// <param name="_UsedMemory">The memory that was used during this <see cref="Benchmark"/>.</param>
        /// <param name="_MemoryUnit">The unity the memory was calculated in.</param>
        /// <returns>A <see cref="string"/> that displayed the elapsed time + the <see cref="TimeResolution"/> and memory usage.</returns>
        private static string GetOutputString(decimal _ElapsedTime, TimeResolution _TimeResolution, decimal _UsedMemory, string _MemoryUnit)
        {
            var _time = $"Elapsed Time: {_ElapsedTime.TrimTrailingZero().Bold()} {_TimeResolution.GetName()}";
            var _memory = $"Memory Used: {_UsedMemory.TrimTrailingZero().Bold()} {_MemoryUnit}";
            
            return _time + (_MemoryUnit != string.Empty ? $" | {_memory}" : string.Empty);
        }

        /// <summary>
        /// Measures the median time and memory usage of the given <see cref="Action"/>.
        /// </summary>
        /// <param name="_Action">The <see cref="Action"/> to measure the time of.</param>
        /// <param name="_WarmupIterations">The number of times to run the <see cref="Action"/> before the time is measured.</param>
        /// <param name="_Iterations">The number of iterations to measure the average time of.</param>
        /// <param name="_ProgressCallback">Callback that holds the current iteration count.</param>
        /// <returns>An awaitable <see cref="Task"/> that completes after all iterations have been run.</returns>
        public static Task Run(Action _Action, uint _WarmupIterations = DEFAULT_WARMUP_ITERATION, uint _Iterations = DEFAULT_ITERATIONS, Action<uint> _ProgressCallback = null)
        {
            return Run(AsyncAction, _WarmupIterations, _Iterations, _ProgressCallback);

            Task AsyncAction()
            {
                _Action();
                return Task.CompletedTask;
            }
        }
        
        /// <summary>
        /// Measures the median time and memory usage of the given <see cref="Func{Task}"/>.
        /// </summary>
        /// <param name="_Action">The <see cref="Func{Task}"/> to measure the time of.</param>
        /// <param name="_WarmupIterations">The number of times to run the <see cref="Func{Task}"/> before the time is measured.</param>
        /// <param name="_Iterations">The number of iterations to measure the average time of.</param>
        /// <param name="_ProgressCallback">Callback that holds the current iteration count.</param>
        /// <returns>An awaitable <see cref="Task"/> that completes after all iterations have been run.</returns>
        public static Task Run(Func<Task> _Action, uint _WarmupIterations = DEFAULT_WARMUP_ITERATION, uint _Iterations = DEFAULT_ITERATIONS, Action<uint> _ProgressCallback = null)
        {
            if (_Iterations == 0)
            {
                throw new ArgumentException("Iterations must be greater than 0.");
            }
            
            lock (benchmarkLock)
            {
                if (benchmarkIsRunning)
                {
                    Debug.LogError("A benchmark us currently running, you have to cancel it before starting a new one.");
                    
                    return Task.CompletedTask;
                }
                
                benchmarkIsRunning = true;
            }

            cancelBenchmark?.Dispose();
            cancelBenchmark = new CancellationTokenSource();
            
            try
            {
                return Task.Run(async () =>
                {
                    var _iterationCount = 0u;
                    
                    foreach (var _ in _WarmupIterations)
                    {
                        cancelBenchmark.Token.ThrowIfCancellationRequested();

                        await _Action();
                        
                        _ProgressCallback?.Invoke(++_iterationCount);
                    }

                    var _ticks = new long[_Iterations];
                    var _memoryAllocations = new long[_Iterations];

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    foreach (uint _iteration in _Iterations)
                    {
                        cancelBenchmark.Token.ThrowIfCancellationRequested();

                        var _allocatedBefore = GC.GetTotalMemory(false);
                        var _startTicks = Stopwatch.GetTimestamp();

                        await _Action();

                        var _endTicks = Stopwatch.GetTimestamp();
                        var _allocatedAfter = GC.GetTotalMemory(false);

                        var _index = _iteration - 1;
                        _ticks[_index] = _endTicks - _startTicks;
                        _memoryAllocations[_index] = _allocatedAfter - _allocatedBefore;

                        _ProgressCallback?.Invoke(++_iterationCount);
                    }

                    _ticks.SortAscending();
                    _memoryAllocations.SortAscending();

                    var _medianTicks = _ticks.Median(false);
                    var _medianMemory = _memoryAllocations.Median(false);

                    var _time = GetAppropriateTimeResolution(_medianTicks, Stopwatch.Frequency, out var _timeResolution);
                    var _memory = GetAppropriateMemoryUnit(_medianMemory, out var _memoryUnit);

                    var _outputString = GetOutputString(_time, _timeResolution, _memory, _memoryUnit);

                    Debug.Log(_outputString);

                }, cancelBenchmark.Token);
            }
            catch (TaskCanceledException) { /* No need to print the exception on cancel. */ }
            catch (OperationCanceledException) { /* No need to print the exception on cancel. */ }
            finally
            {
                lock (benchmarkLock)
                {
                    benchmarkIsRunning = false;
                }
            }

            return Task.CompletedTask;
        }
        
        /// <summary>
        /// Cancels the currently running benchmark.
        /// </summary>
        public static void CancelBenchmark()
        {
            cancelBenchmark?.Cancel();
            
            lock (benchmarkLock)
            {
                benchmarkIsRunning = false;
            }
        }
        #endregion
    }
}