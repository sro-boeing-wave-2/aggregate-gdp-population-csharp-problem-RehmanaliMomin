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

    public class FileOperations
    {
        //Function to read file asynchronously
        public static async Task<string> ReadFileAsync(string filepath)
        {
            string data = "";
            using (StreamReader streamReader = new StreamReader(filepath))
            {
                data = await streamReader.ReadToEndAsync();
            }
            return data;
        }

        //Function to write file asynchronously
        public static async void WriteFileAsync(string filepath, string data)
        {
            using (StreamWriter streamwriter = new StreamWriter(filepath))
            {
                await streamwriter.WriteLineAsync(data);
            }
        }
    }


    public class GDPAggregate
    {

        //Function to convert string json file into dictionary object
        public static Dictionary<string, string> JsonToDictionary(string DataFile)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(DataFile);
        }

        //Update Map, add countries data
        public static void UpdateContinentData(Dictionary<string, Dictionary<string, float>> mainDict, string continent, int gdpIndex, int populationIndex, string[] row)
        {
            Dictionary<string, float> continentDataDictionary = mainDict[continent];
            continentDataDictionary["GDP_2012"] = continentDataDictionary["GDP_2012"] + float.Parse(row[gdpIndex]);
            continentDataDictionary["POPULATION_2012"] = continentDataDictionary["POPULATION_2012"] + float.Parse(row[populationIndex]);
        }

        //Add new continentCountrydata
        public static void AddNewContinenData(Dictionary<string, Dictionary<string, float>> mainDict, string continent, float gdp, float pop)
        {
            Dictionary<string, float> NewContinentDataDictionary = new Dictionary<string, float>();
            NewContinentDataDictionary.Add("GDP_2012", gdp);
            NewContinentDataDictionary.Add("POPULATION_2012", pop);
            mainDict.Add(continent, NewContinentDataDictionary);
        }

        // Main method
        public async static void OperationsMethod()
        {
            Task<string> dataTask = FileOperations.ReadFileAsync("../../../../AggregateGDPPopulation/data/datafile.csv");
            Task<string> mappingTask = FileOperations.ReadFileAsync("../../../../AggregateGDPPopulation/data/continent-country.json");

           
            string dataFile = await dataTask;

            string dataWithoutQuotes = dataFile.Replace("\"", "");
            string[] rows = Regex.Split(dataWithoutQuotes, "\n");
            string[] header = rows[0].Split(',');

            //string dataMapping = File.ReadAllText(@"../../../../AggregateGDPPopulation/data/continent-country.json", Encoding.UTF8);
            //Dictionary<string, string> countryContinentMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapFile);
        

            int countryNameIndex = Array.IndexOf(header, "Country Name");
            int populationIndex = Array.IndexOf(header, "Population (Millions) 2012");
            int gdpIndex = Array.IndexOf(header, "GDP Billions (USD) 2012");

            Dictionary<string, Dictionary<string, float>> outputMap = new Dictionary<string, Dictionary<string, float>>();

            string mapFile = await mappingTask;
            Dictionary<string, string> countryContinentMap = JsonToDictionary(mapFile);

            for (int i = 1; i < rows.Length - 2; i++)
            {
                string[] row = rows[i].Split(',');
                string country = row[countryNameIndex];
                string continent = countryContinentMap[country];

                if (!outputMap.ContainsKey(continent))
                {
                    AddNewContinenData(outputMap, continent, float.Parse(row[gdpIndex]), float.Parse(row[populationIndex]));
                }
                else
                {
                    UpdateContinentData(outputMap, continent, gdpIndex, populationIndex, row);
                }
            }
            string outputString = JsonConvert.SerializeObject(outputMap);
            FileOperations.WriteFileAsync("../../../../AggregateGDPPopulation/output/output.json", outputString);
        }
    }
}
