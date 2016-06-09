using org.allseen.LSF.LampState;
using System;
using System.Threading.Tasks;
using Windows.Devices.AllJoyn;

namespace piano
{
    public class LampHelper
    {
        private AllJoynBusAttachment busAttachment = null;
        private LampStateConsumer consumer = null;
        private string lampDeviceId = "3eb3073dcc98ec532af81ea929aa9462";

        public LampHelper()
        {
            // Initialize AllJoyn bus attachment.
            busAttachment = new AllJoynBusAttachment();

            // Initialize LampState watcher.
            LampStateWatcher watcher = new LampStateWatcher(busAttachment);

            // Subscribe to watcher added event - to watch for any producers with LampState interface.
            watcher.Added += Watcher_Added;

            // Start the LampState watcher.
            watcher.Start();
        }

        public event EventHandler LampFound;
        public event EventHandler LampStateChanged;

        public async Task<bool> GetOnOffAsync()
        {
            if (consumer != null)
            {
                // Get the current On/Off state of the lamp.
                LampStateGetOnOffResult onOffResult = await consumer.GetOnOffAsync();
                if (onOffResult.Status == AllJoynStatus.Ok)
                {
                    return onOffResult.OnOff;
                }
                else
                {
                    throw new Exception(string.Format("Error getting On/Off state - 0x{0:X}", onOffResult.Status));
                }
            }
            else
            {
                throw new NullReferenceException("No lamp found.");
            }
        }

        public async void SetOnOffAsync(bool value)
        {
            if (consumer != null)
            {
                await consumer.SetOnOffAsync(value);
            }
            else
            {
                throw new NullReferenceException("No lamp found.");
            }
        }
     

        private async void Watcher_Added(LampStateWatcher sender, AllJoynServiceInfo args)
        {
            AllJoynAboutDataView aboutData = await AllJoynAboutDataView.GetDataBySessionPortAsync(args.UniqueName, busAttachment, args.SessionPort);

            if (aboutData != null && !string.IsNullOrWhiteSpace(aboutData.DeviceId) && string.Equals(aboutData.DeviceId, lampDeviceId))
            {
                // Join session with the producer of the LampState interface.
                LampStateJoinSessionResult joinSessionResult = await LampStateConsumer.JoinSessionAsync(args, sender);

                if (joinSessionResult.Status == AllJoynStatus.Ok)
                {
                    consumer = joinSessionResult.Consumer;
                    LampFound?.Invoke(this, new EventArgs());
                    consumer.Signals.LampStateChangedReceived += Signals_LampStateChangedReceived;
                }
            }
        }

        private void Signals_LampStateChangedReceived(LampStateSignals sender, LampStateLampStateChangedReceivedEventArgs args)
        {
            LampStateChanged?.Invoke(this, new EventArgs());
        }
    }
}
