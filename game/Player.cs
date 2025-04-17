using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export]
    public int _Speed = 200;
    private AnimatedSprite2D anim;
    [Export] public PackedScene BulletScene;
    Vector2 screenSize;
    private int hitSignal = 0;
    private Timer hitTimer;
    Vector2 minBounds;
	Vector2 maxBounds;

    Vector2 bulletDirection = new Vector2(1f,0);    //총알의 방향
    Vector2 lastmoveCheck = new Vector2(0,0);       //키보드의 마지막 입력 방향

    public override void _Ready()
    {
        screenSize = GetViewport().GetVisibleRect().Size;
        minBounds = new Vector2(0, 0);
        maxBounds = new Vector2(Global.screenSize.X, Global.screenSize.Y);

        Position = new Vector2(screenSize.X/2, screenSize.Y/2);

        anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        hitTimer = GetNode<Timer>("HitTimer");
        BulletScene = GD.Load<PackedScene>("res://Bullet.tscn");

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

        if (Input.IsActionPressed("ui_right")){
            velocity.X += 1;
            bulletDirection.X += 2;
            resetBulletDirection("Y");
        }
        if (Input.IsActionPressed("ui_left")){
            velocity.X -= 1;
            bulletDirection.X -= 2;
            resetBulletDirection("Y");
        }
        if (Input.IsActionPressed("ui_down")){
            velocity.Y += 1;
            bulletDirection.Y += 2;
            resetBulletDirection("X");
        }
        if (Input.IsActionPressed("ui_up")){
            velocity.Y -= 1;
            bulletDirection.Y -= 2;
            resetBulletDirection("X");
        }

        anim.FlipH = bulletDirection.X < 0;
        Position += velocity.Normalized() * Speed * (float)delta;
        Position = Position.Clamp(minBounds, maxBounds);
        
        if(Global.GameMode == "firemode"){
            //캐릭터가 정지하지 않았을때만 방향값 저장
            if (velocity != Vector2.Zero){
                lastmoveCheck = velocity.Normalized();
            }
            bulletDirection = ClampedDirection(bulletDirection);

            if (Input.IsActionPressed("shoot"))
            {
                FireBullet(calcDirection());
            }
            if (Input.IsActionJustPressed("bomb"))
            {
                if(GetNode<Main>("/root/Main").getBomb() < 1 ){
                    GD.Print("폭탄이 없다");
                }
                else{
                    for(int sur = 0 ; sur < 3 ; sur ++){
                    float delay = sur * 0.2f; // 0초, 0.2초, 0.4초 순서

                    GetTree().CreateTimer(delay).Timeout += () =>
                        {
                        FireBomb(calcDirection());
                        };
                    }
                    GetNode<Main>("/root/Main").AddBomb(-1);
                }
            }
        }
        

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

    //총알 방향 체크
    private Vector2 calcDirection(){
        if(bulletDirection.X == 0 && bulletDirection.Y == 0){
            return lastmoveCheck;
        }
        else{
            return bulletDirection; // 발사 방향 (왼쪽/오른쪽 등)
        }
    }

    //총알 발사
    private void FireBullet(Vector2 direction)
    {
        var bullet = BulletScene.Instantiate<Bullet>();
        bullet.Position = GlobalPosition; // 총알 출발 위치
        bullet.Direction = direction;
        
        GetParent().AddChild(bullet);
    }

    //특수키 발사
    private void FireBomb(Vector2 direction)
    {
        int checkXY = 0; //x = 0, y = 1
        if(Math.Abs(direction.X)<Math.Abs(direction.Y)){
            checkXY = 1;
            direction.X -= 2;
            if(direction.Y < 0){
                direction.Y = -10;
            }
            else{
                direction.Y = 10;
            }
        }
        else{
            direction.Y -= 2;
            if(direction.X < 0){
                direction.X = -10;
            }
            else{
                direction.X = 10;
            }
        }

        for(int i = -2 ; i < 3 ; i ++){
            FireBullet(direction);
            if(checkXY == 1){
                direction.X += 1;
            }
            else{
               direction.Y += 1; 
            }
        }
    }

    //벡터 범위 제한
    Vector2 ClampedDirection(Vector2 vec, float min = -10f, float max = 10f)
    {
        return new Vector2(
            Mathf.Clamp(vec.X, min, max),
            Mathf.Clamp(vec.Y, min, max)
        );
    }

    //내가 진행하고 있는 방향으로 총알 정렬
    private void resetBulletDirection(string direction){
        if(direction == "X"){
            if(bulletDirection.X == 0){
                return;
            }
            else if(bulletDirection.X < 0){
                bulletDirection.X += 1;
            }
            else{
                bulletDirection.X -= 1;
            }
        }
        else if(direction == "Y"){
            if(bulletDirection.Y == 0){
                return;
            }
            else if(bulletDirection.Y < 0){
                bulletDirection.Y += 1;
            }
            else{
                bulletDirection.Y -= 1;
            }
        }
    }
}
