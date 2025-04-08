using Godot;
using System;

public partial class Enemy : Node2D
{
    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(-50, 0) * (float)delta; // 왼쪽으로 이동
    }
}
