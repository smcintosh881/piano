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
using System.Threading.Tasks;

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
        Dictionary<String, LampStateConsumer> consumers = new Dictionary<string, LampStateConsumer>();
        String light_c_1 = "3eb3073dcc98ec532af81ea929aa9462";
        String light_c_2 = "b6afb5592fb6fcb7ab5d589c161168f4";
        String light_d_1 = "9b9e31a71b2b62d78d14fcad1f451941";
        String light_d_2 = "b6497b602aebd73b4eb72eb628fcb06d";
        String light_e_1 = "76882ed0b5209e35aa15ffba7a8de783";
        String light_e_2 = "d3cc1d629dda8d3df81fe0129bc2bf56";
        String light_f_1 = "49ae9deae8c0e64011be3047b78183ac";
        String light_f_2 = "9830c3a7bc62d771b20899ace42611db";
        String light_g_1 = "ac1d99ede0ab11be80386048df3e06bc";
        String light_g_2 = "356be4dd88ba0a43074d1cf9d2f3c646";
        String light_a_1 = "5cd65cc2e054d3206c1e8c16b2d76d29";
        String light_a_2 = "ee8455d57c16121d2b9d684582a87064";
        String light_b_1 = "c6d70a563310dad2764d13513eabd689";
        String light_b_2 = "db78dbbc949e2061dc33f9ab3c831e5b";
        String light_ch_1 = "f653aa62825ea3500c4fde186d1eab0f";
        String light_ch_2 = "7bf07d016da620730d7b8de03d71e246";
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

            consumers.Add(light_c_1, null);
            consumers.Add(light_c_2, null);
            consumers.Add(light_d_1, null);
            consumers.Add(light_d_2, null);
            consumers.Add(light_e_1, null);
            consumers.Add(light_e_2, null);
            consumers.Add(light_f_1, null);
            consumers.Add(light_f_2, null);
            consumers.Add(light_g_1, null);
            consumers.Add(light_g_2, null);
            consumers.Add(light_a_1, null);
            consumers.Add(light_a_2, null);
            consumers.Add(light_b_1, null);
            consumers.Add(light_b_2, null);
            consumers.Add(light_ch_1, null);
            consumers.Add(light_ch_2, null);

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
        
        private async void Watcher_Added(LampStateWatcher sender, AllJoynServiceInfo args) 
        {
            AllJoynAboutDataView aboutData = await AllJoynAboutDataView.GetDataBySessionPortAsync(args.UniqueName, busAttachment, args.SessionPort);
            if (aboutData != null && !string.IsNullOrWhiteSpace(aboutData.DeviceId) && consumers.ContainsKey(aboutData.DeviceId)) //string.Equals(aboutData.DeviceId, lampId))
            {
                LampStateJoinSessionResult result = await LampStateConsumer.JoinSessionAsync(args, sender);
                if (result.Status == AllJoynStatus.Ok && consumers[aboutData.DeviceId] == null)
                {
                    //consumer = result.Consumer;
                    consumers[aboutData.DeviceId] = result.Consumer;
                }
            }
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

        private async void key_press_light_helper(LampStateConsumer consumer, LampStateConsumer consumer2, RepeatButton button, int color)
        {
            if (consumer != null && consumer2 != null)
            {
                await consumer.SetSaturationAsync(defaultSaturation);
                await consumer2.SetSaturationAsync(defaultSaturation);
                await consumer.SetBrightnessAsync(defaultBrightness);
                await consumer2.SetBrightnessAsync(defaultBrightness);
                await consumer.SetOnOffAsync(true);
                await consumer2.SetOnOffAsync(true);
                await consumer.SetHueAsync(colors[color]);
                await consumer2.SetHueAsync(colors[color]);
                if (!button.IsPressed)
                {
                    await consumer.SetOnOffAsync(false);
                    await consumer2.SetOnOffAsync(false);
                }
            }
        }

        private void key_press_button_helper(RepeatButton button, byte opac, byte r, byte g, byte b, Boolean whiteKey)
        {
            set_color(button, opac, r, g, b);
            if (!button.IsPressed)
            {
                if (whiteKey)
                    restore_color(button, 255, 255, 255, 255);
                else
                    restore_color(button, 255, 0, 0, 0);
            }
        }

        private void c_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_c_1], consumers[light_c_2], c_button, 0);
            key_press_button_helper(c_button, 120, 255, 0, 0, true);
            C.Play();
        }

        private void c_s_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_c_1], consumers[light_c_2], c_s_button, 7);
            key_press_light_helper(consumers[light_d_1], consumers[light_d_2], c_s_button, 7);
            key_press_button_helper(c_s_button, 255, 255, 64, 0, false);
            C_s.Play();
        }
        
        private void d_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_d_1], consumers[light_d_2], d_button, 1);
            key_press_button_helper(d_button, 120, 255, 128, 0, true);
            D.Play();
        }

        private void e_b_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_d_1], consumers[light_d_2], e_b_button, 8);
            key_press_light_helper(consumers[light_e_1], consumers[light_e_2], e_b_button, 8);
            key_press_button_helper(e_b_button, 255, 255, 192, 0, false);
            E_b.Play();
        }

        private void e_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_e_1], consumers[light_e_2], e_button, 2);
            key_press_button_helper(e_button, 120, 255, 255, 0, true);
            E.Play();
        }

        private void f_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_f_1], consumers[light_f_2], f_button, 3);
            key_press_button_helper(f_button, 120, 0, 255, 0, true);
            F.Play();
        }

        private void f_s_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_f_1], consumers[light_f_2], f_s_button, 9);
            key_press_light_helper(consumers[light_g_1], consumers[light_g_2], f_s_button, 9);
            key_press_button_helper(f_s_button, 255, 0, 255, 128, false);
            F_s.Play();
        }

        private void g_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_g_1], consumers[light_g_2], g_button, 4);
            key_press_button_helper(g_button, 120, 0, 255, 255, true);
            G.Play();
        }

        private void g_s_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_g_1], consumers[light_g_2], g_s_button, 9);
            key_press_light_helper(consumers[light_a_1], consumers[light_a_2], g_s_button, 9);
            key_press_button_helper(g_s_button, 255, 0, 128, 255, false);
            G_s.Play();
        }

        private void a_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_a_1], consumers[light_a_2], a_button, 5);
            key_press_button_helper(a_button, 120, 0, 0, 255, true);
            A.Play();
        }

        private void b_b_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_a_1], consumers[light_a_2], b_b_button, 10);
            key_press_light_helper(consumers[light_b_1], consumers[light_b_2], b_b_button, 10);
            key_press_button_helper(b_b_button, 255, 51, 0, 255, false);
            B_b.Play();
        }

        private void b_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_b_1], consumers[light_b_2], b_button, 6);
            key_press_button_helper(b_button, 120, 102, 0, 255, true);
            B.Play();
        }

        private void c_h_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_ch_1], consumers[light_ch_2], c_h_button, 0);
            key_press_button_helper(c_h_button, 120, 255, 0, 0, true);
            C_h.Play();
        }
        
        private void c_h_s_button_click(object sender, RoutedEventArgs e)
        {
            key_press_light_helper(consumers[light_c_1], consumers[light_c_2], c_h_s_button, 11);
            key_press_light_helper(consumers[light_ch_1], consumers[light_ch_2], c_h_s_button, 11);
            key_press_button_helper(c_h_s_button, 255, 255, 64, 0, false);
            C_h_s.Play();
        }

        private async void tutorial_button_Click(object sender, RoutedEventArgs e)
        {
            List<RepeatButton> maryHadALittleLamb = new List<RepeatButton>
            {
                e_button, d_button, c_button, d_button, e_button, e_button, e_button,
                d_button, d_button, d_button, e_button, e_button, e_button,
                e_button, d_button,c_button, d_button, e_button, e_button, e_button,
                e_button, d_button, d_button, e_button, d_button, c_button
            };
            await Task.Delay(1500);
            foreach (RepeatButton button in maryHadALittleLamb)
            {
                set_color(button, 255, 233, 5, 188);
                await Task.Delay(1500);
                restore_color(button, 255, 255, 255, 255);
                await Task.Delay(100);
            }
        }
    }
}