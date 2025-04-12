using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed = 200;
    private AnimatedSprite2D anim;

    public override void _Ready()
    {
        float randomX = GD.RandRange(50, 1100);  // 해상도 가로 1200 기준
        float randomY = GD.RandRange(50, 600); // 해상도 세로 700 기준

        Position = new Vector2(randomX, randomY);

        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        anim.Play("idle"); // 기본 애니메이션 실행
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("ui_right") && Position.X < 1152)
            velocity.X += 1;
        if (Input.IsActionPressed("ui_left") && Position.X > 0)
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_down") && Position.Y < 648)
            velocity.Y += 1;
        if (Input.IsActionPressed("ui_up") && Position.Y > 0)
            velocity.Y -= 1;

        Position += velocity.Normalized() * Speed * (float)delta;

        if (Position.Length() < 0.1f)
            anim.Play("idle");
        else
            anim.Play("walk");
    }
}
