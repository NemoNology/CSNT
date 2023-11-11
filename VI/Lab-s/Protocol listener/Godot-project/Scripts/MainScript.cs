using Godot;
using System;
using System.Net.Sockets;
using System.Net;
using System.Text;

public partial class MainScript : Control
{
    [Signal]
    public delegate void OnPacketGetEventHandler(string data);

    Socket socket;
    byte[] bytes = new byte[256];
    bool isRunning = false;

    public override void _Ready()
    {
        base._Ready();
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65535));
        socket.Listen(32);
    }

    public override async void _Process(double delta)
    {
        base._Process(delta);

        while (isRunning)
        {
            var listener = await socket.AcceptAsync();
            var size = 0;
            var data = new StringBuilder();

            do
            {
                size = await listener.ReceiveAsync(bytes, SocketFlags.Peek);
                data.Append(Encoding.UTF8.GetString(bytes, 0, size));
            } while (listener.Available > 0);

            EmitSignal(SignalName.OnPacketGet, data.ToString());
        }
    }

    public void OnStopPressed()
    {
        isRunning = false;
    }

    public void OnStartPressed()
    {
        isRunning = true;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        socket.Close();
    }
}
