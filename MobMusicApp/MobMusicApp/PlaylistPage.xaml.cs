using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistPage : ContentPage
    {

        public bool IsPlaying { get; set; } = false;
        public bool FirstSongSelected { get; set; } = false;
        static DataCollections dc = new DataCollections(1);
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = dc;

        }
        public static void LoadPlaylist(string PlaylistName)
        {
            dc.ReadSongsInPlaylistFromXml(PlaylistName);
            
        }
        private void AddSong_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSongPage());
        }

        private void Pause_Clicked(object sender, EventArgs e)
        {

            if (IsPlaying && FirstSongSelected)
            {
                Pause.Source = "Unpause.png";
                IsPlaying = !IsPlaying;
            }
            else
            {
                Pause.Source = "Pause.png";
                if (FirstSongSelected)
                    IsPlaying = !IsPlaying;
            }
        }

        private void songsLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
    
}