using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int Speed = 200;
    private AnimatedSprite2D anim;
    Vector2 screenSize;

    public override void _Ready()
    {
        screenSize = GetViewport().GetVisibleRect().Size;

        Position = new Vector2(screenSize.X/2, screenSize.Y/2);

        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        anim.Play("idle"); // 기본 애니메이션 실행
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;

        if (Input.IsActionPressed("ui_right") && Position.X < screenSize.X)
            velocity.X += 1;
        if (Input.IsActionPressed("ui_left") && Position.X > 0)
            velocity.X -= 1;
        if (Input.IsActionPressed("ui_down") && Position.Y < screenSize.Y)
            velocity.Y += 1;
        if (Input.IsActionPressed("ui_up") && Position.Y > 0)
            velocity.Y -= 1;

        Position += velocity.Normalized() * Speed * (float)delta;

        if (velocity.Length() < 0.1f)
            anim.Play("idle");
        else
            anim.Play("walk");
    }
}
