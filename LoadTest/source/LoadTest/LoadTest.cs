// <copyright file="LoadTest.cs" company="Omnicell Inc.">
// Copyright (c) 2020 Omnicell Inc. All rights reserved.
//
// Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is
// prohibited without the prior written consent of the copyright owner.
// </copyright>


using System;
using System.Threading.Tasks;

namespace LoadTest
{
    public class LoadTest
    {
        public string LoadTaskAction { get; set; }

        public long? MemoryCached { get; set; }

        public int CpuUsage { get; set; }

        public string Message { get; set; }

    }
}
