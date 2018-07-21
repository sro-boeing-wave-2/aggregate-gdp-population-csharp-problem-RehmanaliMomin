using System;
using System.IO;
using System.Text;

namespace AggregateGDPPopulation
{
    public class Class1
    {

        public static void getAggregate()
        {
            string text;
            using (var streamReader = new StreamReader(@"", Encoding.UTF8))
            {
                text = streamReader.ReadToEnd();
            }
        }
    }
}
