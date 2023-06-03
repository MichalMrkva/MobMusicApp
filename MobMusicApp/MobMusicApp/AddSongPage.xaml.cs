using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddSongPage : ContentPage
    {
        DataCollections dc = new DataCollections(2);
        public AddSongPage()
        {
            InitializeComponent();
            BindingContext = dc;
        }

        private void AddSong_Clicked(object sender, EventArgs e)
        {
            Song s1 = new Song("Ahoj333", TimeSpan.FromSeconds(93456), "FilePath");
            Song s2 = new Song("Ahoj154", TimeSpan.FromSeconds(55555), "FilePath");
            Song s3 = new Song("Ahoj512", TimeSpan.FromSeconds(176), "FilePath");
            Song s4 = new Song("Ahoj654", TimeSpan.FromSeconds(98625), "FilePath");
            dc.WriteSongsInFilesToXml(s1);
            dc.WriteSongsInFilesToXml(s2);
            dc.WriteSongsInFilesToXml(s3);
            dc.WriteSongsInFilesToXml(s4);
            Playlist p1 = new Playlist("Name1", 5);
            Playlist p2 = new Playlist("Name2", 85);
            Playlist p3 = new Playlist("Name3", 4);
            Playlist p4 = new Playlist("Name4", 1);
            dc.WritePlaylistToXml(p1);
            dc.WritePlaylistToXml(p2);
            dc.WritePlaylistToXml(p3);
            dc.WritePlaylistToXml(p4);
            dc.WriteSongsInToPlaylist("Name1", s1);
            dc.WriteSongsInToPlaylist("Name1", s2);
            dc.WriteSongsInToPlaylist("Name1", s3);
            dc.WriteSongsInToPlaylist("Name1", s4);
            dc.WriteSongsInToPlaylist("Name2", s1);
            dc.WriteSongsInToPlaylist("Name2", s2);
            dc.WriteSongsInToPlaylist("Name2", s3);
            dc.WriteSongsInToPlaylist("Name2", s4);
            dc.WriteSongsInToPlaylist("Name2", s1);
            dc.WriteSongsInToPlaylist("Name2", s2);
            dc.WriteSongsInToPlaylist("Name2", s3);
            dc.WriteSongsInToPlaylist("Name2", s4);
        }
    }
}