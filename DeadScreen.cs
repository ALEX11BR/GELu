using Godot;
using System;

public class DeadScreen : CanvasLayer
{
    AnimationPlayer animationPlayer;
    World world;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        world = GetParent<World>();
    }

    public void _on_Player_GameOver()
    {
        animationPlayer.Play("GameOver");
    }
    public void _on_Button_pressed()
    {
        world.reload();
    }
}
