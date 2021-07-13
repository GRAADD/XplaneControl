using Xamarin.Forms;

namespace XplaneMaster.CommandPages
{
    public class CustomWebView : WebView
    {
        public static readonly BindableProperty UriProperty = BindableProperty.Create(propertyName: "Uri",
            returnType: typeof(string),
            declaringType: typeof(CustomWebView),
            defaultValue: default(string));

        public string Uri
        {
            get => GetValue(UriProperty) as string;
            set => SetValue(UriProperty, value);
        }
    }
}