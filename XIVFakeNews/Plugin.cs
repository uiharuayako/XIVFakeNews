using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using XIVFakeNews.Windows;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Plugin.Services;
using Dalamud.Utility;
using ECommons;
using ECommons.DalamudServices;

namespace XIVFakeNews
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "FAKE NEWS";
        private const string CommandName = "/fakenews";

        private IDalamudPluginInterface PluginInterface { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("XIVFakeNews");

        public MainWindow MainWindow { get; init; }
        public Plugin(
            IDalamudPluginInterface pluginInterface,
            ICommandManager commandManager)
        {
            PluginInterface = pluginInterface;
            CommandManager = commandManager;
            ECommonsMain.Init(pluginInterface, this);        
            
            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(PluginInterface);
            
            MainWindow = new MainWindow(this);
            WindowSystem.AddWindow(MainWindow);

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "/fakenews 打开假消息主页面"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public static void SendMessage(XivChatType type, string name, string server, string message)
        {
            var n = StringExtensions.IsNullOrEmpty(server) ? new SeString(
                new TextPayload(name)) :
                new SeString(
                new TextPayload(name),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload(server));

            var msg = new SeString(
                new TextPayload(message)
                );

            Svc.Chat.Print(new XivChatEntry()
            {
                Name = n,
                Message = msg,
                Type = type,
            });
        }
        public void Dispose()
        {
            ECommonsMain.Dispose();
            WindowSystem.RemoveAllWindows();
            CommandManager.RemoveHandler(CommandName);
        }

        private void OnCommand(string command, string args)
        {
            MainWindow.Toggle();
        }

        private void DrawUI()
        {
            WindowSystem.Draw();
        }

        public void DrawConfigUI()
        {

            MainWindow.Toggle();
        }

    }
}
