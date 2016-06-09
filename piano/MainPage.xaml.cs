using org.allseen.LSF.LampState;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.AllJoyn;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace piano
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        MediaElement A, B, B_b, C, C_s, D, E, E_b, F, F_s, G, G_s, C_h, C_h_s;
        uint[] colors;
        AllJoynBusAttachment busAttachment = null;
        String lampId = "76882ed0b5209e35aa15ffba7a8de783";

        LampStateConsumer consumer = null;
        uint defaultSaturation = uint.MaxValue;
        uint defaultBrightness = 2028045600;


        public MainPage()
        {
            this.InitializeComponent();

            this.setAllMedia();

            colors = new uint[] { 0, 34, 51, 101, 153, 175, 220, 25, 47, 135, 158, 186};
            for (int i = 0; i < 12; i++)
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
            C_h = new MediaElement();
            C_h_s = new MediaElement();
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
            file = await folder.GetFileAsync("C_h.wav");
            C_h.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
            file = await folder.GetFileAsync("C_h_s.wav");
            C_h_s.SetSource(await file.OpenAsync(Windows.Storage.FileAccessMode.Read), file.ContentType);
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

        /*
         * Sets the background of a button to a given color
         * @param btn a button
         * @param opac the opacity setting  
         * @param r the red hue number
         * @param g green hue number
         * @param b blue hue number
         * returns the new button background color
         */
        private void set_color(RepeatButton btn, byte opac, byte r, byte g, byte b){
            var colored_key = new SolidColorBrush(Color.FromArgb(opac, r, g, b));
            btn.Background = colored_key;
        }

        /*
         * Returns the background of a button back to white
         * @param btn a button
         * @param opac the opacity setting
         * @param r red hue number
         * @param g green hue number
         * @param b blue hue number
         * returns the background color of a button to white - the original state
         * */
        private void restore_color(RepeatButton btn, byte opac, byte r, byte g, byte b)
        {
            btn.Background = new SolidColorBrush(Color.FromArgb(opac, r, g, b));
        }

        /*
         * Click event that plays appropriate piano tone 
         * and changes the button and light color
         * @param sender 
         * @param e
         */

        private async void c_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(c_button, 120, 255, 0, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[0]);

                if (!c_button.IsPressed)
                {
                    restore_color(c_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            C.Play();
        }

        private async void c_s_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(c_s_button, 255, 255, 64, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[7]);

                if (!c_s_button.IsPressed)
                {
                    restore_color(c_s_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            C_s.Play();
        }

        private async void d_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(d_button, 120, 255, 128, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[1]);

                if (!d_button.IsPressed)
                {
                    restore_color(d_button, 120, 255, 255, 255);
                    turn_off();  
                }
            }
            D.Play();
        }

        private async void e_b_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(e_b_button, 255, 255, 192, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[8]);

                if (!e_b_button.IsPressed)
                {
                    restore_color(e_b_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            E_b.Play();
        }

        private async void e_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(e_button, 120, 255, 255, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[2]);

                if (!e_button.IsPressed)
                {
                    restore_color(e_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            E.Play();
        }

        private async void f_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(f_button, 120, 0, 255, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[3]);

                if (!f_button.IsPressed)
                {
                    restore_color(f_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            F.Play();
        }

        private async void f_s_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(f_s_button, 255, 0, 255, 128);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[9]);

                if (!f_s_button.IsPressed)
                {
                    restore_color(f_s_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            F_s.Play();
        }

        private async void g_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(g_button, 120, 0, 255, 255);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[4]);

                if (!g_button.IsPressed)
                {
                    restore_color(g_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            G.Play();
        }

        private async void g_s_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(g_s_button, 255, 0, 128, 255);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[10]);

                if (!g_s_button.IsPressed)
                {
                    restore_color(g_s_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            G_s.Play();
        }

        private async void a_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(a_button, 120, 0, 0, 255);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[5]);

                if (!a_button.IsPressed)
                {
                    restore_color(a_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            A.Play();
        }

        private async void b_b_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(b_b_button, 255, 51, 0, 255);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[11]);

                if (!b_b_button.IsPressed)
                {
                    restore_color(b_b_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            B_b.Play();
        }

        private async void b_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(b_button, 120, 102, 0, 255);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[6]);

                if (!b_button.IsPressed)
                {
                    restore_color(b_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            B.Play();
        }

        private async void c_h_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(c_h_button, 120, 255, 0, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[0]);

                if (!c_h_button.IsPressed)
                {
                    restore_color(c_h_button, 120, 255, 255, 255);
                    turn_off();
                }
            }
            C_h.Play();
        }
        
        private async void c_h_s_button_click(object sender, RoutedEventArgs e)
        {
            if (consumer != null)
            {
                set_color(c_h_s_button, 255, 255, 64, 0);
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[7]);

                if (!c_h_s_button.IsPressed)
                {
                    restore_color(c_h_s_button, 255, 0, 0, 0);
                    turn_off();
                }
            }
            C_h_s.Play();
        }
    }
}
