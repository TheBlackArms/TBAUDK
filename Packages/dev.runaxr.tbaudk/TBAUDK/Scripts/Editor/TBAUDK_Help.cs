using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    public class TheBlackArms_Help
    {
        [MenuItem("TheBlackArms/Help/Github", false, 1049)]
        public static void OpenDiscordLink()
        {
            Application.OpenURL("https://github.com/TheBlackArms/TBAUDK");
        }

        [MenuItem("TheBlackArms/Utilities/Update configs", false, 1000)]
        public static void ForceUpdateConfigs()
        {
            TheBlackArms_ImportManager.updateConfig();
        }

        public static void UpdateTBAUDKBtn()
        {
            TheBlackArms_AutomaticUpdateAndInstall.AutomaticTBAUDKInstaller();
        }
    }
}