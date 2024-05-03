using System;
using Godot;
using LacpSniffer.Data.Models;
namespace LacpSniffer.Data.Controllers;

public partial class SnifferingViewController : Control
{
    [ExportCategory("SnifferingViewController")]
    [Export]
    public ItemList PacketsOutput { get; set; }

    public override async void _EnterTree()
    {
        base._EnterTree();
        SnifferController.MessageReceived += OnMessageReceived;
        if (!await SnifferController.StartListenningAsync())
        {
            var alert = new Popup()
            {
                Title = "Внимание!"
            };
            alert.AddChild(new Label() { Text = $"Непредвиденная ошибка" });
            GetViewport().AddChild(alert);
            alert.PopupCentered();
            OnReturnButtonPressed();
        }
    }

    private void OnMessageReceived(byte[] message)
    {
        PacketsOutput.AddItem(BitConverter.ToString(message));
    }

    public void OnClearItemsButtonPressed()
    {
        PacketsOutput.Clear();
    }

    public void OnReturnButtonPressed()
    {
        SnifferController.MessageReceived -= OnMessageReceived;
        SnifferController.StopListenning();
        GetTree().ChangeSceneToFile("res://Data/Views/main.tscn");
    }
}
