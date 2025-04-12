using Godot;
using System;

public partial class Enemy : Area2D
{

	private Player _player;  // 플레이어를 참조할 변수

	private AnimatedSprite2D anim;

	public override void _Ready()
	{
		Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
		_player = GetNode<Player>("/root/Main/PlayerBody");
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		int speed = 100;

		Vector2 playerPosition = _player.GlobalPosition;
		Vector2 enemyPosition = GlobalPosition;

		// 플레이어와의 거리 계산
		float distanceToPlayer = enemyPosition.DistanceTo(playerPosition);

		if(distanceToPlayer < 300f){
			speed = 250;
			anim.Modulate = new Color(1, 0.7f, 0.7f); // 엄청 연한 빨간색
		} else {
			speed = 100;
			anim.Modulate = new Color(1, 1, 1); // 기본
		}

		Vector2 fleeDirection = (enemyPosition - playerPosition).Normalized();

		// 도망치는 방향으로 이동 (속도 적용)
		Vector2 velocity = fleeDirection * speed * (float)delta;  // delta는 프레임에 따른 속도 보정

		// Position을 업데이트하여 이동
		Position += velocity;

		anim.FlipH = fleeDirection.X < 0;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			GD.Print("잡혔다!");
			QueueFree(); // 아이템 제거
			GetNode<Main>("/root/Main").AddScore(200);
		}
	}
}
