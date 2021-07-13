using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XplaneMaster.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XplaneMap : ContentPage
    {
        public XplaneMap()
        {
            InitializeComponent();
        }

        private void DrawContent()
        {
            ContentPage contentPage = new ContentPage();

        }
    }
}