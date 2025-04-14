using Godot;
using System;

public partial class StartScene : Control
{
    public override void _Ready()
    {
        var mainText = GetNode<Label>("MainText");
        mainText.AddThemeFontSizeOverride("font_size", 48);

        GetNode<Button>("VBoxContainer/Button").Pressed += () => StartGame("easy");
        GetNode<Button>("VBoxContainer/Button2").Pressed += () => StartGame("hard");
        GetNode<Button>("VBoxContainer/Button3").Pressed += () => StartGame("runaway");
        
        Global.screenSize = GetViewport().GetVisibleRect().Size;
        Global.minBounds = new Vector2(0, 0);
        Global.maxBounds = new Vector2(Global.screenSize.X, Global.screenSize.Y);
    }

    private void StartGame(string mode)
    {
        GD.Print("선택한 모드: ", mode);
        
        // 모드 전달 방법 1: Global 변수 (싱글톤 이용)
        Global.GameMode = mode;
        // 게임 씬으로 전환
        GetTree().ChangeSceneToFile("res://Player.tscn");
    }
}
