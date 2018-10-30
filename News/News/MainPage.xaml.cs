using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace News
{
    public partial class MainPage : ContentPage
    {
        private NewsInfo toolBarItemSelected { get; set; }
        private Web web = new Web();
        internal NewsViewModel viewModel = new NewsViewModel();
        private Region rgn = new Region();
        private bool state = false;
        private TapGestureRecognizer tapGesture;
        private bool exit = false;
        private ToolbarItem[] toolbarItems;
        public MainPage()
        {
            InitializeComponent();
            toolbarItems = new ToolbarItem[ToolbarItems.Count];
            ToolbarItems.CopyTo(toolbarItems, 0);
            tapGesture = new TapGestureRecognizer();
            tapGesture.Command = new Command(async() => await Navigation.PushModalAsync(rgn));
            listView.BindingContext = viewModel;
            NewsInfo info = NewsInfo.Deserialize((string)Application.Current.Properties["Inf"]);
            if (info == rgn.SelectedInfo)
                RegionProc(true);
            toolBarItemSelected = info;
            viewModel.LoadAsync(info).ContinueWith((b) =>
            {
                if (b.Result)
                    return;
                else
                    Exit();
            });
        }
        internal void Exit()
        {
            ToolbarItems.Clear();
            Content = new Label
            {
                Text = "Ошибка приложения.\n" +
                "Проверьте интернет соединение и перезагрузите приложение",
                HorizontalTextAlignment = TextAlignment.Center, VerticalOptions = LayoutOptions.Center
            };
        }
        private async void OnToolbarItemClicked(object sender, EventArgs e)
        {
            NewsInfo info;
            NewsToolbarItem tItem = (NewsToolbarItem)sender;
            if(tItem.Description=="rgn")
            {
                info = rgn.SelectedInfo;
                RegionProc(true);
            }
            else
            {
                info = new NewsInfo { Address = tItem.Address, Description = tItem.Description };
                RegionProc(false);
            }
            if (toolBarItemSelected == info)
                return;
            toolBarItemSelected = info;
            if(!await viewModel.LoadAsync(info))
            {
                Exit();
                return;
            }
            Application.Current.Properties["Inf"] = NewsInfo.Serialize(info);
            await Application.Current.SavePropertiesAsync();
            listView.ScrollTo(((RSSItem[])listView.ItemsSource).First(), ScrollToPosition.End, true);
        }

        private void RegionProc(bool isRgn)
        {
            if (isRgn == state)
                return;
            if(isRgn)
            {
                exit = false;
                label.BackgroundColor = Color.LightGray;
                label.TextColor = Color.Black;
                label.GestureRecognizers.Add(tapGesture);
                state = true;
                Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
                {
                    if (exit)
                        return false;
                    Animation();
                    return false;
                });
                Device.StartTimer(TimeSpan.FromMilliseconds(2000), () =>
                {
                    if (exit)
                        return false;
                    Animation();
                    return false;
                });
            }
            else
            {
                exit = true;
                label.BackgroundColor = Color.Black;
                label.TextColor = Color.White;
                label.GestureRecognizers.Clear();
                state = false;
            }
        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            RSSItem item = (RSSItem)e.Item;
            web.Navigate = item.Link;
            web.Title = item.Title;
            Navigation.PushAsync(web);
            listView.SelectedItem = null;
        }

        private async void Animation()
        {
            await label.ScaleTo(1.05, 100, Easing.SinOut);
            await label.ScaleTo(1, 100, Easing.SinIn);
            await label.ScaleTo(1.05, 100, Easing.SinOut);
            await label.ScaleTo(1, 100, Easing.SinIn);
        }
    }
}
