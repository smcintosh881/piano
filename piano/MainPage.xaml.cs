using org.allseen.LSF.LampState;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace piano
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaElement A, B, B_b, C, C_s, D, E, E_b, F, F_s, G, G_s;
        uint[] colors;
        AllJoynBusAttachment busAttachment = null;
        String lampId = "d3cc1d629dda8d3df81fe0129bc2bf56";
        LampStateConsumer consumer = null;

        public MainPage()
        {
            this.InitializeComponent();

            this.setAllMedia();

            colors = new uint[] { 0, 34, 51, 101, 153, 167, 204 };
            for (int i = 0; i < 7; i++)
            {
                colors[i] = (uint)colors[i] * 14035841;
            }

            busAttachment = new AllJoynBusAttachment();
            LampStateWatcher watcher = new LampStateWatcher(busAttachment);
            watcher.Added += Watcher_Added; 

            watcher.Start();
        }

        private async void setAllMedia()
        {
            A = new MediaElement();
            B = new MediaElement();
            B_b = new MediaElement();
            C = new MediaElement();
            C_s = new MediaElement();
            D = new MediaElement();
            E = new MediaElement();
            E_b = new MediaElement();
            F = new MediaElement();
            F_s = new MediaElement();
            G = new MediaElement();
            G_s = new MediaElement();
            Windows.Storage.StorageFolder folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            Windows.Storage.StorageFile file = await folder.GetFileAsync("A.wav");
            A.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("B.wav");
            B.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("B_b.wav");
            B_b.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("C.wav");
            C.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("C_s.wav");
            C_s.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("D.wav");
            D.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("E.wav");
            E.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("E_b.wav");
            E_b.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("F.wav");
            F.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("F_s.wav");
            F_s.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("G.wav");
            G.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("G_s.wav");
            G_s.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
        }

        private async void Watcher_Added(LampStateWatcher sender, AllJoynServiceInfo args) // can only wait on async method inside async method
        {
            AllJoynAboutDataView aboutData = await AllJoynAboutDataView.GetDataBySessionPortAsync(args.UniqueName, busAttachment, args.SessionPort);
            if (aboutData != null && !string.IsNullOrWhiteSpace(aboutData.DeviceId) && string.Equals(aboutData.DeviceId, lampId))
            {
                LampStateJoinSessionResult result = await LampStateConsumer.JoinSessionAsync(args, sender);
                if (result.Status == AllJoynStatus.Ok)
                {
                    consumer = result.Consumer;
                }
            }
        }

        private async void turn_off()
        {
            if (consumer != null)
                await consumer.SetOnOffAsync(false);
        }

        private async void c_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[0]);
                if (!c_button.IsPressed)
                {
                    turn_off();
                }
            }
            C.Play();
        }

        private async void d_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[1]);
                if (!d_button.IsPressed)
                {
                    turn_off();
                }
            }
            D.Play();
        }

        private async void e_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[2]);
                if (!e_button.IsPressed)
                {
                    turn_off();
                }
            }
            E.Play();
        }

        private async void f_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[3]);
                if (!f_button.IsPressed)
                {
                    turn_off();
                }
            }
            F.Play();
        }

        private async void g_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[4]);
                if (!g_button.IsPressed)
                {
                    turn_off();
                }
            }
            G.Play();
        }

        private async void a_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[5]);
                if (!a_button.IsPressed)
                {
                    turn_off();
                }
            }
            A.Play();
        }

        private async void b_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[6]);
                if (!b_button.IsPressed)
                {
                    turn_off();
                }
            }
            B.Play();
        }
    }
}
