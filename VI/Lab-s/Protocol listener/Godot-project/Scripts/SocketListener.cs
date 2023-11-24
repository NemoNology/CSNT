using System.Net;
using System.Net.Sockets;
using System.Timers;

class RawSocketsListener
{
	readonly Socket socket = null!;
	readonly IPEndPoint endPoint;
	public List<byte[]> RawPackets { get; private set; }
	readonly System.Timers.Timer timer;
	bool isRecieving = false;

	public RawSocketsListener(IPAddress ip, int port)
	{
		timer = new()
		{
			AutoReset = false
		};
		timer.Elapsed += OnTimerElapsed;
		endPoint = new(ip, port);
		RawPackets = new List<byte[]>(64);
		socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		socket.Bind(endPoint);
	}

	public void StartReciving(bool clearAllPreviousPackets = true, TimeSpan? recievingTime = null!)
	{
		if (clearAllPreviousPackets)
			RawPackets.Clear();

		if (recievingTime is not null)
		{
			var ms = ((TimeSpan)recievingTime!).TotalMilliseconds;
			timer.Interval = ms;
			timer.Start();
			socket.ReceiveTimeout = (int)ms;
		}

		isRecieving = true;
		Task.Run(() =>
		{
			Recieving();
		});
	}

	void Recieving()
	{
		Span<byte> buffer = new byte[UInt16.MaxValue];
		int bufferLength;

		while (isRecieving)
		{
			bufferLength = socket.Receive(buffer);
			RawPackets.Add(buffer[..bufferLength].ToArray());
		}
	}

	public void WaitRecivingEnd()
	{
		while (isRecieving)
			Task.Delay(10).Wait();
	}

	void OnTimerElapsed(object? sender, ElapsedEventArgs e)
	{
		timer.Stop();
		StopRecieve();
	}

	public void ChangeEndPoint(IPAddress ip, int port)
	{
		StopRecieve();

		endPoint.Address = ip;
		endPoint.Port = port;
	}

	public void StopRecieve()
	{
		isRecieving = false;
	}
}