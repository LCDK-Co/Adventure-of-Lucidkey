using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class LevelUpMenu : CanvasLayer
{
    public override void _Ready()
    {
        allOptions.Add(new UpgradeOption("폭탄 충전 속도 증가", () => Global.bombSpeed *= 0.8f));
        allOptions.Add(new UpgradeOption("공격력 증가", () => Global.bulletDamage += 20));
        allOptions.Add(new UpgradeOption("폭탄 +1", () => Global.bombCount += 1));
        allOptions.Add(new UpgradeOption("발사속도 증가", () => Global.shootCooldown *= 0.8f));
        allOptions.Add(new UpgradeOption("폭탄 데미지 증가", () => Global.bombDamage += 10));

        ShowRandomUpgrades();
    }

    private void ResumeGame()
    {
        GetTree().Paused = false;
        var main = GetNodeOrNull<Main>("/root/main");
        main?.UpdateUI();
        QueueFree(); // UI 제거
    }

    private void ShowRandomUpgrades()
{
    var random = new RandomNumberGenerator();
    random.Randomize();
    var chosen = allOptions.OrderBy(x => random.Randf()).Take(3).ToList();

    for (int i = 0; i < 3; i++)
    {
        var button = GetNode<Button>($"Panel/VBoxContainer/Button{i + 1}");
        if (button != null && i < chosen.Count)
        {
            button.Text = chosen[i].Name;
            int index = i; // 🔐 람다 캡처용 지역 복사
            button.Pressed += () => {
                chosen[index].Effect.Invoke();
                ResumeGame();
            };
        }
        else
        {
            GD.PrintErr($"버튼이 존재하지 않거나 chosen에 데이터 없음: i = {i}");
        }
    }
}

    
    private class UpgradeOption
    {
        public string Name;
        public Action Effect;

        public UpgradeOption(string name, Action effect)
        {
            Name = name;
            Effect = effect;
        }
    }
    private List<UpgradeOption> allOptions = new List<UpgradeOption>();
}
