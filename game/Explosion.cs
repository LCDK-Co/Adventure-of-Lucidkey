using Godot;
using System;

public partial class Explosion : Node2D
{
    public override void _Ready()
    {
        var anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        anim.Visible = true;        // 실행 시 표시
        anim.Play("default");       // 원하는 애니메이션 재생

        GetNode<Timer>("Timer").Timeout += () => QueueFree();
    }
}
