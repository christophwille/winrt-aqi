using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient.Mocks
{
    public class MockHttpClient : IHttpClient
    {
        public Task<string> GetStringAsync(string url)
        {
            return Task.FromResult(LoadAsText("sampledata.json").ToString());
        }

        public static Stream LoadStreamFromResource(string resourceName)
        {
            var assembly = typeof(MockHttpClient).GetTypeInfo().Assembly;
            return assembly.GetManifestResourceStream("AirQualityInfo.DataClient.Mocks." + resourceName);
        }

        public static StringBuilder LoadAsText(string textFileName)
        {
            using (var stream = LoadStreamFromResource(textFileName))
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return new StringBuilder(reader.ReadToEnd());
                }
            }
        }
    }
}
