using Godot;
using System;

public partial class Main : Node2D
{
    // 적 프리팹 로드
    PackedScene enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");

    public override void _Ready()
    {
        
        GD.Print("타이머 레디!");
        
        // Timer에 연결
        Timer timer = GetNode<Timer>("EnemyTimer");
        timer.WaitTime = 1.0; // 1초 간격
        timer.Autostart = true;
        timer.OneShot = false;
        timer.Timeout += OnEnemyTimerTimeout;

        timer.Start();
    }

    // 타이머에 의해 호출될 함수
    private void OnEnemyTimerTimeout()
    {
        
        var instance = enemyScene.Instantiate();

        GD.Print("생성된 인스턴스 타입: ", instance.GetType());

        if (instance is Node2D enemy)
        {
            float randomX = GD.RandRange(50, 1100);  // 해상도 가로 1200 기준
            float randomY = GD.RandRange(50, 600); // 해상도 세로 700 기준

            enemy.Position = new Vector2(randomX, randomY);
            AddChild(enemy);
            GD.Print("적 생성 완료!");
        }
        else
        {
            GD.PrintErr("enemyScene의 루트가 Node2D가 아닙니다!");
        }
    }
}
