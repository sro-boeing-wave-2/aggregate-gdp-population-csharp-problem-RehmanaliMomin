using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AggregateGDPPopulation
{
    public class Class1
    {
        public static async Task<string> ReadFileAsync(string filepath)
        {
            string data = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                data = await streamReader.ReadToEndAsync();
            }
            return data;
        }

        public static async void WriteFileAsync(string filepath, string data)
        {
            using (StreamWriter streamwriter = new StreamWriter(filepath))
            {
                await streamwriter.WriteLineAsync(data);
            }
        }


        public static async Task OperationsMethod()
        {
            Task<string> dataTask = ReadFileAsync("../../../../AggregateGDPPopulation/data/datafile.csv");
            Task<string> mappingTask = ReadFileAsync("../../../../AggregateGDPPopulation/data/continent-country.json");

            string dataFile = await dataTask;
            string mapFile = await mappingTask;

            string dataWithoutQuotes = dataFile.Replace("\"", "");
            string[] rows = Regex.Split(dataWithoutQuotes, "\n");
            string[] header = rows[0].Split(',');


            //string dataMapping = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/continent-country.json", Encoding.UTF8);
            var countryContinentMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapFile);

            int countryNameIndex = Array.IndexOf(header, "Country Name");
            int populationIndex = Array.IndexOf(header, "Population (Millions) 2012");
            int gdpIndex = Array.IndexOf(header, "GDP Billions (USD) 2012");

            Dictionary<string, OutputObject> outputMap = new Dictionary<string, OutputObject>();

            for (int i = 1; i < rows.Length - 2; i++)
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
            Console.WriteLine("Output String");
            Console.WriteLine(outputString);
            WriteFileAsync("../../../../AggregateGDPPopulation/output/output.json", outputString);
        }


        class OutputObject
        {
            public float GDP_2012 { get; set; }
            public float POPULATION_2012 { get; set; }
        }
    }
}
