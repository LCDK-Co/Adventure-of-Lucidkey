using Godot;
using System;

public partial class Enemy : Area2D
{
    private Vector2 moveDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized();
    private AnimatedSprite2D anim;
    private Player _player;  // 플레이어를 참조할 변수

    PackedScene explosionScene = GD.Load<PackedScene>("res://Explosion.tscn");
    PackedScene floatingTextScene = GD.Load<PackedScene>("res://FloatingText.tscn");

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));
        _player = GetNode<Player>("/root/Main/PlayerBody");

        // 타이머 시그널 연결
        Timer timer = GetNode<Timer>("RandomMove");
        timer.WaitTime = 0.5; // 0.5초 간격
        timer.Timeout += OnMoveTimerTimeout;
        timer.Start();
        
        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }
    public override void _PhysicsProcess(double delta)
    {
        
        float speed = GD.RandRange(0, 300);
        anim.FlipH = moveDirection.X < 0;
        
        if (Global.GameMode == "easy")
        {
            Position += moveDirection * speed * (float)delta;

            if (Position.X < 0)
            {
                moveDirection.X *= -1;
                Position = new Vector2(1, Position.Y); // 살짝 안쪽으로 밀기
            }
            else if (Position.X > Global.screenSize.X)
            {
                moveDirection.X *= -1;
                Position = new Vector2(Global.screenSize.X - 1, Position.Y);
            }

            if (Position.Y < 0)
            {
                moveDirection.Y *= -1;
                Position = new Vector2(Position.X, 1);
            }
            else if (Position.Y > Global.screenSize.Y)
            {
                moveDirection.Y *= -1;
                Position = new Vector2(Position.X, Global.screenSize.Y - 1);
            }
        }
        else if (Global.GameMode == "hard")
        {
            Position += moveDirection * speed * (float)delta;

            if (Position.X < 0 || Position.Y < 0 || Position.X > Global.screenSize.X || Position.Y > Global.screenSize.Y){ // 여유를 조금 두고
            QueueFree();
            GetNode<Main>("/root/Main").AddScore(-50);
            }
        }
        else if(Global.GameMode == "runaway"){
            Vector2 playerPosition = _player.GlobalPosition;
		    Vector2 enemyPosition = GlobalPosition;

		    // 플레이어와의 거리 계산
		    float distanceToPlayer = enemyPosition.DistanceTo(playerPosition);

		    if(distanceToPlayer < 300f){
			    speed = 250;
			    anim.Modulate = new Color(1, 0.7f, 0.7f); // 엄청 연한 빨간색
		    }
            else {
			    speed = 100;
			    anim.Modulate = new Color(1, 1, 1); // 기본
		    }   

		    Vector2 fleeDirection = (enemyPosition - playerPosition).Normalized();

		    // 도망치는 방향으로 이동 (속도 적용)
		    Vector2 velocity = fleeDirection * speed * (float)delta;  // delta는 프레임에 따른 속도 보정

		    // Position을 업데이트하여 이동
		    Position += velocity;
            Position = Position.Clamp(Global.minBounds, Global.maxBounds);
        }
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Player player)
        {
            GD.Print("잡혔다!");

            // 이펙트 생성
            var explosion = explosionScene.Instantiate<Explosion>();
            explosion.Position = GlobalPosition;
            GetParent().AddChild(explosion);

            // 텍스트 생성
            var floatingText = floatingTextScene.Instantiate() as FloatingText;
            floatingText.Position = GlobalPosition;
            floatingText.Setup("Ang!");
            GetParent().AddChild(floatingText);

            player.PlayHit();
            
            QueueFree(); // 아이템 제거
            GetNode<Main>("/root/Main").AddScore(100);
        }
    }

    private void OnMoveTimerTimeout()
    {
        // 새로운 랜덤 방향 설정 (단위 벡터로 정규화)
        float randX = GD.RandRange(-500, 500);
        float randY = GD.RandRange(-500, 500);
        moveDirection = new Vector2(randX, randY).Normalized();
    }
}
