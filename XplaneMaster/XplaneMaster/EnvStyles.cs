using Xamarin.Forms;

namespace XplaneMaster
{
    static class EnvStyles
    {
        //public static ButtonStyle ButtonStyle;
        public static Color ButtonBackgroundColor = Color.FromHex("#303f9f");
        public static Color ButtonBorderColor;
        public static Color ButtonTextColor = Color.Black;
        public static int ButtonTextSize;

        public static Color LabelBackgroundColor;
        public static Color LabelBorderColor;
        public static Color LabelTextColor;
        public static int LabelTextSize;

        public static Color EntryBackgroundColor;
        public static Color EntryBorderColor;
        public static Color EntryTextColor;
        public static int EntryTextSize;

        public static Color BackgroundColor;


        public static void SetLight()
        {
            ButtonBackgroundColor = Color.FromHex("#4fc3f7");
            ButtonBorderColor = Color.FromHex("#0093c4");
            ButtonTextColor = Color.Black;

            LabelTextColor = Color.Black;

            EntryBackgroundColor = Color.FromHex("#8bf6ff");
            EntryTextColor = Color.Black;

            EntryBorderColor = Color.FromHex("#0093c4");

            BackgroundColor = Color.WhiteSmoke;

            
        }
    }
}