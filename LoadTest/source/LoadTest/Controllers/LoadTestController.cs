// <copyright file="LoadTestController.cs" company="Omnicell Inc.">
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LoadTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadTestController : ControllerBase
    {
        
        private readonly ILogger<LoadTestController> _logger;

        private readonly IConfiguration _configuration;

        public LoadTestController(ILogger<LoadTestController> logger)
        {
            _logger = logger;

            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        [HttpGet("{task}")]
        public IActionResult Get(string task="cpu")
        {
            if (task == "cpu")
            {
                try
                {
                    int minValue = 1000;
                    int maxValue = 5000;
                    int iterations = 10;
                    var rng = new Random();
                    int elapsedMilseconds = rng.Next(minValue, maxValue);

                    IEnumerable<LoadTest> loadtests = Enumerable.Range(1, iterations).Select(
                        index => CpuLoader.GetCPULoadTest(elapsedMilseconds)).ToArray();
                    return Ok(loadtests);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return Problem(ex.Message);
                }
            } 
            else
            {
                try
                {
                    int iterations = 5;
                    int slidingexpiration = 6000;
                    IEnumerable<LoadTest> loadtests = MemLoader.GetMemoryLoadTest(iterations, slidingexpiration);
                    return Ok(loadtests);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.StackTrace);
                    return Problem(ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("api/authuser")]
        public IActionResult authuser([FromBody] TestUser testUser)
        {
            try
            {
                var rng = new Random();
                if(testUser.token != "omnicell")
                {
                    throw new Exception("Invalid Token.");
                }
                string user = JsonConvert.SerializeObject(testUser);
                var userBytes = System.Text.Encoding.UTF8.GetBytes(user);
                string hash;
                using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
                {
                    hash = BitConverter.ToString(md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user))
                    ).Replace("-", String.Empty);
                }

                string encodedtext = Convert.ToBase64String(userBytes);
                encodedtext += "=hash=" + hash;
                encodedtext += "=timestamp=" + DateTime.Now.ToString();

                int sleepMilseconds = rng.Next(500, 1000);
                Thread.Sleep(sleepMilseconds);

                int slidingexpiration = 600;
                IEnumerable<LoadTest> loadtests = MemLoader.GetMemoryLoadTest(1, slidingexpiration, encodedtext);
                return Ok(loadtests);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return Problem(ex.Message);
            }
        }

    }
}
