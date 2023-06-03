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
    public partial class AddSongPage : ContentPage
    {
        DataCollections dc = new DataCollections(2);
        public AddSongPage()
        {
            InitializeComponent();
            BindingContext = dc;
        }
    }
}