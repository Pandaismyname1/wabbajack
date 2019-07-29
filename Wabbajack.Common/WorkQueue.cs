﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wabbajack.Common
{
    public class WorkQueue
    {
        private static BlockingCollection<Action> Queue = new BlockingCollection<Action>();

        [ThreadStatic]
        private static int CpuId;

        public static void Init(Action<int, string, int> report_function)
        {
            ReportFunction = report_function;
            ThreadCount = Environment.ProcessorCount;
            StartThreads();
        }

        private static void StartThreads()
        {
            Threads = Enumerable.Range(0, ThreadCount)
                                .Select(idx =>
                                {
                                    var thread = new Thread(() => ThreadBody(idx));
                                    thread.Priority = ThreadPriority.BelowNormal;
                                    thread.IsBackground = true;
                                    thread.Name = String.Format("Wabbajack_Worker_{0}", idx);
                                    thread.Start();
                                    return thread;
                                }).ToList();
        }

        private static void ThreadBody(int idx)
        {
            CpuId = idx;

            while(true)
            {
                Report("Waiting", 0);
                var f = Queue.Take();
                f();
            }
            
        }
        public static void Report(string msg, int progress)
        {
            ReportFunction(CpuId, msg, progress);
        }

        public static void QueueTask(Action a)
        {
            Queue.Add(a);
        }

        public static Action<int, string, int> ReportFunction { get; private set; }
        public static int ThreadCount { get; private set; }
        public static List<Thread> Threads { get; private set; }
    }
}