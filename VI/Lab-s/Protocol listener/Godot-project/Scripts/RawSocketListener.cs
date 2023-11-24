using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

class SocketsListener
{
    readonly Socket tcpSocket = null!;
    readonly Socket udpSocket = null!;
    readonly IPEndPoint endPoint;
    public const int dataBufferLength = 2048;
    readonly byte[] dataBuffer = new byte[dataBufferLength];
    public List<byte[]> RawPackets { get; private set; }
    readonly CancellationTokenSource cancellationTokenSource = new();
    Task backgroundTask = null!;

    public SocketsListener(IPAddress ip, int port)
    {
        endPoint = new(ip, port);
        RawPackets = new List<byte[]>(64);
        tcpSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        udpSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        tcpSocket.Bind(endPoint);
        udpSocket.Bind(endPoint);
        tcpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
        udpSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.HeaderIncluded, true);
    }

    public void StartReciveAsync(bool clearAllPreviousPackets = true,
                                bool isResizedPacketsAdding = true)
    {
        if (clearAllPreviousPackets)
            RawPackets.Clear();

        backgroundTask = Task.Run(() =>
        {
            StartRecive(cancellationTokenSource.Token, false, isResizedPacketsAdding);
        }, cancellationTokenSource.Token);
    }

    public void StartRecive(CancellationToken cancellationToken,
                            bool clearAllPreviousPackets = true,
                            bool isResizedPacketsAdding = true)
    {
        if (clearAllPreviousPackets)
            RawPackets.Clear();

        while (!cancellationToken.IsCancellationRequested)
        {
            int gettedDataSize = udpSocket.Receive(dataBuffer);
            if (isResizedPacketsAdding)
                RawPackets.Add(dataBuffer[..gettedDataSize]);
            else
                RawPackets.Add(dataBuffer);

            gettedDataSize = tcpSocket.Receive(dataBuffer);
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
        tcpSocket.Disconnect(true);
        udpSocket.Disconnect(true);
        tcpSocket.Bind(endPoint);
        udpSocket.Bind(endPoint);
    }

    public void StopRecieve()
    {
        cancellationTokenSource.Cancel();
        while (!backgroundTask.IsCompleted)
            Task.Delay(10).Wait();
    }

    public async Task StopRecieveAsync()
    {
        cancellationTokenSource.Cancel();
        await backgroundTask.WaitAsync(cancellationTokenSource.Token);
    }
}