using Godot;
using System;

public partial class Bullet : Area2D
{
    [Export] public int Speed = 800;
    public Vector2 Direction = Vector2.Right;

    public override void _Ready()
    {
    Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    
    public override void _PhysicsProcess(double delta)
    {
        Position += Direction.Normalized() * Speed * (float)delta;

        // 화면 밖으로 나가면 삭제
        if (Position.X < -100 || Position.X > Global.screenSize.X + 100 ||
            Position.Y < -100 || Position.Y > Global.screenSize.Y + 100)
        {
            QueueFree();
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is Enemy enemy)
        {
            enemy.TakeDamage(Global.bulletDamage);
            QueueFree();          // 총알은 사라짐
        }
    }
}
