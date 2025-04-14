using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int _Speed = 200;
    private AnimatedSprite2D anim;
    Vector2 screenSize;
    private int hitSignal = 0;
    private Timer hitTimer;
    Vector2 minBounds;
	Vector2 maxBounds;

    public override void _Ready()
    {
        screenSize = GetViewport().GetVisibleRect().Size;
        minBounds = new Vector2(0, 0);
        maxBounds = new Vector2(Global.screenSize.X, Global.screenSize.Y);

        Position = new Vector2(screenSize.X/2, screenSize.Y/2);

        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        hitTimer = GetNode<Timer>("HitTimer");

        hitTimer.Timeout += () => changeHit(0); // 타이머 끝나면 hit 상태 해제

        anim.Play("idle"); // 기본 애니메이션 실행
    }
    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Vector2.Zero;
        int Speed = _Speed;

		if (Input.IsActionPressed("dash")){
			Speed = _Speed + 450;
			GetNode<Main>("/root/Main").AddScore(-3);
			anim.Modulate = new Color(1, 0.7f, 0.7f); // 엄청 연한 빨간색
		}
        else{
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

        if (velocity.Length() < 0.1f && hitSignal == 0)
            anim.Play("idle");
        else if(velocity.Length() < 0.1f)
            anim.Play("hit_idle");
        else if(hitSignal == 0)
            anim.Play("walk");
        else
            anim.Play("hit_walk");
    }

    public void PlayHit()
    {
        changeHit(1);

        // 타이머 재시작
        if (hitTimer.IsStopped())
        {
            hitTimer.Start(0.5f);
        }
        else
        {
            hitTimer.Stop();
            hitTimer.Start(0.5f);
        }
    }

    private void changeHit(int signal){
        if(signal == 1){
            this.hitSignal = 1;
        }
        else{
            this.hitSignal = 0;
        }
    }
}
