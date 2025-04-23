using Godot;
using System;

public partial class Obstacle : StaticBody2D
{
    [Export] public int MaxHp = 100;
    private int currentHp;
    private TextureProgressBar hpBar;

    public override void _Ready()
    {
        currentHp = MaxHp;  

        hpBar = GetNode<TextureProgressBar>("HpBar");
        hpBar.MaxValue = MaxHp;
        hpBar.Value = currentHp;
        
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        currentHp = Math.Max(currentHp, 0);

        hpBar.Value = currentHp;

        if (currentHp <= 0)
        {
            GD.Print("장애물 파괴됨!");
            QueueFree();
        }
    }
}
