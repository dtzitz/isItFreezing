using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace isItFreezing
{
    class OpenWeatherProxy
    {
        public class OpenWeatherMapProxy
        {
            public static async Task<RootObject> GetWeatherAsync(int zipcode)
            {
                var http = new HttpClient();
                string weatherUrl = String.Format("http://api.openweathermap.org/data/2.5/weather?zip={0},us&appid=1597aa818dd8c98bd122399d4e92dc85&units=imperial", zipcode);
                var response = await http.GetAsync(weatherUrl);
                var result = await response.Content.ReadAsStringAsync();
                var seralizer = new DataContractJsonSerializer(typeof(RootObject));
                var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));

                var data = (RootObject)seralizer.ReadObject(ms);
                return data;
            }
        }
        [DataContract]
        public class Coord
        {
            [DataMember]
            public double lon { get; set; }

            [DataMember]
            public double lat { get; set; }
        }

        [DataContract]
        public class Weather
        {
            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string main { get; set; }

            [DataMember]
            public string description { get; set; }

            [DataMember]
            public string icon { get; set; }
        }

        [DataContract]
        public class Main
        {
            [DataMember]
            public double temp { get; set; }

            [DataMember]
            public int pressure { get; set; }

            [DataMember]
            public int humidity { get; set; }

            [DataMember]
            public double temp_min { get; set; }

            [DataMember]
            public double temp_max { get; set; }
        }

        [DataContract]
        public class Wind
        {
            [DataMember]
            public double speed { get; set; }

            [DataMember]
            public double deg { get; set; }
        }

        [DataContract]
        public class Clouds
        {
            [DataMember]
            public int all { get; set; }
        }

        [DataContract]
        public class Sys
        {
            [DataMember]
            public int type { get; set; }

            [DataMember]
            public int id { get; set; }

            [DataMember]
            public double message { get; set; }

            [DataMember]
            public string country { get; set; }

            [DataMember]
            public int sunrise { get; set; }

            [DataMember]
            public int sunset { get; set; }
        }

        [DataContract]
        public class RootObject
        {
            [DataMember]
            public Coord coord { get; set; }

            [DataMember]
            public List<Weather> weather { get; set; }

            [DataMember]
            public string @base { get; set; }

            [DataMember]
            public Main main { get; set; }

            [DataMember]
            public int visibility { get; set; }

            [DataMember]
            public Wind wind { get; set; }

            [DataMember]
            public Clouds clouds { get; set; }

            [DataMember]
            public int dt { get; set; }

            [DataMember]
            public Sys sys { get; set; }

            [DataMember]
            public int id { get; set; }

            [DataMember]
            public string name { get; set; }

            [DataMember]
            public int cod { get; set; }
        }
    }
}
