global using System;
global using System.Collections.Generic;
global using System.Linq;
global using Godot;
global using LacpSniffer.Data.Models;
using SharpPcap;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
	private bool _isListening = false;
	private int _previousPacketIndex = -1;
	private readonly List<LacpPacket> _packets = new(10);

	[ExportCategory("Sniffer")]
	[Export]
	public ItemList PacketsOutput { get; set; }
	[Export]
	public RichTextLabel PacketOutput { get; set; }
	[Export]
	public Button StartStopListeningButton { get; set; }

	private void OnStartStopListeningButtonPressed()
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
		StartStopListeningButton.Text = (_isListening ? "Закончить" : "Начать") + " слушать";
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
		if (_previousPacketIndex == index)
			SetPacketOutputText("");
		else
		{
			_previousPacketIndex = index;
			SetPacketOutputText(_packets[index].FullInfoAsString);
		}
	}

	private void OnPacketDeselected(Vector2 mousePosition, int mouseButtonIndex)
		=> SetPacketOutputText("");

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
