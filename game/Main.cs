using Godot;
using System;

public partial class Main : Node2D
{
	// 적 프리팹 로드
	PackedScene enemyScene = GD.Load<PackedScene>("res://Enemy.tscn");
	
	private int score = 0;
	private int hp = 100;
	private double bonusRate = 1.0;
	private double spawnTime = 1.0;

	Timer timer;
	Timer timer2;
		
	public override void _Ready()
	{
		spawnTime = 1.0;
		UpdateUI();

		GD.Print("타이머 레디!");
		
		// Timer에 연결
		timer = GetNode<Timer>("EnemyTimer");
		timer.WaitTime = 1.0; // 1초 간격
		timer.OneShot = false;
		timer.Timeout += OnEnemyTimerTimeout;

		timer.Start();
		
		// Timer에 연결
		timer2 = GetNode<Timer>("EnemyTimer2");
		timer2.WaitTime = 10.0; // 10초 간격
		timer2.OneShot = false;
		timer2.Timeout += OnEnemyTimerTimeout2;

		timer.Start();
		timer2.Start();
	}

	private void OnEnemyTimerTimeout2()
	{
		if(spawnTime > 0.1){
			GD.Print("스피드업!!!");
			spawnTime = spawnTime * 0.9;
			bonusRate += 0.1;
		}
		timer.WaitTime = spawnTime;
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

	public void TakeDamage(int amount)
	{
		hp = Math.Max(0, hp - amount);
		UpdateUI();
	}

	public void AddScore(int amount)
	{
		score += (int)(amount * bonusRate);
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
