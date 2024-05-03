using Godot;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
#nullable enable
namespace LacpSniffer.Models;

public partial class SnifferController : Node
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public static event Action<byte[]>? MessageReceived;
    public static Socket Socket { get; private set; } = null!;
    public static IPEndPoint IpEndPoint { get; set; } = null!;

    public SnifferController()
    {
        Socket = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        Socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
        Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.HeaderIncluded, true);
        Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
    }

    public static bool IsAddressBusy(IPAddress address, int port)
    {
        foreach (var listener in IPGlobalProperties.GetIPGlobalProperties().GetActiveUdpListeners())
        {
            if (listener.Address.Equals(address) && listener.Port == port)
                return true;
        }

        return false;
    }

    public async Task<bool> StartListenningAsync(int port)
    {
        try
        {
            Socket.Bind(new IPEndPoint(IPAddress.Any, port));
            Socket.Connect(IpEndPoint);
            byte[] buffer = new byte[4096];
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                int receivedBytesLength = await Socket.ReceiveAsync(buffer, SocketFlags.None, _cancellationTokenSource.Token);
                if (receivedBytesLength > 0)
                    MessageReceived?.Invoke(buffer[..receivedBytesLength]);
            }
        }
        catch (OperationCanceledException)
        {
            return true;
        }

        return false;
    }

    public void StopListenning()
    {
        _cancellationTokenSource.Cancel();
    }
}
