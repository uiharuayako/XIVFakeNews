using System;
using System.Numerics;
using Dalamud.Game.Text;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;

namespace XIVFakeNews.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private Configuration configuration;

    public MainWindow(Plugin plugin) : base(
        "Fake News!", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 180),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        this.plugin = plugin;
        configuration = plugin.Configuration;
    }

    public void Dispose() { }

    public override void Draw()
    {
        ImGui.TextColored(ImGuiColors.DPSRed,"愚人节快乐！别开太过分的玩笑哦~");
        // 渲染下拉框
        if (ImGui.BeginCombo("类型", configuration.CurrentType.ToString()))
        {
            foreach (XivChatType type in Enum.GetValues(typeof(XivChatType)))
            {

                bool isSelected = configuration.CurrentType==type;
                if (ImGui.Selectable(type.ToString(),ref isSelected))
                {
                    configuration.CurrentType = type;
                    configuration.Save();
                }
            }

            ImGui.EndCombo();
        }

        string name = configuration.Name;
        if (ImGui.InputText("发送者", ref name, 500))
        {
            configuration.Name = name;
            configuration.Save();
        }

        string message = configuration.Message;
        if (ImGui.InputText("消息内容", ref message, 500))
        {
            configuration.Message = message;
            configuration.Save();
        }

        if (ImGui.Button("发送"))
        {
            plugin.SendMessage(configuration.CurrentType, configuration.Name, configuration.Message);
        }

        ImGui.Spacing();
        ImGui.Unindent(55);
    }
}
