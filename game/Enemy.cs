using Godot;
using System;

public partial class Enemy : Area2D
{
    private Vector2 moveDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized();

    public override void _Ready()
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered)));

        // 타이머 시그널 연결
        Timer timer = GetNode<Timer>("RandomMove");
        timer.WaitTime = 0.5; // 0.5초 간격
        timer.Timeout += OnMoveTimerTimeout;
        timer.Start();
    }
    public override void _PhysicsProcess(double delta)
    {
        float speed = GD.RandRange(0, 300);
        Position += moveDirection * speed * (float)delta;

        if (Position.X < -50 || Position.Y < -50 || Position.X > 1200 || Position.Y > 700) // 여유를 조금 두고
        {
            GD.Print("위치 기준으로 제거됨!", Position.X);
            QueueFree();
            GetNode<Main>("/root/Main").AddScore(-50);
        }
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

    private void OnMoveTimerTimeout()
    {
        // 새로운 랜덤 방향 설정 (단위 벡터로 정규화)
        float randX = GD.RandRange(-500, 500);
        float randY = GD.RandRange(-500, 500);
        moveDirection = new Vector2(randX, randY).Normalized();

        GD.Print("방향 갱신: ", moveDirection);
    }
}
