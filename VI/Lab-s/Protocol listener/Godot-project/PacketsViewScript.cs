using Godot;

public partial class PacketsViewScript : ItemList
{
    public override void _Ready()
    {
        base._Ready();
    }

    public void OnPacketGet(string data)
    {
        AddItem(data);
    }
}
