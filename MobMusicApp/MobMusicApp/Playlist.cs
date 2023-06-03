using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace MobMusicApp
{
    class DataCollections : INotifyPropertyChanged
    {
        string filePathPlaylists = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Playlists.xml");
        string filePathSongsInFiles = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Songs.xml");
        private TimeSpan _Max;
        public TimeSpan Max { 
            get => _Max;
            set
            {
                _Max = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Max)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Time)));
            } 
        }
        public string Time { get => TimeSpan.Zero.ToString() + " - " + Max;}
        private string _CurrentSong;
        public string CurrentSong { 
            get => "Now Playing: " + _CurrentSong; 
            set 
            {
                _CurrentSong = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentSong)));
            }
        }
        private ObservableCollection<Song> _Songs;
        public ObservableCollection<Song> Songs
        {
            get { return _Songs; }
            set
            {
                _Songs = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Songs)));
            }
        }
        private ObservableCollection<Song> _SongsInFiles;

        public ObservableCollection<Song> SongsInFiles
        {
            get { return _SongsInFiles; }
            set
            {
                _SongsInFiles = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SongsInFiles)));
            }
        }
        private ObservableCollection<Playlist> _Playlists;

        public ObservableCollection<Playlist> Playlists
        {
            get { return _Playlists; }
            set
            {
                _Playlists = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Playlists)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        public DataCollections(int choice)
        {
            if (choice == 1)
            {
                
            }
            if (choice == 2)
            {
                SongsInFiles = ReadSongsInFilesFromXml();
            }
            else
            {
                Playlists = ReadPlaylistFromXml();
            }
        }
        public ObservableCollection<Playlist> ReadPlaylistFromXml()
        {
            if (!File.Exists(filePathPlaylists))
            {
                XDocument doc1 = new XDocument(
                new XElement("Playlists"));
                doc1.Save(filePathPlaylists);
            }
            var list = new ObservableCollection<Playlist>();
            XDocument doc = XDocument.Load(filePathPlaylists);
            foreach (var elm in doc.Root.Elements())
            {
                Playlist p = new Playlist(
                    elm.Attribute("Name").Value,
                    Int32.Parse(elm.Attribute("Size").Value)
                    );
                list.Add(p);
            }
            return list;
        }
        public void WritePlaylistToXml(Playlist play)
        {
            XDocument doc = XDocument.Load(filePathPlaylists);
            XElement playlist = new XElement("Playlist");
            playlist.SetAttributeValue("Name", play.Name);
            playlist.SetAttributeValue("Size", play.Size);
            doc.Root.Add(playlist);
            doc.Save(filePathPlaylists);
        }
        public ObservableCollection<Song> ReadSongsInPlaylistFromXml(string playlistName)
        {

            var list = new ObservableCollection<Song>();
            XmlDocument doc = new XmlDocument();
            doc.Load(filePathPlaylists);
            XmlNode playlistElement = doc.SelectSingleNode($"//Playlist[@Name='{playlistName}']");
            XmlNodeList songElements = playlistElement.SelectNodes(".//Song");
            foreach (XmlNode elm in songElements)
            {
                Song song = new Song(
                elm.Attributes["Name"].Value,
                TimeSpan.Parse(elm.Attributes["Length"].Value),
                elm.Attributes["FilePath"].Value
                );
                list.Add(song);
            }
            return list;
        }
        public void WriteSongsInToPlaylist(string playlistName, Song s)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePathPlaylists);
            XmlNode playlistElement = doc.SelectSingleNode($"//Playlist[@Name='{playlistName}']");
            XmlNode song = doc.CreateElement("Song");
            song.Attributes.Append(CreateAttribute(doc, "Name", s.Name));
            song.Attributes.Append(CreateAttribute(doc, "Length", s.Length.ToString()));
            song.Attributes.Append(CreateAttribute(doc, "FilePath", s.FilePath));
            playlistElement.AppendChild(song);
            doc.Save(filePathPlaylists);
        }
        public void WriteSongsInFilesToXml(Song s)
        {
            if (!File.Exists(filePathSongsInFiles))
            {
                XDocument doc1 = new XDocument(
                new XElement("Songs"));
                doc1.Save(filePathSongsInFiles);
            }
            XDocument doc123 = XDocument.Load(filePathSongsInFiles);
            XElement sng = new XElement("Song");
            sng.SetAttributeValue("Name", s.Name);
            sng.SetAttributeValue("Length", s.Length.ToString());
            sng.SetAttributeValue("FilePath", s.FilePath);
            doc123.Root.Add(sng);
            doc123.Save(filePathSongsInFiles);
        }
        public ObservableCollection<Song> ReadSongsInFilesFromXml()
        {
            if (!File.Exists(filePathSongsInFiles))
            {
                XDocument doc1 = new XDocument(
                new XElement("Songs"));
                doc1.Save(filePathSongsInFiles);
            }
            var list = new ObservableCollection<Song>();
            XDocument doc = XDocument.Load(filePathSongsInFiles);
            foreach (var elm in doc.Root.Elements())
            {
                Song s = new Song(
                elm.Attribute("Name").Value,
                TimeSpan.Parse(elm.Attribute("Length").Value),
                elm.Attribute("FilePath").Value
                );
                list.Add(s);
            }
            return list;
        }
        static XmlAttribute CreateAttribute(XmlDocument doc, string name, string value)
        {
            XmlAttribute attribute = doc.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }
    }
    class Playlist
    {
        public string Name { get; set; }
        public int Size { get; set; }
        public Playlist(string name, int size)
        {
            Name = name;
            Size = size;
        }
        public Playlist()
        {

        }
    }
    class Song
    {
        public string Name { get; set; }
        public TimeSpan Length { get; set; }
        public string FilePath { get; set; }
        public Song(string name, TimeSpan length, string filePath)
        {
            Name = name;
            Length = length;
            FilePath = filePath;
        }
    }
}
