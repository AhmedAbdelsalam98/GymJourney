using GymJourney.Services;
using GymJourney.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace GymJourney
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GymView : ContentPage
    {
        //This private variable stores selected item from GymList
        private Result _result;

        //This constructor accepts the gym instance from GymList and user's location instance and initializes View values
        public GymView(Result result, Xamarin.Essentials.Location loc = null)
        {
            InitializeComponent();

            _result = result;

            //checks if user's location is null or not to calculate distance
            if (loc != null)
            {
                var value = Xamarin.Essentials.Location.CalculateDistance(loc.Latitude, loc.Longitude, result.Geometry.Location.Lat, result.Geometry.Location.Lng, DistanceUnits.Kilometers);
                distance.Text = String.Format("{0:f2} Km away approximately", value);
            }
            else
            {
                distance.Text = "Distance could not be calculated, try again later";
            }
            
            //checks if photo array from Google API is null and if not uses the first photo in array, otherwise uses a stock no photo found from the web.
            if (result.Photos != null)
            {
                image.Source = GooglePlaceService.GetPictureUrl(result.Photos[0].PhotoReference);
            }
            else
            {
                image.Source = new Uri("https://www.rspcansw.org.au/wp-content/uploads/noPhotoFound.png");
            }

            //name check and set
            if (result.Name != null)
            {
                name.Text = result.Name;
            }

            //vicinity check and set
            if (result.Vicinity != null)
            {
                vicinity.Text = result.Vicinity;
            }

            //opening hours check and set
            if (result.OpeningHours != null)
            {
                if (result.OpeningHours.OpenNow)
                {
                    isOpen.Text = "Open";
                }
                else
                {
                    isOpen.Text = "Closed";
                }
            }
            else
            {
                isOpen.IsVisible = false;
            }

            //User reviews check and set
            if (userRating != null)
            {
                if (result.UserRatingsTotal != 0)
                {
                    userRating.Text = $"Rated {result.Rating} / 5 according to {result.UserRatingsTotal} user reviews";
                }
                else
                {
                    userRating.IsVisible = false;
                }
            }
        }

        //This event handler opens map according to location latitude and longitude
        private async void MapsBtn_Clicked(object sender, EventArgs e)
        {
            await Map.OpenAsync(_result.Geometry.Location.Lat, _result.Geometry.Location.Lng);
        }
    }
}