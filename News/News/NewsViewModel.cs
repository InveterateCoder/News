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
                if (!await Refresh())
                    Device.BeginInvokeOnMainThread(() => ((MainPage)((NavigationPage)Application.Current.MainPage).RootPage).Exit());
            }, canExecute: () => !IsRefreshing);
        }
        public ICommand RefreshCommand { private set; get; }
        private async Task<bool> Refresh()
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
            if (Channel == null)
                throw new NullReferenceException();
            Items = Channel.Elements(XName.Get("item")).Select((XElement element) =>
            {
                return new RSSItem(element);
            }).ToArray();
            IsRefreshing = false;
            return true;
        }
    }
}
