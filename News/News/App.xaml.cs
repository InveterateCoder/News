using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace News
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (!Properties.ContainsKey("Inf"))
                Properties.Add("Inf", NewsInfo.Serialize(new NewsInfo { Address = "https://news.yandex.ru/daily.rss", Description = "Важное за сутки" }));
            if(!Properties.ContainsKey("Rgn"))
                Properties.Add("Rgn", NewsInfo.Serialize(new NewsInfo { Address = "https://news.yandex.ru/Barnaul/index.rss", Description = "Алтайский край" }));
            MainPage = new NavigationPage(new MainPage()) {BarTextColor = Color.Black};
        }

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
