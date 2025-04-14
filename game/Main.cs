using Godot;
using System;

public partial class Main : Node2D
{
    // 적 프리팹 로드
    PackedScene enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
    
    private int score = 0;
    private PauseMenu pauseMenu;

    public override void _Ready()
    {
        GD.Print("현재 모드: ", Global.GameMode);

        GD.Print("현재 해상도: ", Global.screenSize);

        pauseMenu = GetNode<PauseMenu>("CanvasLayer/PauseMenu");

        UpdateUI();
        
        // Timer에 연결
        Timer timer = GetNode<Timer>("EnemyTimer");
        timer.WaitTime = 1.5; // 1초 간격
        timer.OneShot = false;
        timer.Timeout += OnEnemyTimerTimeout;

        timer.Start();
    }

    //esc누르면 일시정지메뉴 뜸
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel")) // 기본적으로 ESC에 매핑
        {
            if (!GetTree().Paused)
                pauseMenu.ShowPauseMenu();
            else
                pauseMenu.HidePauseMenu();
        }
    }

    // 타이머에 의해 호출될 함수
    private void OnEnemyTimerTimeout()
    {
        
        var instance = enemyScene.Instantiate();

        if (instance is Node2D enemy)
        {
            float randomX = (float)GD.RandRange(0, (double)Global.screenSize.X);  // 해상도 가로불러온 후 랜덤
            float randomY = (float)GD.RandRange(0, (double)Global.screenSize.Y);  // 해상도 세로불러온 후 랜덤

            enemy.Position = new Vector2(randomX, randomY);
            AddChild(enemy);
        }
        else
        {
            GD.PrintErr("enemyScene의 루트가 Node2D가 아닙니다!");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        var scoreLabel = GetNode<Label>("CanvasLayer/ScoreLabel");

        scoreLabel.Text = $"점수: {score}";
    }
}
