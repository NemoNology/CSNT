using Godot;

namespace CSNT.Clientserverchat.Data.Controllers
{
	public partial class MainViewController : Control
	{
		private void OnOpenServerWindowButtonPressed() =>
			CreateNewWindow("res://Data/Views/server.tscn", "Сервер");

		private void OnOpenClientWindowButtonPressed() =>
			CreateNewWindow("res://Data/Views/client.tscn", "Клиент");

		private void CreateNewWindow(string scenePath, string title)
		{
			var window = new Window() { Title = title, Size = new Vector2I(640, 480) };
			window.AddChild(GD.Load<PackedScene>(scenePath).Instantiate());
			window.CloseRequested += () => { window.QueueFree(); };
			GetTree().Root.AddChild(window);
			window.PopupCentered();
		}
	}
}
