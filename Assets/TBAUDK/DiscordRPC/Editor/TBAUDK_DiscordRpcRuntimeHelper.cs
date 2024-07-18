using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace TBAUDK
{
    [InitializeOnLoadAttribute]
    public static class TBAUDKDiscordRpcRuntimeHelper
    {
        // register an event handler when the class is initialized
        static TBAUDKDiscordRpcRuntimeHelper()
        {
            EditorApplication.playModeStateChanged += LogPlayModeState;
            EditorSceneManager.activeSceneChanged += SceneChanged;
        }

        private static void SceneChanged(Scene old, Scene next)
        {
            TbaudkDiscordRPC.SceneChanged(next);
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                TbaudkDiscordRPC.UpdateState(RpcState.EDITMODE);
                TbaudkDiscordRPC.ResetTime();
            }
            else if (state == PlayModeStateChange.EnteredPlayMode)
            {
                TbaudkDiscordRPC.UpdateState(RpcState.PLAYMODE);
                TbaudkDiscordRPC.ResetTime();
            }
        }
    }
}