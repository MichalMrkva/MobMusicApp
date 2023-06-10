using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TagLib;
using Android.Widget;

namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSongPage : ContentPage
    {
        public static event Loader Load;
        public delegate void Loader(string PlaylistName);
        DataCollections dc = new DataCollections(2);
        Song song1;
        Song song2;
        public AddSongPage()
        {
            InitializeComponent();
            BindingContext = dc;
        }
        private async void AddSong_Clicked(object sender, EventArgs e)
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.Android, new[] { "audio/mpeg" } }
                    })
                });
                if (file != null)
                {
                    string name = Path.GetFileName(file.FullPath);
                    try
                    {
                        System.IO.File.Copy(file.FullPath, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)));
                    }
                    catch (Exception)
                    {
                        Toast.MakeText(Android.App.Application.Context, "Tato písnička byla již přidána!", ToastLength.Short).Show();
                        return;
                    }
                    song1 = new Song(
                        name,
                        NoMiliseconds(TagLib.File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), name)).Properties.Duration)                  
                        );
                    dc.SongsInFiles.Add(NewSong(song1));
                    dc.WriteSongsInFilesToXml(song1);
                    dc.ReadSongsInFilesFromXml();
                    Toast.MakeText(Android.App.Application.Context,$"Písnička {song1.Name} byla přidána!", ToastLength.Short).Show();
                    song1 = null;
                    file = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void songsLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            song2 = e.SelectedItem as Song;
            AddSongToPlaylist.IsVisible = true;
        }
        
        private void AddSongToPlaylist_Clicked(object sender, EventArgs e)
        {
            if (DataCollections.IsSongInPlaylist(song2.Name))
            {
                Toast.MakeText(Android.App.Application.Context, $"Písnička {song2.Name} byla již do toho to playlistu přidána!", ToastLength.Short).Show();
            }
            else
            {
                dc.WriteSongsInToPlaylist(DataCollections.currentPlaylist, song2);
                Load.Invoke(DataCollections.currentPlaylist);
                Navigation.PopAsync();
                Toast.MakeText(Android.App.Application.Context, $"Písnička {song2.Name} byla přidána!", ToastLength.Short).Show();
            }
        }
        private TimeSpan NoMiliseconds(TimeSpan time)
        {
            return new TimeSpan(time.Hours, time.Minutes, time.Seconds);
        }
        private Song NewSong(Song sng)
        {
            Song song = new Song(sng.Name, sng.Length);
            return song;
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {
            Song song3 = ((Xamarin.Forms.Button)sender).BindingContext as Song;
            dc.DeleteSongFromFiles(song3);
            dc.SongsInFiles.Remove(song3);
            song2 = null;
            AddSongToPlaylist.IsVisible = false;
            Toast.MakeText(Android.App.Application.Context, $"Písnička {song3.Name} byla odebrána!", ToastLength.Short).Show();
        }
    }
}