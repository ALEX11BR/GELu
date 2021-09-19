using Godot;
using System.Threading;

public class World : Node2D
{
    [Export]
    int level = 1;

    AnimationPlayer deadAnimationPlayer;
    Player player;
    bool dead = false;
    public bool won = false;
    public void reload()
    {
        if (!won)
        {
            GetTree().ChangeScene($"res://World{level}.tscn");
        }
        else
        {
            GetTree().ChangeScene($"res://World{level+1}.tscn");

        }
    }
    public override void _Ready()
    {
        deadAnimationPlayer = GetNode<AnimationPlayer>("DeadScreen/AnimationPlayer");
        player = GetNode<Player>("Player");
    }

    public void _on_Player_GameOver()
    {
        if (dead || won) return;

        dead = true;
        deadAnimationPlayer.Play("GameOver");
    }

    public void _on_Boss_Damaged(int lives)
    {
        if (lives == 0)
        {
            won = true;
            player.dead = true;
            deadAnimationPlayer.Play("LevelWon");
        }
        else
        {
            player.changeDirection();
        }
    }
}
