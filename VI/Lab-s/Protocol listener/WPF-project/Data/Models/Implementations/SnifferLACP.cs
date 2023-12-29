using SharpPcap;

namespace WPF_project.Data.Models.Implementations
{
    class SnifferLACP : IDisposable
    {
        private readonly CaptureDeviceList _devices;
        private bool _isRunning = false;
        public event EventHandler<byte[]>? PacketReceived;
        public bool IsRunning => _isRunning;

        public SnifferLACP()
        {
            _devices = CaptureDeviceList.Instance;

            foreach (ICaptureDevice device in _devices)
            {
                device.Open();
                device.OnPacketArrival += OnPacketReceived;
            }
        }

        ~SnifferLACP()
        {
            Dispose();
        }

        public void Start()
        {
            if (!_isRunning)
            {
                _isRunning = true;

                foreach (var device in _devices)
                    device.StartCapture();
            }
        }

        public void Stop()
        {
            if (_isRunning)
            {
                foreach (var device in _devices)
                    device.StopCapture();

                _isRunning = false;
            }
        }

        private void OnPacketReceived(object? sender, PacketCapture e)
        {
            PacketReceived?.Invoke(sender, e.Data.ToArray());
        }

        public void Dispose()
        {
            Stop();

            foreach (var device in _devices)
            {
                device.Close();
                device.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}