using Godot;
using System;

public partial class StotButtonScript : Button
{
    public void OnStartPressed()
    {
        Disabled = false;
    }

    public void OnPressed()
    {
        Disabled = true;
    }
}
