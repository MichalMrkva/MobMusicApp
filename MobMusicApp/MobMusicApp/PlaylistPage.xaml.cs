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
        public bool FirstSongSelected { get; set; } = true;
        DataCollections dc = new DataCollections(1);
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = dc;
            PlaylistsPage.Load += LoadPlaylist;
            AddSongPage.Load += LoadPlaylist;
        }
        public void LoadPlaylist(string PlaylistName)
        {
            ME.Source = null;
            if (dc.Songs != null)
            {
                dc.Songs.Clear();
            }
            
            dc.Songs = dc.ReadSongsInPlaylistFromXml(PlaylistName);
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
            Song s = e.SelectedItem as Song;
            dc.Max = s.Length;
            dc.CurrentSong = s.Name;
            //ME.Source = s.FilePath;
        }
    }
    
}