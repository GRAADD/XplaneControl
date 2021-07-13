using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XplaneMaster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XplaneCommand : TabbedPage
    {
        public XplaneCommand()
        {
            InitializeComponent();
        }
    }
}