using GymJourney.Models;
using GymJourney.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GymJourney
{
    //Codebehind class for Workout Session AKA. Journal View information Class
    //By: Ahmed Abdelsalam

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SessionView : ContentPage
    {
        //private variables
        private CameraService _cs = new CameraService();
        private ExerciseSessionManager _esm = new ExerciseSessionManager();
        private ExerciseSession _session;
        private IEnumerable<Checkbox> _goals;

        //Constructor with session object instance passed from the List
        // stores session in variable and sets bindingContext
        public SessionView(ExerciseSession session)
        {
            InitializeComponent();
            _session = session;
            BindingContext = _session;
        }

        //This Asynchronous method allows user to capture an image and stores its file path
        private async void Capture_Clicked(object sender, EventArgs e)
        {
            string _filepath = await _cs.TakePhoto();
            if (String.IsNullOrWhiteSpace(_filepath))
                return;
            _session.FilePath = _filepath;
            image.Source = ImageSource.FromFile(_filepath);
        }


        //This method is called on appearing and assigns goals list to list template using UpdateGoalsView() and image source
        protected override async void OnAppearing()
        {
            UpdateGoalsView();
            if (!String.IsNullOrWhiteSpace(_session.FilePath))
            {
                image.Source = ImageSource.FromFile(_session.FilePath);
            }
        }

        //This method calls service to save changes to database upon page disappearing
        protected override void OnDisappearing()
        {
            _esm.UpdateSession(_session);
            _esm.UpdateGoals(_goals);
        }

        //This method updates the goals view from database manager and orders from most recent goal
        private async void UpdateGoalsView()
        {
            var _goals = await _esm.GetGoals(_session.Id);
            checklist.ItemsSource = _goals.OrderByDescending((g) => g.Id);
        }

       //This event handler deletes goals and calls service to update database and view
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var goal = (sender as MenuItem).CommandParameter as Checkbox;
            await _esm.DeleteGoal(goal);
            UpdateGoalsView();
        }

        //This event handler adds goal according to entry cell and updates goals view
        private async void AddGoalBtnClicked(object sender, EventArgs e)
        {
            await _esm.AddGoal(_session.Id, entryCell.Text);
            UpdateGoalsView();
        }

        //stops list from being selected event-handler
        private void checklist_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            checklist.SelectedItem = null;
        }
    }
}