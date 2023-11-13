using Godot;
using System.Net.Sockets;
using System.Text;


public partial class MainScript : Control
{
    [Signal]
    public delegate void OnPacketGetEventHandler(string data);

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
    }

    public void OnStopPressed()
    {

    }

    public void OnStartPressed()
    {

    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }
}
