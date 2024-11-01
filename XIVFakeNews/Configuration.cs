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

        public XivChatType CurrentType = XivChatType.Party;
        public string Name = "愚人节";
        public string Server = "";
        public string Message = "快乐！";

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private IDalamudPluginInterface? PluginInterface;

        public void Initialize(IDalamudPluginInterface pluginInterface)
        {
            PluginInterface = pluginInterface;
        }

        public void Save()
        {
            PluginInterface!.SavePluginConfig(this);
        }
    }
}
