using Godot;
using System;
using Utils;

public class Player : KinematicBody2D
{
    [Signal]
    public delegate void GameOver();
    public bool dead = false;
    float degreesToRotate = 0;
    float rotationDirection = -1;
    Node2D world;
    AnimationPlayer animationPlayer;
    Vector2 direction;
    public void changeDirection()
    {
        direction = -direction;
    }
    private float getFriction()
    {
        if (IsOnFloor())
            return Util.FRICTION;
        else
            return Util.FRICTION / 10;
    }
    private void jump()
    {
        animationPlayer.Play("Jumping");
        direction.y = -Util.JUMPFORCE;
    }
    private void rotateWorld(float sign)
    {
        degreesToRotate = 90 * sign;
        rotationDirection = sign;
    }
    public override void _Ready()
    {
        world = GetParent<Node2D>();
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public override void _Process(float delta)
    {
        if (dead) return;

        if (degreesToRotate != 0)
        {
            var currentRotationSpeed = Util.clamp0(
                Util.ROTATIONSPEED * rotationDirection * delta,
                Math.Abs(degreesToRotate)
            );

            world.RotationDegrees += currentRotationSpeed;
            RotationDegrees -= currentRotationSpeed;
            degreesToRotate -= currentRotationSpeed;
        }
    }
    public override void _PhysicsProcess(float delta)
    {
        if (dead) return;

        direction.y += Util.GRAVITY * delta;
        if (IsOnFloor())
        {
            if (Input.IsActionPressed("ui_down") && degreesToRotate == 0)
            {
                if (Input.IsActionJustPressed("ui_right"))
                {
                    jump();
                    rotateWorld(1);
                }
                else if (Input.IsActionJustPressed("ui_left"))
                {
                    jump();
                    rotateWorld(-1);
                }
                else if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
                {
                    animationPlayer.Play("Normal");
                }
                else
                {
                    animationPlayer.Play("ReadyToJump");
                }
            }
            else
            {
                animationPlayer.Play("Normal");
            }
        }

        var xInput = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
        if (xInput != 0)
        {
            direction.x = Util.clamp0(
                direction.x + ((xInput * Util.ACCELERATION - Util.ROTATIONBOOST * degreesToRotate) * delta),
                Util.MAXSPEED
            );
        }
        else
        {
            direction.x = Util.lerp0(direction.x, getFriction());
        }

        direction = MoveAndSlide(direction, Vector2.Up);
    }
    public void _on_HurtArea_area_entered(Area2D area)
    {
        if ((area.CollisionLayer & 2) != 2) return;
        if (dead) return;
        
        GetNode<Area2D>("HurtArea").SetDeferred("monitorable", false);
        animationPlayer.Play("Dead");
        dead = true;
        EmitSignal(nameof(GameOver));
    }
}
