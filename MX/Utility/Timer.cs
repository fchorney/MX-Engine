using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MX
{
    public class Timer
    {
        private static Timer t;
        private bool isUsingQPF;
        private bool isTimerStopped;
        private long ticksPerSecond;
        private long stopTime;
        private long lastElapsedTime;
        private long baseTime;

        public bool Stopped { get { return isTimerStopped; } } // Returns true if timer stopped

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceFrequency(ref long PerformanceFrequency);

        [System.Security.SuppressUnmanagedCodeSecurity]
        [DllImport("kernel32")]
        public static extern bool QueryPerformanceCounter(ref long PerformanceCount);

        public Timer()
        {
            isTimerStopped = true;
            ticksPerSecond = 0;
            stopTime = 0;
            lastElapsedTime = 0;
            baseTime = 0;
            // Use QueryPerformanceFrequency to get frequency of the timer
            isUsingQPF = QueryPerformanceFrequency(ref ticksPerSecond);
            if(t != null) t = new Timer();
        }

        public void Reset()
        { // Resets the timer
            if (!isUsingQPF) return; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;

            //if (stopTime != 0) time = stopTime;
            //else 
            QueryPerformanceCounter(ref time);

            baseTime = time;
            lastElapsedTime = time;
            stopTime = 0;
            isTimerStopped = false;
        }

        public void Start()
        { // Starts the timer
            if (!isUsingQPF) return; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;

            if (stopTime != 0) time = stopTime;
            else QueryPerformanceCounter(ref time);

            if (isTimerStopped) baseTime += (time - stopTime);
            stopTime = 0;
            lastElapsedTime = time;
            isTimerStopped = false;
        }

        public void Stop()
        { // Stop (or pause) the timer
            if (!isUsingQPF)
                return; // Nothing to do

            if (!isTimerStopped)
            {
                // Get either the current time or the stop time
                long time = 0;
                if (stopTime != 0)
                    time = stopTime;
                else
                    QueryPerformanceCounter(ref time);

                stopTime = time;
                lastElapsedTime = time;
                isTimerStopped = true;
            }
        }

        public void Advance()
        {    // Advance the timer a tenth of a second
            if (!isUsingQPF) return; // Nothing to do

            stopTime += ticksPerSecond / 10;
        }

        public double GetAbsoluteTime()
        { // Get the absolute system time
            if (!isUsingQPF) return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;

            if (stopTime != 0) time = stopTime;
            else QueryPerformanceCounter(ref time);

            double absoluteTime = time / (double)ticksPerSecond;
            return absoluteTime;
        }

        public static double Now()
        {
            return t.GetAbsoluteTime();
        }

        public double GetTime()
        { // Get the current time
            if (!isUsingQPF) return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0) time = stopTime;
            else QueryPerformanceCounter(ref time);

            double appTime = (double)(time - baseTime) / (double)ticksPerSecond;
            return appTime;
        }

        public double GetElapsedTime()
        { // Get time elapsed between GetElapsedTime() calls
            if (!isUsingQPF) return -1.0; // Nothing to do

            // Get either the current time or the stop time
            long time = 0;
            if (stopTime != 0) time = stopTime;
            else QueryPerformanceCounter(ref time);

            double elapsedTime = (double)(time - lastElapsedTime) / (double)ticksPerSecond;
            lastElapsedTime = time;
            return elapsedTime;
        }
    }
}