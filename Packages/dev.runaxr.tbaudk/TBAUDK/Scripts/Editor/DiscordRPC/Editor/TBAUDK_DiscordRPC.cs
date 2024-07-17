using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TBAUDK
{
    [InitializeOnLoad]
    public class TBAUDK_DiscordRPC
    {
        private static readonly DiscordRpc.RichPresence presence = new DiscordRpc.RichPresence();

        private static TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
        private static long timestamp = (long)time.TotalSeconds;

        private static RpcState rpcState = RpcState.EDITMODE;
        private static string GameName = Application.productName;
        private static string SceneName = SceneManager.GetActiveScene().name;

        static TBAUDK_DiscordRPC()
        {
            if (EditorPrefs.GetBool("TBAUDK_discordRPC", true))
            {
                TBAUDKLog("Starting discord rpc");
                DiscordRpc.EventHandlers eventHandlers = default(DiscordRpc.EventHandlers);
                DiscordRpc.Initialize("1024018088661884999", ref eventHandlers, false, string.Empty);
                UpdateDRPC();
            }
        }

        public static void UpdateDRPC()
        {
            TBAUDKLog("Updating everything");
            SceneName = SceneManager.GetActiveScene().name;
            presence.details = string.Format("Project: {0} Scene: {1}", GameName, SceneName);
            presence.state = "State: " + rpcState.StateName();
            presence.startTimestamp = timestamp;
            presence.largeImageKey = "tba";
            presence.largeImageText = "The Black Arms UDK";
            presence.smallImageKey = "rxr";
            presence.smallImageText = "By RunaXR";
            DiscordRpc.UpdatePresence(presence);
        }

        public static void updateState(RpcState state)
        {
            TBAUDKLog("Updating state to '" + state.StateName() + "'");
            rpcState = state;
            presence.state = "State: " + state.StateName();
            DiscordRpc.UpdatePresence(presence);
        }

        public static void sceneChanged(Scene newScene)
        {
            TBAUDKLog("Updating scene name");
            SceneName = newScene.name;
            presence.details = string.Format("Project: {0} Scene: {1}", GameName, SceneName);
            DiscordRpc.UpdatePresence(presence);
        }

        public static void ResetTime()
        {
            TBAUDKLog("Reseting timer");
            time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
            timestamp = (long)time.TotalSeconds;
            presence.startTimestamp = timestamp;

            DiscordRpc.UpdatePresence(presence);
        }

        private static void TBAUDKLog(string message)
        {
            Debug.Log("[TBAUDK] DiscordRPC: " + message);
        }
    }
}