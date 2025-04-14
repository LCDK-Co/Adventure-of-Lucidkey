using Godot;
using System;

public partial class Global : Node
{
    public static string GameMode = "easy";
    public int Score = 0;
    public int PlayerHP = 100;

    public static Vector2 screenSize = new Vector2(0,0);
    public static Vector2 minBounds;
	public static Vector2 maxBounds;
}