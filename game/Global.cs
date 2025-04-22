using Godot;
using System;

public partial class Global : Node
{
    public static string GameMode = "easy";
    public static int Score = 0;
    public static int PlayerHP = 100;

    public static Vector2 screenSize = new Vector2(0,0);
    public static Vector2 minBounds;
	public static Vector2 maxBounds;

    public static int bulletDamage = 10;
    public static int bombDamage = 50;
    public static int bombCount = 3;
    public static float shootCooldown = 0.2f;
    public static float bombSpeed = 4f;
    
    public static float expBoost = 1f;
}