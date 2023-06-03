using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace MobMusicApp
{
    class DataCollections : INotifyPropertyChanged
    {
        string filePathPlaylists = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Playlists.xml");
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
                Songs = ReadSongsInPlaylistFromXml("asdf");
            }
            if (choice == 2)
            {
                SongsInFiles = new ObservableCollection<Song>();
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
            
            //if (!File.Exists(filePathPlaylists))
            //{
            //    XDocument doc = new XDocument(
            //    new XElement("Playlists",
            //        new XElement("Playlist",
            //            new XAttribute("Name", play.Name),
            //            new XAttribute("Size", play.Size)
            //        )
            //    ));
            //    doc.Save(filePathPlaylists);
            //}
            //else
            //{
                XDocument doc = XDocument.Load(filePathPlaylists);
                XElement playlist = new XElement("Playlist");
                playlist.SetAttributeValue("Name", play.Name);
                playlist.SetAttributeValue("Size", play.Size);
                doc.Root.Add(playlist);
                doc.Save(filePathPlaylists);
            //}
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
        public Song(string name, TimeSpan length , string filePath)
        {
            Name = name;
            Length = length;
            FilePath = filePath;
        }
    }
}
