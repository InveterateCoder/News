using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace News
{
    public class Web : ContentPage
    {
        private WebView webView;
        public string Navigate
        {
            set => webView.Source = value;
            get => webView.Source.ToString();
        }
        public Web()
        {
            webView = new WebView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Button bckButton = new Button
            {
                VerticalOptions = LayoutOptions.End,
                HorizontalOptions = LayoutOptions.Center,
                Text = "Назад",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Button)),
                TextColor = Color.Blue,
                BackgroundColor = Color.White,
                CornerRadius = 15,
                BorderWidth = 0
            };
            bckButton.BindingContext = webView;
            bckButton.SetBinding(Button.IsEnabledProperty, "CanGoBack");
            bckButton.SetBinding(Button.IsVisibleProperty, "CanGoBack");
            bckButton.Clicked += (sender, args) => webView.GoBack();
            Grid grid = new Grid
            {
                Children =
                {
                    webView, bckButton
                }
            };
            Content = grid;
        }
    }
}