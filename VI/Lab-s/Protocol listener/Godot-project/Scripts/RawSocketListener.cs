using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class RawSocketsListener
{
    Socket socket = null!;
    readonly IPEndPoint endPoint;
    public const int dataBufferLength = 2048;
    readonly byte[] dataBuffer = new byte[dataBufferLength];
    public List<byte[]> RawPackets { get; private set; }
    public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();
    Task backgroundTask = null!;

    public RawSocketsListener(IPAddress ip, int port)
    {
        endPoint = new(ip, port);
        RawPackets = new List<byte[]>(64);
        socket = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        socket.Bind(endPoint);
        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
    }

    public void StartReciveAsync(bool clearAllPreviousPackets = true, bool isResizedPacketsAdding = true)
    {
        if (clearAllPreviousPackets)
            RawPackets.Clear();

        backgroundTask = Task.Run(() =>
        {
            StartRecive(CancellationTokenSource.Token, false, isResizedPacketsAdding);
        }, CancellationTokenSource.Token);
    }

    public void StartRecive(CancellationToken cancellationToken, bool clearAllPreviousPackets = true, bool isResizedPacketsAdding = true)
    {
        if (clearAllPreviousPackets)
            RawPackets.Clear();

        while (!cancellationToken.IsCancellationRequested)
        {
            int gettedDataSize = socket.Receive(dataBuffer);
            if (isResizedPacketsAdding)
                RawPackets.Add(dataBuffer[..gettedDataSize]);
            else
                RawPackets.Add(dataBuffer);
        }
    }

    public void ChangeEndPoint(IPAddress ip, int port)
    {
        endPoint.Address = ip;
        endPoint.Port = port;
        socket.Disconnect(true);
        socket.Bind(endPoint);
    }

    public void StopRecieve()
    {
        CancellationTokenSource.Cancel();
        while (!backgroundTask.IsCompleted)
            Task.Delay(10).Wait();
    }
}