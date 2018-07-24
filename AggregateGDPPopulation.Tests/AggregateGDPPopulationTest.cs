using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            await Class1.OperationsMethod();

            Task<string> expectedOutputTask = Class1.ReadFileAsync(@"../../../expected-output.json");
            Task<string> actualOutputTask = Class1.ReadFileAsync(@"../../../../AggregateGDPPopulation/output/output.json");

            var tasks = new List<Task>();
            tasks.Add(expectedOutputTask);
            tasks.Add(actualOutputTask);

            Task.WaitAll(tasks.ToArray());

            string expectedOutput = expectedOutputTask.Result;
            string actualOutput =  actualOutputTask.Result;
            Assert.Equal(expectedOutputTask, actualOutputTask);
        }
    }
}
