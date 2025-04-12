using Godot;
using System;

public partial class Item : Area2D
{
	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			GD.Print("아이템 획득!");
			QueueFree(); // 아이템 제거
		}
	}
}
