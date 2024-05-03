using Godot;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
#nullable enable
namespace LacpSniffer.Data.Models;

public partial class SnifferController : Node
{
    private static readonly CancellationTokenSource _cancellationTokenSource = new();
    private static Socket _socket = null!;
    private static IPEndPoint _ipEndPoint = null!;

    public static event Action<byte[]>? MessageReceived;

    public static IPEndPoint IPEndPoint
    {
        get => _ipEndPoint;
        set => _ipEndPoint = value;
    }

    public SnifferController()
    {
        _socket = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.IP);
        _socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.HeaderIncluded, true);
        _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
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

    public static async Task<bool> StartListenningAsync(int port = 0)
    {
        try
        {
            _socket.Bind(new IPEndPoint(IPAddress.Any, port));
            _socket.Connect(_ipEndPoint);
            byte[] buffer = new byte[4096];
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                int receivedBytesLength = await _socket.ReceiveAsync(buffer, SocketFlags.None, _cancellationTokenSource.Token);
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

    public static void StopListenning()
    {
        _cancellationTokenSource.Cancel();
    }
}
