using Godot;
using PcapDotNet.Core;
using System.Collections.Generic;
using System;

public partial class MainScript : Control
{
    [Signal]
    public delegate void OnPacketGetEventHandler(string data);

    public override void _Ready()
    {
        base._Ready();
        IList<LivePacketDevice> allDevices = LivePacketDevice.AllLocalMachine;

        if (allDevices.Count == 0)
        {
            GD.Print("No interfaces found! Make sure WinPcap is installed.");
            return;
        }

        for (int i = 0; i < allDevices.Count; i += 1)
        {
            LivePacketDevice device = allDevices[i];
            EmitSignal(SignalName.OnPacketGet,
                $"{(i + 1)}. {device.Name} ({device.Description ?? "No description available"})");
        }
    }
}
