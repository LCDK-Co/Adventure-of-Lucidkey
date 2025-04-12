using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public int _Speed = 400;

	private AnimatedSprite2D anim;

	Vector2 minBounds = new Vector2(0, 0);
	Vector2 maxBounds = new Vector2(1280, 720);

	Vector2 screenSize;

	public override void _Ready()
	{
		screenSize = GetViewportRect().Size;
		maxBounds = screenSize;
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		anim.Play("walk"); // 기본 애니메이션 실행
	}
	public override void _PhysicsProcess(double delta)
	{


		Vector2 velocity = Vector2.Zero;
		
		int Speed = _Speed;

		if (Input.IsActionPressed("dash")){
			Speed = _Speed + 450;
			GetNode<Main>("/root/Main").AddScore(-3);
			anim.Modulate = new Color(1, 0.7f, 0.7f); // 엄청 연한 빨간색
		}else{
			anim.Modulate = new Color(1, 1, 1); // 기본
		}
			
		if (Input.IsActionPressed("ui_right"))
			velocity.X += 1;
		if (Input.IsActionPressed("ui_left"))
			velocity.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			velocity.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			velocity.Y -= 1;

		Position += velocity.Normalized() * Speed * (float)delta;
		Position = Position.Clamp(minBounds, maxBounds);

		if (Position.Length() < 0.1f)
			anim.Play("idle");
		else {
			if (Input.IsActionPressed("dash")){
				anim.Play("dash");
			} else {
				anim.Play("walk");
			}
		}
			

	}
}
