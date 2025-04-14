using Godot;
using System;

public partial class FloatingText : Node2D
{
    public override void _Ready()
    {
        GetNode<Timer>("Timer").Timeout += () => QueueFree();
    }

    public void Setup(string text)
    {
        GetNode<Label>("Label").Text = text;
    }

    public override void _Process(double delta)
    {
        Position += new Vector2(0, -30) * (float)delta; // 위로 떠오름
        Modulate = Modulate with { A = Modulate.A - 0.5f * (float)delta }; // 서서히 투명
    }
}
