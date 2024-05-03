using System.Net;
using Godot;
using LacpSniffer.Data.Models;
namespace LacpSniffer.Data.Controllers;

public partial class MainViewController : Control
{
    [Export]
    public LineEdit IpAddressInput { get; set; }
    [Export]
    public LineEdit PortInput { get; set; }
    [Export]
    public Label ErrorsOutput { get; set; }

    public void OnStartSnifferingButtonPressed()
    {
        ushort port = 0;
        if (!IPAddress.TryParse(IpAddressInput.Text, out IPAddress address))
        {
            ErrorsOutput.Text = "Некорректный IP-адрес;";
        }
        else if (!ushort.TryParse(PortInput.Text, out port))
        {
            ErrorsOutput.Text += "\nНекорректный порт;";
            return;
        }
        else if (SnifferController.IsAddressBusy(address, port))
        {
            ErrorsOutput.Text = "Данный адрес (IP + порт) занят;";
        }

        ErrorsOutput.Text = "";
        SnifferController.IpEndPoint = new(address, port);

        GetTree().ChangeSceneToFile("res://Data/Scenes/sniffering.tscn");
    }
}
