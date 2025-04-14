using Godot;
using System;

public partial class Main : Node2D
{
    // 적 프리팹 로드
    PackedScene enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
    Vector2 screenSize;
    
    private int score = 0;
    private int hp = 100;

    public override void _Ready()
    {
        screenSize = GetViewport().GetVisibleRect().Size;
        GD.Print("현재 해상도: ", screenSize);

        UpdateUI();
        
        // Timer에 연결
        Timer timer = GetNode<Timer>("EnemyTimer");
        timer.WaitTime = 1.5; // 1초 간격
        timer.OneShot = false;
        timer.Timeout += OnEnemyTimerTimeout;

        timer.Start();
    }

    // 타이머에 의해 호출될 함수
    private void OnEnemyTimerTimeout()
    {
        
        var instance = enemyScene.Instantiate();

        if (instance is Node2D enemy)
        {
            float randomX = (float)GD.RandRange(0, (double)screenSize.X);  // 해상도 가로불러온 후 랜덤
            float randomY = (float)GD.RandRange(0, (double)screenSize.Y);  // 해상도 세로불러온 후 랜덤

            enemy.Position = new Vector2(randomX, randomY);
            AddChild(enemy);
        }
        else
        {
            GD.PrintErr("enemyScene의 루트가 Node2D가 아닙니다!");
        }
    }

    public void TakeDamage(int amount)
    {
        hp = Math.Max(0, hp - amount);
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var hpBar = GetNode<ProgressBar>("CanvasLayer/HpBar");
        var scoreLabel = GetNode<Label>("CanvasLayer/ScoreLabel");

        hpBar.Value = hp;
        scoreLabel.Text = $"점수: {score}";
    }
}
