using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobMusicApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistsPage : ContentPage
    {
        public static event Loader Load;
        public delegate void Loader(string PlaylistName);
        public static event Hider Hide;
        public delegate void Hider();
        public bool CreatingPlaylist = false;
        DataCollections dc = new DataCollections(3);
        public PlaylistsPage()
        {
            InitializeComponent();
            PlaylistNameEntry.Text = "";
            BindingContext = dc;
            
        }
        private void Addplaylist_Clicked(object sender, EventArgs e)
        {
            
            if (CreatingPlaylist && PlaylistNameEntry.Text != "")
            {
                Playlist p = new Playlist(
                    PlaylistNameEntry.Text
                );
                if (dc.Playlists.Any(play => play.Name ==  p.Name))
                {
                    Toast.MakeText(Android.App.Application.Context, $"Playlist s tímto jménem byl již vytvořen!", ToastLength.Short).Show();
                    PlaylistNameEntry.Text = "";
                }
                else
                {
                    dc.Playlists.Add(p);
                    dc.WritePlaylistToXml(p);
                    Toast.MakeText(Android.App.Application.Context, $"Playlist {p.Name} byl přidán!", ToastLength.Short).Show();
                    PlaylistNameEntry.Text = "";
                    Addplaylist.Text = "Add playlist";
                    PlaylistNameEntry.IsVisible = !CreatingPlaylist;
                }
            }
            if (CreatingPlaylist && PlaylistNameEntry.Text == "")
            {
                Addplaylist.Text = "Add playlist";
                PlaylistNameEntry.IsVisible = !CreatingPlaylist;
            }
            else
            {
                Addplaylist.Text = "Save";
                PlaylistNameEntry.IsVisible = !CreatingPlaylist;
            }
            CreatingPlaylist = !CreatingPlaylist;

        }

        private void playlistsLv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Playlist p = e.SelectedItem as Playlist;
            DataCollections.currentPlaylist = p.Name;
            Load.Invoke(p.Name);
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            Playlist p = ((Xamarin.Forms.Button)sender).BindingContext as Playlist;
            dc.DeletePlaylistFromXml(p);
            dc.Playlists.Remove(p);
            Toast.MakeText(Android.App.Application.Context, $"Playlist {p.Name} byl odebrán!", ToastLength.Short).Show();
            if (DataCollections.currentPlaylist == p.Name)
            {
                Hide.Invoke();
            }
        }
    }
}