using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymJourney.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GymJourney.Services;
using Xamarin.Essentials;

namespace GymJourney
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    //Code-behind class for view of list of Gyms
    //By Ahmed Abdelsalam
    public partial class GymList : ContentPage
    {
        //This private variable instantiates Google Place API service class
        private GooglePlaceService _gps = new GooglePlaceService();
        //This private variable stores mile radius for scan
        private int _miles;
        
        //default constructor
        public GymList()
        {
            InitializeComponent();
        }

        //OnAppearing the scan class is called to scan for gyms and update templateView list
        protected override async void OnAppearing()
        {
            Scan();
        }

        //This Scan class takes the mile value from stepper
        //Tries to get last known GPS location, otherwise..
        //Tries to get New GPS location with medium accuracy and timeout of 30 seconds
        //if location variable iss non-initiated then throws exception
        //Gets gyms information from service class and inputs user location and miles radius parameters
        //updates Listview itemtemplate
        private async void Scan()
        {
            _miles = (int) stepper.Value;
            Xamarin.Essentials.Location loc = null;
            //=============================================
            // Reference A3: externally sourced algorithm
            // Purpose: implementation of Xamarin.Essentials.Geolocation to find user's location
            // Date: 8/11/2020
            // Source: Xamarin.Essentials.Documentation Related Video
            // Author: Xamarin
            // url: https://docs.microsoft.com/en-us/xamarin/essentials/geolocation?tabs=android  (related video)
            // Adaptation: changed messages for exception
            //=============================================
            try
            {
                loc = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
                if (loc == null)
                {
                    loc = await Xamarin.Essentials.Geolocation.GetLocationAsync(new GeolocationRequest { DesiredAccuracy = GeolocationAccuracy.Medium, Timeout = TimeSpan.FromSeconds(30) });
                    if (loc == null)
                    {
                        throw new Exception("location could not be found");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to acquire location: {ex.Message}");
            }
            //=============================================
            // End reference A3
            //=============================================
            var root = await _gps.GetGyms(loc.Latitude, loc.Longitude, _miles * 1000);
            listGym.ItemsSource = root.Results;
        }


        //This event handler for refresh button click does the same as Scan() but tries to get new location first then tries to get most recent location
        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            _miles = (int)stepper.Value;
            Xamarin.Essentials.Location loc = null;
            try
            {
                loc = await Xamarin.Essentials.Geolocation.GetLocationAsync(new GeolocationRequest { DesiredAccuracy = GeolocationAccuracy.Medium, Timeout = TimeSpan.FromSeconds(30) });
                if (loc == null)
                {
                    loc = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
                    if (loc == null)
                    {
                        throw new Exception("location could not be found");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to acquire location: {ex.Message}");
            }
            var root = await _gps.GetGyms(loc.Latitude, loc.Longitude, _miles * 1000);
            listGym.ItemsSource = root.Results;
        }

        //This event handler for selecting gym and passing it to GymView for detailed view
        //Also passes user's location as parameter to GymView
        private async void listGym_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var result = e.SelectedItem as Result;
            listGym.SelectedItem = null;
            var loc = await Xamarin.Essentials.Geolocation.GetLastKnownLocationAsync();
            await Navigation.PushAsync(new GymView(result, loc));
        }

        //This Scan even handler next to radius update just calls Scan()
        private void Scan_Clicked(object sender, EventArgs e)
        {
            Scan();
        }
    }
}