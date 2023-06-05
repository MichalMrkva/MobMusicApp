using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using NAudio.Wave;

namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSongPage : ContentPage
    {
        public static event Loader Load;
        public delegate void Loader(string PlaylistName);
        DataCollections dc = new DataCollections(2);
        Song song;
        public AddSongPage()
        {
            InitializeComponent();
            BindingContext = dc;
        }
        private async void AddSong_Clicked(object sender, EventArgs e)
        {
            //Song s1 = new Song("Ahoj333", TimeSpan.FromSeconds(93456), "FilePath");
            //Song s2 = new Song("Ahoj154", TimeSpan.FromSeconds(55555), "FilePath");
            //Song s3 = new Song("Ahoj512", TimeSpan.FromSeconds(176), "FilePath");
            //Song s4 = new Song("Ahoj654", TimeSpan.FromSeconds(98625), "FilePath");
            //dc.WriteSongsInFilesToXml(s1);
            //dc.WriteSongsInFilesToXml(s2);
            //dc.WriteSongsInFilesToXml(s3);
            //dc.WriteSongsInFilesToXml(s4);
            //Playlist p1 = new Playlist("Name1", 5);
            //Playlist p2 = new Playlist("Name2", 85);
            //Playlist p3 = new Playlist("Name3", 4);
            //Playlist p4 = new Playlist("Name4", 1);
            //dc.WritePlaylistToXml(p1);
            //dc.WritePlaylistToXml(p2);
            //dc.WritePlaylistToXml(p3);
            //dc.WritePlaylistToXml(p4);
            //dc.WriteSongsInToPlaylist("Name1", s1);
            //dc.WriteSongsInToPlaylist("Name1", s2);
            //dc.WriteSongsInToPlaylist("Name1", s3);
            //dc.WriteSongsInToPlaylist("Name1", s4);
            //dc.WriteSongsInToPlaylist("Name2", s1);
            //dc.WriteSongsInToPlaylist("Name2", s2);
            //dc.WriteSongsInToPlaylist("Name2", s3);
            //dc.WriteSongsInToPlaylist("Name2", s4);
            //dc.WriteSongsInToPlaylist("Name2", s1);
            //dc.WriteSongsInToPlaylist("Name2", s2);
            //dc.WriteSongsInToPlaylist("Name2", s3);
            //dc.WriteSongsInToPlaylist("Name2", s4);
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
                    SongNameEntry.IsVisible = true;//přidat nový button, který se zobrazí po vybrání
                    string filePath = file.FullPath;
                    song = new Song(
                        "",
                        GetMp3Duration(filePath),//edit přesun souboru do aplikace 
                        filePath
                        );
                    dc.WriteSongsInFilesToXml(song);//edit dat to dofunkce aplikace
                    dc.ReadSongsInFilesFromXml();
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
            song = e.SelectedItem as Song;
            AddSongToPlaylist.IsVisible = true;
            AddSong.IsVisible = false;
            SongNameEntry.IsVisible = false;
        }
        public TimeSpan GetMp3Duration(string filePath)
        {
            //using (var mp3FileReader = new Mp3FileReader(filePath))
            //{
            //    var duration = mp3FileReader.TotalTime;
            //    return duration;
            //}
            return TimeSpan.FromSeconds(123456);
        }
        private void AddSongToPlaylist_Clicked(object sender, EventArgs e)
        {
            dc.WriteSongsInToPlaylist(DataCollections.currentPlaylist, song);//test zdali to funguje
            Load.Invoke(DataCollections.currentPlaylist);
            Navigation.PopAsync();
        }
        private void Delete_Clicked(object sender, EventArgs e)
        {

        }
    }
}