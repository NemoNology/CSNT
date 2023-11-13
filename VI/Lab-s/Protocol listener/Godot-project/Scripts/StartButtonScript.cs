using Godot;
using System;

public partial class StartButtonScript : Button
{
    public void OnStopPressed()
    {
        Disabled = false;
    }

    public void OnPressed()
    {
        Disabled = true;
    }
}
