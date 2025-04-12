using Godot;
using System;

public partial class Enemy : Area2D
{

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}
	public override void _PhysicsProcess(double delta)
	{
		Position += new Vector2(-50, 0) * (float)delta; // 왼쪽으로 이동
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			GD.Print("잡혔다!");
			QueueFree(); // 아이템 제거
			GetNode<Main>("/root/Main").AddScore(100);
		}
	}
}
