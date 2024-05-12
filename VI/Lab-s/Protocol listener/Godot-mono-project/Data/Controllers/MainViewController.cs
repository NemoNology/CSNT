using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using LacpSniffer.Data.Models;
using LACPsniffer.Data.Models;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
    private readonly CancellationTokenSource _cts = new();
    private Socket _socket;
    private bool _isListening = false;

    [ExportCategory("Sniffer")]
    [Export]
    public ItemList PacketsOutput { get; set; }
    [Export]
    public Label ErrorsOutput { get; set; }
    [Export]
    public Button StartStopListenningButton { get; set; }

    public const int BUFFERSIZE = ushort.MaxValue;

    private void OnStartStopSnifferingButtonPressed()
    {
        ErrorsOutput.Text = "";
        if (_isListening)
        {
            _cts.Cancel();
            _socket.Close();
        }
        else
        {
            PacketsOutput.Clear();
            _socket = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Raw);
            GD.Print("Socket created");
            Task.Run(() =>
            {
                try
                {
                    byte[] buffer = new byte[BUFFERSIZE];
                    _socket.Bind(new IPEndPoint(IPAddress.Any, 0));
                    while (!_cts.IsCancellationRequested)
                    {
                        int receivedBytesLength = _socket.Receive(buffer);
                        GD.Print($"Recieved bytes:\n\t{string.Join(", ", buffer[..receivedBytesLength].Select(x => x.ToString()))}");
                    }
                }
                catch (TaskCanceledException)
                {
                    GD.Print("Task was canceled");
                }
                catch (Exception e)
                {
                    GD.PushError("Error: " + e.Message);
                }
            }, _cts.Token);
        }
        _isListening = !_isListening;
        StartStopListenningButton.Text = (_isListening ? "Закончить" : "Начать") + " слушать";
    }

    private void OnPacketRecieved(LacpPacket packet)
    {
        PacketsOutput.AddItem(packet.ToString());
    }
}
