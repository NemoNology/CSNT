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
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private Socket _socket;
    private bool _isListening = false;

    [ExportCategory("Sniffer")]
    [Export]
    public LineEdit PortInput { get; set; }
    [Export]
    public ItemList PacketsOutput { get; set; }
    [Export]
    public Label ErrorsOutput { get; set; }
    [Export]
    public Button StartStopListenningButton { get; set; }

    public const int BUFFERSIZE = 4096;

    private void OnStartStopSnifferingButtonPressed()
    {
        ErrorsOutput.Text = "";
        if (_isListening)
        {
            _cancellationTokenSource.Cancel();
            _socket.Close();
        }
        else
        {
            if (!ushort.TryParse(PortInput.Text, out var port))
            {
                ErrorsOutput.Text = "Некорректный порт";
                return;
            }
            PacketsOutput.Clear();
            _socket = new(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Raw);
            Task.Run(() =>
            {
                EndPoint endPoint;
                byte[] buffer = new byte[BUFFERSIZE];
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    endPoint = new IPEndPoint(IPAddress.Any, port);
                    int receivedBytesLength = _socket.ReceiveFrom(buffer, BUFFERSIZE, SocketFlags.None, ref endPoint);
                    GD.Print($"Recieved bytes:\n\t{string.Join(", ", buffer[..receivedBytesLength].Select(x => x.ToString()))}");
                }
            }, _cancellationTokenSource.Token);
        }
        _isListening = !_isListening;
        StartStopListenningButton.Text = (_isListening ? "Начать" : "Закончить") + " слушать";
    }

    private void OnPacketRecieved(LacpPacket packet)
    {
        PacketsOutput.AddItem(packet.ToString());
    }
}
