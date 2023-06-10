using Android.Widget;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistPage : ContentPage
    {
        public bool IsNotPlaying = true;
        int index;
        public bool FirstSongSelected = false;
        Timer timer;
        DataCollections dc = new DataCollections(1);
        public PlaylistPage()
        {
            InitializeComponent();
            BindingContext = dc;
            PlaylistsPage.Load += LoadPlaylist;
            AddSongPage.Load += LoadPlaylist;
            PlaylistsPage.Hide += HideAll;
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            songProgress.Value = songProgress.Value + 1;
        }
        public void LoadPlaylist(string PlaylistName)
        {
            if (!AddSong.IsVisible)
            {
                CurrentPlaylistLabel.IsVisible = true;
                AddSong.IsVisible = true;
                songsLv.IsVisible = true;
                nowPlayingLabel.IsVisible = true;
                timeLabel.IsVisible = true;
                songProgress.IsVisible = true;
                Back.IsVisible = true;
                Pause.IsVisible = true;
                Next.IsVisible = true;
                AlertLab.IsVisible = false;
            }
            dc.CurPlaylist = PlaylistName;
            ME.Source = null;
            if (dc.Songs != null)
            {
                dc.Songs.Clear();
            }

            dc.Songs = dc.ReadSongsInPlaylistFromXml(PlaylistName);
        }
        public void HideAll()
        {
            CurrentPlaylistLabel.IsVisible = false;
            AddSong.IsVisible = false;
            songsLv.IsVisible = false;
            nowPlayingLabel.IsVisible = false;
            timeLabel.IsVisible = false;
            songProgress.IsVisible = false;
            Back.IsVisible = false;
            Pause.IsVisible = false;
            Next.IsVisible = false;
            AlertLab.IsVisible = true;
            ME.Source = null;
        }
        private void AddSong_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddSongPage());
        }

        private void Pause_Clicked(object sender, EventArgs e)
        {

            if (IsNotPlaying && FirstSongSelected)
            {
                ME.Pause();
                timer.Stop();
                Pause.Source = "Unpause.png";
                IsNotPlaying = !IsNotPlaying;
            }
            else
            {
                ME.Play();
                timer.Start();
                Pause.Source = "Pause.png";
                if (FirstSongSelected)
                    IsNotPlaying = !IsNotPlaying;
            }
        }

        private void songsLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            songProgress.Value = 0;
            IsNotPlaying = true;
            FirstSongSelected = true;
            Song s = e.SelectedItem as Song;
            if (s == null)
            {
                ME.Source = null;
                dc.Max = TimeSpan.Zero;
                dc.CurrentSong = "";
                timer.Stop();
                Pause.Source = "Pause.png";
                songProgress.Value = 0;
                songProgress.Maximum = 1;
                return;
            }
            songProgress.Maximum = s.Length.TotalSeconds;
            timer.Start();
            index = dc.Songs.IndexOf(s);
            dc.Max = s.Length;
            dc.CurrentSong = s.Name;
            ME.Source = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), s.Name);
            Pause.Source = "Pause.png";
            ME.Play();
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Song song1 = ((Xamarin.Forms.Button)sender).BindingContext as Song;
            dc.DeleteSongFromPlaylist(song1);
            dc.Songs.Remove(song1);
            Toast.MakeText(Android.App.Application.Context, $"Písnička {song1.Name} byla odebrána!", ToastLength.Short).Show();
        }

        private void ME_MediaEnded(object sender, EventArgs e)
        {
            if (index < dc.Songs.Count - 1)
            {
                index++;
                songsLv.SelectedItem = dc.Songs[index];
            }
            else
            {
                songsLv.SelectedItem = null;
            }
        }

        private void Next_Clicked(object sender, EventArgs e)
        {
            if (FirstSongSelected)
            {
                if (index < dc.Songs.Count - 1)
                {
                    index++;
                    songsLv.SelectedItem = dc.Songs[index];
                }
                else
                {
                    songsLv.SelectedItem = null;
                }
            }
        }

        private void Back_Clicked(object sender, EventArgs e)
        {
            if (FirstSongSelected)
            {
                if (index > -1)
                {
                    index--;
                    songsLv.SelectedItem = dc.Songs[index];
                }
                else
                {
                    songsLv.SelectedItem = null;
                }
            }
        }

        private void songProgress_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            dc.CurrTime = TimeSpan.FromSeconds(songProgress.Value);
        }

        private void songProgress_DragCompleted(object sender, EventArgs e)
        {
            ME.Position = TimeSpan.FromSeconds(songProgress.Value);
        }
    }

}