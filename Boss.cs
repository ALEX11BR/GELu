using Godot;
using System;

public class Boss : Area2D
{
    [Export]
    Vector2 start;

    [Export]
    Vector2 end;

    [Export]
    float duration;

    [Export]
    public int lives = 2;

    [Export]
    bool fourthSpike = false;

    [Signal]
    public delegate void Damaged(int lives);

    AnimationPlayer animationPlayer;
    Sprite sprite;
    float xDirection = 1;
    Vector2 direction;
    public override void _Ready()
    {
        sprite = GetNode<Sprite>("Sprite");
        sprite.Frame = lives;
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

        var animation = new Animation();
        animation.Length = 2 * duration;
        var trackIndex = animation.AddTrack(Animation.TrackType.Value);
        animation.TrackSetPath(trackIndex, GetPath()+":position");
        animation.TrackInsertKey(trackIndex, 0, start);
        animation.TrackInsertKey(trackIndex, duration, end);
        animation.ValueTrackSetUpdateMode(trackIndex, Animation.UpdateMode.Continuous);
        animation.Loop = true;

        var movementAnimationPlayer = GetNode<AnimationPlayer>("MovementAnimationPlayer");
        movementAnimationPlayer.AddAnimation("Run", animation);
        movementAnimationPlayer.Play("Run");
    }
    public void _on_Boss_area_entered(Area2D area)
    {
        EmitSignal(nameof(Damaged), --lives);

        sprite.Frame = lives;
        if (lives == 0)
        {
            GetNode<Area2D>("SpikeArea").SetDeferred("monitorable", false);
            GetNode<Area2D>("SpikeArea2").SetDeferred("monitorable", false);
            GetNode<Area2D>("SpikeArea3").SetDeferred("monitorable", false);
            if (fourthSpike)
            {
                GetNode<Area2D>("SpikeArea4").SetDeferred("monitorable", false);
            }

            animationPlayer.Play("Die");
        }
        else
        {
            animationPlayer.Play("Hurt");
        }
    }
}
