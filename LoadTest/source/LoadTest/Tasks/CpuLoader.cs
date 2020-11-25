// <copyright file="CpuLoader.cs" company="Omnicell Inc.">
// Copyright (c) 2020 Omnicell Inc. All rights reserved.
//
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is
// prohibited without the prior written consent of the copyright owner.

// </copyright>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LoadTest
{
    public class CpuLoader
    {

        public static LoadTest GetCPULoadTest(int elapsedMilseconds)
        {
            LoadTest loadTest = new LoadTest();
            loadTest.LoadTaskAction = "CpuUsages";
            loadTest.CpuUsage = elapsedMilseconds;
            Task.Run(() => ConsumeCPU(elapsedMilseconds));
            return loadTest;
        }

        public static void ConsumeCPU(int elapsedMilseconds)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                if (watch.ElapsedMilliseconds > elapsedMilseconds)
                {
                    Thread.Sleep(100);
                    watch.Reset();
                    watch.Stop();
                    break;
                }
            }
        }
    }
}
