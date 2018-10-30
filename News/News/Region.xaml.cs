using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace News
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Region : ContentPage
	{
        public NewsInfo SelectedInfo { set; get; }
		public Region ()
		{
			InitializeComponent ();
            SelectedInfo = NewsInfo.Deserialize((string)Application.Current.Properties["Rgn"]);
		}

        private void Cancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void RgnItemTaped(object sender, ItemTappedEventArgs e)
        {
            ListView list = (ListView)sender;
            SelectedInfo = (NewsInfo)list.SelectedItem;
            list.SelectedItem = null;
            ((MainPage)((NavigationPage)Application.Current.MainPage).RootPage).viewModel.LoadAsync(SelectedInfo);
            Application.Current.Properties["Inf"] = Application.Current.Properties["Rgn"] = NewsInfo.Serialize(SelectedInfo);
            await Application.Current.SavePropertiesAsync();
            await Navigation.PopModalAsync();
        }
    }
}