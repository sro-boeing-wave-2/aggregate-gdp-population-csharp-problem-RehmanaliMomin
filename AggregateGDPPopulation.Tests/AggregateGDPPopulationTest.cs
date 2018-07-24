using Newtonsoft.Json.Linq;
using System;
using System.IO;
using Xunit;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()             //..\..\..\..\AggregateGDPPopulation\data\datafile.csv
        {
            Class1.Solution();
            var actual = File.ReadAllText(@"../../../expected-output.json");
            var expected = File.ReadAllText(@"../../../../output/output.json");

            JObject actualJson = JObject.Parse(actual);
            JObject expectedJson = JObject.Parse(expected);

            Assert.Equal(actualJson, expectedJson);

        }
    }
}
