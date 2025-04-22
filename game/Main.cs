using Godot;
using System;

public partial class Main : Node2D
{
    // 적 프리팹 로드
    PackedScene enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
    
    private int score = 0;
    private int bomb = 3;
    private PauseMenu pauseMenu;
    private Timer bombTimer;

    public override void _Ready()
    {
        GD.Print("현재 모드: ", Global.GameMode);

        GD.Print("현재 해상도: ", Global.screenSize);

        pauseMenu = GetNode<PauseMenu>("CanvasLayer/PauseMenu");

        if(Global.GameMode == "firemode"){
            var bombLabel = GetNode<Label>("CanvasLayer/BombLabel");
            bombLabel.Visible = true;
            bombTimer = GetNode<Timer>("CanvasLayer/BombTimer");
            
            bombTimer.WaitTime = Global.bombSpeed; // 3초 간격
            bombTimer.OneShot = false;
            bombTimer.Timeout += bombTimerTimeout;
            bombTimer.Start();

        }
        

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

    private void bombTimerTimeout()
    {
        if(bomb < Global.bombCount){
            AddBomb(1);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void UpdateUI()
    {
        var scoreLabel = GetNode<Label>("CanvasLayer/ScoreLabel");
        scoreLabel.Text = $"점수: {score}";

        if(Global.GameMode == "firemode"){
            var bombLabel = GetNode<Label>("CanvasLayer/BombLabel");
            bombLabel.Text = $"Bomb: {bomb} / {Global.bombCount}";
        }
    }
    public void AddBomb(int amount)
    {
        bomb += amount;
        UpdateUI();
    }

    public int getBomb(){
        return bomb;
    }
}
