using Godot;

public partial class PacketsViewScript : ItemList
{
	public void OnPacketGet(string data)
	{
		AddItem(data);
	}
}
