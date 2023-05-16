using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using XIVFakeNews.Windows;
using Dalamud.Game.Gui;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Game.Text.SeStringHandling.Payloads;
using Dalamud.Utility;

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
            PluginInterface = pluginInterface;
            CommandManager = commandManager;

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
            var n = server.IsNullOrEmpty() ? new SeString(
                new TextPayload(name)) :
                new SeString(
                new TextPayload(name),
                new IconPayload(BitmapFontIcon.CrossWorld),
                new TextPayload(server));

            var msg = new SeString(
                new TextPayload(message)
                );

            ChatGui.PrintChat(new XivChatEntry()
            {
                Name = n,
                Message = msg,
                Type = type,
            });
        }
        public void Dispose()
        {
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
