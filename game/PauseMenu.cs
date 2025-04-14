using Godot;
using System;

public partial class PauseMenu : Control
{
    public override void _Ready()
    {
        GetNode<Button>("Panel/VBoxContainer/Button").Pressed += OnResumePressed;
        GetNode<Button>("Panel/VBoxContainer/Button2").Pressed += OnMenuPressed;
        GetNode<Button>("Panel/VBoxContainer/Button3").Pressed += OnQuitPressed;

        // 처음엔 안 보이게
        Visible = false;
    }

    public void ShowPauseMenu()
    {
        GetTree().Paused = true;
        Visible = true;
    }

    public void HidePauseMenu()
    {
        GetTree().Paused = false;
        Visible = false;
    }

    private void OnResumePressed()
    {
        HidePauseMenu();
    }

    private void OnMenuPressed()
    {
        HidePauseMenu();
        GetTree().ChangeSceneToFile("res://StartScene.tscn");
    }

    private void OnQuitPressed()
    {
        GetTree().Quit();
    }
}
