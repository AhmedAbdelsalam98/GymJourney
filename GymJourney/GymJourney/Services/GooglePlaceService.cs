using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using GymJourney.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace GymJourney.Services
{
    //This service class retrieves info from Google Place API based on user's location and radius
    class GooglePlaceService
    {
        private const string Url = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
        private const string photoUrl = "https://maps.googleapis.com/maps/api/place/photo?";
        private const string Key = "AIzaSyDxQXb4QFLxPPOqcipb7lvWt0LPW36-Tiw";
        private HttpClient _client = new HttpClient();

        //This method takes latitude, longitude and radius of the user
        //calls Google Place API to retrieve gyms according to those parameters
        //Deserializes the JSON result into C# objects
        //returns the result
        public async Task<Root> GetGyms(double latitude, double longitude, int radiusKm)
        {
            //=============================================
            // Reference A3: externally sourced algorithm
            // Purpose: Contacting web services and handling JSON string deserialization.
            // Date: 8/11/2020
            // Source: Xamarin Forms: Build Native Mobile Apps with C# Course
            // Author: Mosh Hamedani
            // url: https://codewithmosh.com/courses/enrolled/223139 
            // Adaptation: Adapted to work with Google Place advanced API
            //=============================================
            var response = await _client.GetAsync($"{Url}location={latitude},{longitude}&radius={radiusKm}&type=gym&key={Key}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Root>(content);
            //=============================================
            // End reference A3
            //=============================================
        }


        //This static method takes id reference a for photo and retrieves the photo from Google's photo API
        public static string GetPictureUrl(string idreference)
        {
            return $"{photoUrl}maxwidth=400&photoreference={idreference}&key={Key}";
        }
    }
}
