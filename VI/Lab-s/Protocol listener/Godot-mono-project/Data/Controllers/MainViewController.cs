global using System;
global using System.Collections.Generic;
global using System.Linq;
global using Godot;
global using LacpSniffer.Data.Models;
using LACPsniffer.Data.Models;
using SharpPcap;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
	private bool _isListening = false;
	private int _previousPacketIndex = -1;
	private readonly List<Lacpdu> _packets = new(10);
	private readonly ILiveDevice _networkInterfaceDevice;

	[ExportCategory("Sniffer")]
	[Export]
	public ItemList PacketsOutput { get; set; }
	[Export]
	public RichTextLabel PacketOutput { get; set; }
	[Export]
	public Button StartStopListeningButton { get; set; }

	public MainViewController()
	{
		_networkInterfaceDevice = CaptureDeviceList
		.Instance
		.Where(d => d.Name == "\\Device\\NPF_{9D3F39FF-B9C3-4C72-815B-7C1A82202756}")
		.First();
	}

	private void OnStartStopListeningButtonPressed()
	{
		if (_isListening)
		{
			_networkInterfaceDevice.StopCapture();
			_networkInterfaceDevice.OnPacketArrival -= OnPacketArrival;
			_networkInterfaceDevice.Close();
		}
		else
		{
			PacketsOutput.Clear();
			_packets.Clear();
			_networkInterfaceDevice.Open();
			_networkInterfaceDevice.OnPacketArrival += OnPacketArrival;
			_networkInterfaceDevice.StartCapture();
		}
		_isListening = !_isListening;
		StartStopListeningButton.Text = (_isListening ? "Закончить" : "Начать") + " слушать";
	}

	private void OnPacketArrival(object sender, PacketCapture e)
	{
		var bytes = e.Data.ToArray();
		if (Lacpdu.TryParse(bytes, out Lacpdu? lacpdu))
		{
			AddItemToPacketsItems(lacpdu.ToString());
			_packets.Add((Lacpdu)lacpdu);
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
			SetPacketOutputText(_packets[index].FullInfo);
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
