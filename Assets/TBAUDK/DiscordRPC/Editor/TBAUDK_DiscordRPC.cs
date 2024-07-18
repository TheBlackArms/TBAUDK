using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TBAUDK
{
    [InitializeOnLoad]
    public class TbaudkDiscordRPC
    {
        private static readonly DiscordRpc.RichPresence Presence = new DiscordRpc.RichPresence();

        private static TimeSpan _time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        private static long _timestamp = (long)_time.TotalSeconds;

        private static RpcState _rpcState = RpcState.EDITMODE;
        private static string _gameName = Application.productName;
        private static string _sceneName = SceneManager.GetActiveScene().name;

        static TbaudkDiscordRPC()
        {
            if (EditorPrefs.GetBool("TBA_discordRPC", true))
            {
                TbaudkLog("Starting discord rpc");
                DiscordRpc.EventHandlers eventHandlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("1024018088661884999", ref eventHandlers, false, string.Empty);
                UpdateDRPC();
            }
        }

        public static void UpdateDRPC()
        {
            TbaudkLog("Updating everything");
            _sceneName = SceneManager.GetActiveScene().name;
            Presence.details = string.Format("Project: {0} Scene: {1}", _gameName, _sceneName);
            Presence.state = "State: " + _rpcState.StateName();
            Presence.startTimestamp = _timestamp;
            Presence.largeImageKey = "tba";
            Presence.largeImageText = "The Black Arms UDK";
            Presence.smallImageKey = "rxr";
            Presence.smallImageText = "By RunaXR";
            DiscordRpc.UpdatePresence(Presence);
        }

        public static void UpdateState(RpcState state)
        {
            TbaudkLog("Updating state to '" + state.StateName() + "'");
            _rpcState = state;
            Presence.state = "State: " + state.StateName();
            DiscordRpc.UpdatePresence(Presence);
        }

        public static void SceneChanged(Scene newScene)
        {
            TbaudkLog("Updating scene name");
            _sceneName = newScene.name;
            Presence.details = string.Format("Project: {0} Scene: {1}", _gameName, _sceneName);
            DiscordRpc.UpdatePresence(Presence);
        }

        public static void ResetTime()
        {
            TbaudkLog("Reseting timer");
            _time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            _timestamp = (long)_time.TotalSeconds;
            Presence.startTimestamp = _timestamp;

            DiscordRpc.UpdatePresence(Presence);
        }

        private static void TbaudkLog(string message)
        {
            Debug.Log("[TBAUDK] DiscordRPC: " + message);
        }
    }
}