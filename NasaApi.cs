using System;
using System.Net;
using System.Drawing;
using Newtonsoft.Json.Linq;

namespace NASA_API_Example
{
    public class NasaApi
    {
        private readonly string _apiKey;
        private readonly string _apiUrl;

        // This class is used to connect to the NASA API and retrieve the APOD (Astronomy Picture of the Day)
        public NasaApi(string apiKey)
        {
            _apiKey = apiKey;
            _apiUrl = "https://api.nasa.gov/planetary/apod?api_key=" + _apiKey;
        }

        public string ApiKey { get => _apiKey; }
        public string ApiUrl { get => _apiUrl; }

        // Connects to the API and retrieves the APOD image URL
        private string GetImageUrl()
        {
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(_apiUrl);
                Console.WriteLine(json);
                JObject data = JObject.Parse(json);
                return (string)data["url"];
            }
        }

        // Returns the APOD image as a bitmap
        public Bitmap GetImage()
        {

            return new Bitmap(GetImageUrl());
        }
    }
}
