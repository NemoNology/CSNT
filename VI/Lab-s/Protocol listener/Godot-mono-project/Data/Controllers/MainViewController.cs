using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Godot;
using LacpSniffer.Data.Models;
using SharpPcap;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
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
            foreach (var device in CaptureDeviceList.Instance)
            {
                device.StopCapture();
                device.OnPacketArrival -= OnPacketArrival;
                device.Close();
            }
        }
        else
        {
            GD.Print(string.Join("", Enumerable.Repeat("-", 35)));
            PacketsOutput.Clear();
            foreach (var device in CaptureDeviceList.Instance)
            {
                device.Open();
                device.OnPacketArrival += OnPacketArrival;
                device.StartCapture();
            }
        }
        _isListening = !_isListening;
        StartStopListenningButton.Text = (_isListening ? "Закончить" : "Начать") + " слушать";
    }

    private void OnPacketArrival(object sender, PacketCapture e)
    {
        GD.Print(e.GetPacket().PacketLength);
    }

    public override void _Ready()
    {
        int i = 0;
        foreach (var device in CaptureDeviceList.Instance)
        {
            GD.Print($"Device {i}: {device.Name}");
            i++;
        }
        base._Ready();
    }

    public override void _ExitTree()
    {
        if (_isListening)
        {
            foreach (var device in CaptureDeviceList.Instance)
            {
                device.StopCapture();
                device.OnPacketArrival -= OnPacketArrival;
                device.Close();
            }
        }
        base._ExitTree();
    }
}
