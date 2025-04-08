using Godot;
using System;

public partial class Player : Node2D
{
    [Export]
    public int Speed = 200;

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
    }
}
