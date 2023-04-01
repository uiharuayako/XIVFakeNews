using Dalamud.Configuration;
using Dalamud.Plugin;
using System;
using Dalamud.Game.Text;

namespace XIVFakeNews
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public XivChatType CurrentType { get; set; } = XivChatType.Party;
        public string Name { get; set; } = "愚人节";

        public string Message { get; set; } = "快乐！";

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}
