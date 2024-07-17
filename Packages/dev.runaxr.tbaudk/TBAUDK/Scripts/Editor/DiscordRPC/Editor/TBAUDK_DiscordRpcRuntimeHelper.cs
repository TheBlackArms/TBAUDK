using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace TBAUDK
{
    [InitializeOnLoadAttribute]
    public static class TBAUDK_DiscordRpcRuntimeHelper
    {
        // register an event handler when the class is initialized
        static TBAUDK_DiscordRpcRuntimeHelper()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
            EditorSceneManager.activeSceneChanged += sceneChanged;
        }

        private static void sceneChanged(Scene old, Scene next)
        {
            TBAUDK_DiscordRPC.sceneChanged(next);
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                TBAUDK_DiscordRPC.updateState(RpcState.EDITMODE);
                TBAUDK_DiscordRPC.ResetTime();
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                TBAUDK_DiscordRPC.updateState(RpcState.PLAYMODE);
                TBAUDK_DiscordRPC.ResetTime();
            }
        }
    }
}