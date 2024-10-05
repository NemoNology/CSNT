using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using LacpSniffer.Data.Models;
using LACPsniffer.Data.Models;
using SharpPcap;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
    private bool _isListening = false;
    private int _previousePacketIndex = -1;
    private readonly List<LacpPacket> _packets = new(10);

    [ExportCategory("Sniffer")]
    [Export]
    public ItemList PacketsOutput { get; set; }
    [Export]
    public RichTextLabel PacketOutput { get; set; }
    [Export]
    public Button StartStopListenningButton { get; set; }

    private void OnStartStopSnifferingButtonPressed()
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
        else
        {
            PacketsOutput.Clear();
            _packets.Clear();
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
        var bytes = e.Data.ToArray();
        bool isLacp = bytes.IsLacpPacket();
        if (isLacp)
        {
            var packet = bytes.ToLacpPacket();
            if (_packets.Contains(packet))
                return;

            AddItemToPacketsItems(packet.ToString());
            _packets.Add(packet);
        }
        // else
        // {
        //     GD.Print($"Packet arrivied; Is LAPC: {isLacp}");
        //     GD.Print($"\tPacket data length: {bytes.Length}; [LACP - {LacpPacket.LENGTH}]");
        //     GD.Print($"\tPacket destination: {bytes[..6].ToMacAddressString()}; [LACP - {LacpPacket.LacpDestinationAddress.ToMacAddressString()}]");
        //     GD.Print($"\tPacket type/length: {bytes[12..14].ToHexString()}; [LACP - {LacpPacket.TypeLengthOfLacpPacket.ToHexString()}]");
        // }
    }

    private void SetPacketOutputText_ND(string text)
    {
        PacketOutput.Text = text;
    }

    private void AddItemToPacketsItems_ND(string item)
    {
        PacketsOutput.AddItem(item);
    }

    private void SetPacketOutputText(string text)
    {
        CallDeferred(nameof(SetPacketOutputText_ND), text);
    }

    private void AddItemToPacketsItems(string item)
    {
        CallDeferred(nameof(AddItemToPacketsItems_ND), item);
    }

    private void OnPacketSelected(int index)
    {
        if (_previousePacketIndex == index)
            SetPacketOutputText("");
        else
        {
            _previousePacketIndex = index;
            SetPacketOutputText(_packets[index].FullInfoAsString);
        }
    }

    private void OnPacketDeselected(Vector2 mousePosition, int mouseButtonIndex)
        => SetPacketOutputText("");

    public override void _Ready()
    {
        int i = 0;
        foreach (var device in CaptureDeviceList.Instance)
        {
            GD.Print($"Device {i}: {device.Name}. {device.Description}");
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
