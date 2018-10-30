using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Windows.Input;
using Xamarin.Forms;
using System.Net;
using System.Threading.Tasks;

namespace News
{
    class NewsViewModel : ViewModelBase
    {
        private DateTime timeStamp;
        private RSSItem[] items;
        private string title;
        private string address;
        private bool isRefreshing = false;
        public bool IsRefreshing
        {
            set => SetProperty(ref isRefreshing, value);
            get => isRefreshing;
        }
        public string Title
        {
            set => SetProperty(ref title, value);
            get => title;
        }
        private XElement Channel { get; set; }
        public DateTime TimeStamp
        {
            private set => SetProperty(ref timeStamp, value);
            get => timeStamp;
        }
        public async Task<bool> LoadAsync(NewsInfo info)
        {
            Title = info.Description;
            address = info.Address;
            IsRefreshing = true;
            return await Refresh();
        }
        public RSSItem[] Items
        {
            private set => SetProperty(ref items, value);
            get => items;
        }
        public NewsViewModel()
        {
            RefreshCommand = new Command(execute: async() => 
            {
                if (!await Refresh(true))
                    Device.BeginInvokeOnMainThread(() => ((MainPage)((NavigationPage)Application.Current.MainPage).RootPage).Exit());
            }, canExecute: () => !IsRefreshing);
        }
        public ICommand RefreshCommand { private set; get; }
        private async Task<bool> Refresh(bool refCommand = false)
        {
            WebRequest request = WebRequest.Create(address);
            XDocument doc = null;
            try
            {
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        doc = XDocument.Parse(reader.ReadToEnd());
                }
            }
            catch(Exception)
            {
                IsRefreshing = false;
                return false;
            }
            Channel = doc.Element(XName.Get("rss")).Element(XName.Get("channel"));
            XElement lastBuildDate = Channel.Element(XName.Get("lastBuildDate"));
            DateTimeOffset timeOffset = DateTimeOffset.Parse(lastBuildDate.Value);
            DateTime timeStamp = timeOffset.DateTime + timeOffset.Offset;
            if(!refCommand || timeStamp!=TimeStamp)
            {
                if (Channel == null)
                    throw new NullReferenceException();
                Items = Channel.Elements(XName.Get("item")).Select((XElement element) =>
                {
                    return new RSSItem(element);
                }).ToArray();
            }
            TimeStamp = timeStamp;
            IsRefreshing = false;
            return true;
        }
    }
}
