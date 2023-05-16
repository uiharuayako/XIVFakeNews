using System;
using System.Numerics;
using Dalamud.Game.Text;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Windowing;
using ImGuiNET;

namespace XIVFakeNews.Windows;

public class MainWindow : Window, IDisposable
{
    private Plugin plugin;
    private Configuration configuration;

    public MainWindow(Plugin plugin) : base(
        "Fake News!", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
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
                if (ImGui.Selectable(type.ToString()))
                {
                    configuration.CurrentType = type;
                    configuration.Save();
                }
            }
            ImGui.EndCombo();
        }

        if (ImGui.InputText("发送者", ref configuration.Name, 500))
        {
            configuration.Save();
        }

        if (ImGui.InputText("服务器", ref configuration.Server, 500))
        {
            configuration.Save();
        }

        if (ImGui.InputText("消息内容", ref configuration.Message, 500))
        {
            configuration.Save();
        }

        if (ImGui.Button("发送"))
        {
            Plugin.SendMessage(configuration.CurrentType, configuration.Name, configuration.Server, configuration.Message);
        }

        ImGui.Spacing();
        ImGui.Unindent(55);
    }
}
