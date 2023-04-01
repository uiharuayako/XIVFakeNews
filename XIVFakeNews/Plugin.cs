using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using XIVFakeNews.Windows;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using System;

namespace XIVFakeNews
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "FAKE NEWS";
        private const string CommandName = "/fakenews";

        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("XIVFakeNews");
        
        public MainWindow MainWindow { get; init; }
        [PluginService][RequiredVersion("1.0")] public static ChatGui ChatGui { get; private set; } = null!;
        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] CommandManager commandManager)
        {
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;

            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);
            
            MainWindow = new MainWindow(this);
            WindowSystem.AddWindow(MainWindow);

            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void SendMessage(XivChatType type,string name,string message)
        {
            XivChatEntry entry = new XivChatEntry();
            entry.Type = type;
            entry.Name = name;
            entry.Message = message;
            ChatGui.PrintChat(entry);
        }
        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            MainWindow.Toggle();
        }

        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {

            MainWindow.Toggle();
        }

    }
}
