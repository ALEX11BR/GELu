using Godot;
using System;
using Utils;

public class Enemy : Area2D
{
    [Export]
    Color color;

    [Export]
    Vector2 start;

    [Export]
    Vector2 end;

    [Export]
    float duration;

    AnimationPlayer animationPlayer;
    float xDirection = 1;
    Vector2 direction;
    public override void _Ready()
    {
        GetNode<Sprite>("Sprite").Modulate = color;

        var animation = new Animation();
        animation.Length = 2 * duration;
        var trackIndex = animation.AddTrack(Animation.TrackType.Value);
        animation.TrackSetPath(trackIndex, GetPath()+":position");
        animation.TrackInsertKey(trackIndex, 0, start);
        animation.TrackInsertKey(trackIndex, duration, end);
        animation.ValueTrackSetUpdateMode(trackIndex, Animation.UpdateMode.Continuous);
        animation.Loop = true;

        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.AddAnimation("Run", animation);
        animationPlayer.Play("Run");
    }
}
