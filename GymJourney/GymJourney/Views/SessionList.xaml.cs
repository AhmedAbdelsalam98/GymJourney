using GymJourney.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GymJourney.Models;
using System.Runtime.CompilerServices;

namespace GymJourney
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    //Code-Behind Class for list of journals/sessions.
    //By Ahmed Abdelsalam
    public partial class SessionList : ContentPage
    {
        //private variable service instance
        ExerciseSessionManager _esm = new ExerciseSessionManager();

        //default constructor
        public SessionList()
        {
            InitializeComponent();
        }

        //This method calls service to update database and view sorting from most recent
        private async void UpdateSessionsList()
        {
            var list = await _esm.GetSessions();
            sessionsList.ItemsSource = list.OrderByDescending((s) => s.Id);
        }

        //On appearing fetches database info and updates list view
        protected override void OnAppearing()
        {
            UpdateSessionsList();
        }

        //This event-handler adds session with current time parameter and calls to update list service
        private async void OnAddSession(object sender, EventArgs e)
        {
            await _esm.AddSession(DateTime.Now);
            UpdateSessionsList();
        }

        //This event-handler deletes a session from ContextAction control
        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            var es = (sender as MenuItem).CommandParameter as ExerciseSession;
            await _esm.DeleteSession(es);
            UpdateSessionsList();
        }

        //This event-handler selects session and calls navigation push async to send session object to SessionView for detail view, also deselects 
        private async void sessionsList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;
            var _session = e.SelectedItem as ExerciseSession;
            sessionsList.SelectedItem = null;
            await Navigation.PushAsync(new SessionView(_session));
        }

    }
}