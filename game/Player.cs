using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed = 200;
    private AnimatedSprite2D anim;

    public override void _Ready()
    {
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        anim.Play("walk"); // 기본 애니메이션 실행
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("ui_right"))
            velocity.X += 1;
        if (Input.IsActionPressed("ui_left"))
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_down"))
            velocity.Y += 1;
        if (Input.IsActionPressed("ui_up"))
            velocity.Y -= 1;

        Position += velocity.Normalized() * Speed * (float)delta;

        if (Position.Length() < 0.1f)
            anim.Play("idle");
        else
            anim.Play("walk");
    }
}
