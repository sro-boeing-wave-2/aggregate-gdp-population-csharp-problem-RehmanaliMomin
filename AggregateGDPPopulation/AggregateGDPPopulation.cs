using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AggregateGDPPopulation
{
    public class Class1
    {

        public static void Solution()
        {
            string data = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/datafile.csv", Encoding.UTF8);
            string dataWithoutQuotes = data.Replace("\"", "");
            string[] rows = Regex.Split(dataWithoutQuotes, "\r\n");
            string[] header = rows[0].Split(',');

            string dataMapping = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/continent-country.json", Encoding.UTF8);
            var countryContinentMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(dataMapping);

            int countryNameIndex = Array.IndexOf(header, "Country Name");
            int populationIndex = Array.IndexOf(header, "Population (Millions) 2012");
            int gdpIndex = Array.IndexOf(header, "GDP Billions (USD) 2012");

            Dictionary<string, OutputObject> outputMap = new Dictionary<string, OutputObject>();

            for (int i = 1; i < rows.Length-2; i++)
            {
                string[] row = rows[i].Split(',');
                string country = row[countryNameIndex];
                string continent = countryContinentMap[country];

                if (!outputMap.ContainsKey(continent))
                {
                    OutputObject obj = new OutputObject();

                    obj.GDP_2012 = float.Parse(row[gdpIndex]);
                    obj.POPULATION_2012 = float.Parse(row[populationIndex]);
                    outputMap.Add(continent, obj);
                }
                else
                {

                    OutputObject obj = outputMap[continent];
                    obj.GDP_2012 += float.Parse(row[gdpIndex]);
                    obj.POPULATION_2012 += float.Parse(row[populationIndex]);
                }
            }

            string outputString = JsonConvert.SerializeObject(outputMap);

          
            System.IO.File.WriteAllText(@"../../../../output/output.json", outputString);

           // Console.WriteLine(outputString);

        
        }

        class OutputObject
        {
            public float GDP_2012 { get; set; }
            public float POPULATION_2012 { get; set; }
        }
    }
}
