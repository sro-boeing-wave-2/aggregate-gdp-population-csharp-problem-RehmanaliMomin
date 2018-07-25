using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            GDPAggregate.OperationsMethod();

            var expected = File.ReadAllText(@"../../../expected-output.json");
            var actual = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/output.json");

            JObject actualJson = JObject.Parse(actual);
            JObject expectedJson = JObject.Parse(expected);

            Console.WriteLine(actual);
            Console.WriteLine("------");
            Console.WriteLine(expected);
            Assert.Equal(expectedJson, actualJson);
        }
    }
}
