using Godot;
using System;

public partial class Explosion : Node2D
{
    private Sprite2D sprite;
    private float lifetime = 1f; // 전체 지속 시간
    private float elapsedTime = 0f; // 경과 시간

    public override void _Ready()
    {
        sprite = GetNode<Sprite2D>("Sprite2D");
        sprite.Modulate = new Color(1, 1, 1, 1); // 불투명 시작
        Scale = new Vector2(1, 1); // 시작 스케일
    }

    public override void _Process(double delta)
    {
        elapsedTime += (float)delta;
        float t = elapsedTime / lifetime;

        // 스케일 점점 커지게 (1 → 2)
        Scale = new Vector2(1 + t, 1 + t);

        // 알파 점점 줄이기 (1 → 0)
        float alpha = Mathf.Lerp(1f, 0f, t);
        sprite.Modulate = new Color(1, 1, 1, alpha);

        if (elapsedTime >= lifetime)
        {
            QueueFree();
        }
    }
}
