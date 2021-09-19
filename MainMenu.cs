using Godot;
using System;

public class MainMenu : Control
{
    public void _on_ButtonLev1_pressed()
    {
        GetTree().ChangeScene("res://World1.tscn");
    }
    public void _on_ButtonLev2_pressed()
    {
        GetTree().ChangeScene("res://World2.tscn");
    }
}
