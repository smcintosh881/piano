using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace piano
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaElement beep;

        private LampHelper lampHelper;
        private bool lampFound = false;


        public MainPage()
        {
            this.InitializeComponent();
            beep = new MediaElement();
            this.setMedia();

            lampHelper = new LampHelper();
            lampHelper.LampFound += LampHelper_LampFound;
        
        }


        private async void setMedia()
        {
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("beep.wav");
            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            beep.SetSource(stream, file.ContentType);
        }


        private void LampHelper_LampFound(object sender, EventArgs e)
        {
            lampFound = true;
            GetLampState();
        }

        private async void GetLampState()
        {
            if (lampFound)
            {
                // Get the current On/Off state of the lamp.
                c_button.IsPressed.Equals(await lampHelper.GetOnOffAsync());
            }
        }
        
        //where the light is turned On
        private void button_C(object sender, RoutedEventArgs e)
        {
            if (lampFound)
            {
                //lampHelper.SetOnOffAsync(true);
                beep.Play();
                
                //turns lamp off
                lampHelper.SetOnOffAsync(false);
            } 
        }




        private void button_D(object sender, RoutedEventArgs e)
        {

        }

        private void button_E(object sender, RoutedEventArgs e)
        {

        }

        private void button_F(object sender, RoutedEventArgs e)
        {

        }

        private void button_G(object sender, RoutedEventArgs e)
        {

        }

        private void button_A(object sender, RoutedEventArgs e)
        {

        }

        private void button_B(object sender, RoutedEventArgs e)
        {

        }

    }
}
