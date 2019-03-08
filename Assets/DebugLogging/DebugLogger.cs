using System;
using System.Collections.Generic;
using UnityEngine;

namespace DebugLogging
{
    public static class DebugLogger
    {
        // TODO: create configuration/options class where this can be changed. default should
        // be error when this is done.
        public static LogLevel LogLevel = LogLevel.Informational; // change to error when the todo is done.
        public static bool ContinueOnException = false;

        private const string DEFAULT_SOURCE = "Unknown";
        private const string TRACE_START = "Starting Trace";
        private const string TRACE_END = "Ending Trace";
        private static Dictionary<string, Trace> outstadingTraces = new Dictionary<string, Trace>();
        
        /// <summary>
        /// For non-fatal debug logging
        /// </summary>
        public static void Log(LogLevel level, string message, string source = DEFAULT_SOURCE)
        {
            if (level == LogLevel.Fatal)
            {
                Log(new Exception("Cannot use Log(LogLevel, string) for Fatal Exceptions"));
            }
            if (level >= LogLevel)
            {
                PublishMessage(level, message, source);
            }
        }

        /// <summary>
        /// For non-fatal debug logging
        /// </summary>
        public static void Log(LogLevel level, string message, Type t)
        {
            Log(level, message, t.FullName);
        }

        /// <summary>
        /// Start or end a trace which can track the performance between the start and end.
        /// </summary>
        /// <param name="key"></param>
        public static void Trace(string key)
        {
            if (LogLevel > LogLevel.Trace) return;

            if (!outstadingTraces.ContainsKey(key))
            {
                // output to log
                Log(LogLevel.Trace, TRACE_START, key);
                // start new trace
                outstadingTraces.Add(key, new Trace());
            }
            else
            {
                Trace t = outstadingTraces[key];
                // output to log
                Log(LogLevel.Trace, $"{TRACE_END} :: {t.PerformanceMS}ms", key);
                // kill old trace
                outstadingTraces[key].Dispose();
                outstadingTraces.Remove(key);
            }
        }

        /// <summary>
        /// All exceptions in the program should be funneled through here
        /// </summary>
        /// <param name="ex"></param>
        public static void Log(Exception ex)
        {
            if (LogLevel != LogLevel.None)
            {
                PublishMessage(LogLevel.Fatal, ex.Message, ex.Source);
                //TODO: More stuff in here related to ex?
            }

            if (!ContinueOnException)
            {
                throw ex;
            }
        }

        private static void PublishMessage(LogLevel level, string message, string source)
        {
            string formattedMessage = $"{source}::{message}";
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Informational:
                case LogLevel.GameMessage:
                    Debug.Log(formattedMessage);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(formattedMessage);
                    break;
                case LogLevel.Error:
                case LogLevel.Fatal:
                    Debug.LogError(formattedMessage);
                    break;
                default:
                    Log(new ArgumentOutOfRangeException("Invalid LogLevel"));
                    break;
            }
        }
    }

    public enum LogLevel
    {
        Trace,
        Informational,
        GameMessage,
        Warning,
        Error,
        Fatal,
        None
    }
}