using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    [InitializeOnLoad]
    public class TheBlackArms_Info : EditorWindow
    {
        private const string Url = "https://github.com/TheBlackArms/TBAUDK/";
        private const string Url1 = "https://trigon.systems/";
        private const string Link = "";
        private const string Link1 = "https://trigonstatus.statuspage.io";

        static TheBlackArms_Info()
        {
            EditorApplication.update -= DoSplashScreen;
            EditorApplication.update += DoSplashScreen;
        }

        private static void DoSplashScreen()
        {
            EditorApplication.update -= DoSplashScreen;
            if (!EditorPrefs.HasKey("TheBlackArms_ShowInfoPanel"))
            {
                EditorPrefs.SetBool("TheBlackArms_ShowInfoPanel", true);
            }

            if (EditorPrefs.GetBool("TheBlackArms_ShowInfoPanel"))
                OpenSplashScreen();
        }

        private static Vector2 changeLogScroll;
        private static GUIStyle ToolkitHeader;
        private static GUIStyle TheBlackArmsBottomHeader;
        private static GUIStyle TheBlackArmsHeaderLearnMoreButton;
        private static GUIStyle TheBlackArmsBottomHeaderLearnMoreButton;

        [MenuItem("TheBlackArms/Info", false, 500)]
        public static void OpenSplashScreen()
        {
            GetWindow<TheBlackArms_Info>(true);
        }

        public static void Open()
        {
            OpenSplashScreen();
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("TheBlackArms Info");

            minSize = new Vector2(400, 700);
            ;
            TheBlackArmsBottomHeader = new GUIStyle();
            ToolkitHeader = new GUIStyle
            {
                normal =
                {
                    background = Resources.Load("TheBlackArmsUDKHeader") as Texture2D,
                    textColor = Color.white
                },
                fixedHeight = 200
            };
        }

        public void OnGUI()
        {
            GUILayout.Box("", ToolkitHeader);
            TheBlackArmsHeaderLearnMoreButton = EditorStyles.miniButton;
            TheBlackArmsHeaderLearnMoreButton.normal.textColor = Color.black;
            TheBlackArmsHeaderLearnMoreButton.fontSize = 12;
            TheBlackArmsHeaderLearnMoreButton.border = new RectOffset(10, 10, 10, 10);
            Texture2D texture = AssetDatabase.GetBuiltinExtraResource<Texture2D>("UI/Skin/UISprite.psd");
            TheBlackArmsHeaderLearnMoreButton.normal.background = texture;
            TheBlackArmsHeaderLearnMoreButton.active.background = texture;
            GUILayout.Space(4);
            GUI.backgroundColor = new Color(
                EditorPrefs.GetFloat("TBAUDKColor_R"),
                EditorPrefs.GetFloat("TBAUDKColor_G"),
                EditorPrefs.GetFloat("TBAUDKColor_B"),
                EditorPrefs.GetFloat("TBAUDKColor_A")
            );
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("TheBlackArms Ultimate Development Kit"))
            {
                Application.OpenURL(Url);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Trigon.Systems"))
            {
                Application.OpenURL(Url1 + Link);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            //Update assets
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Status"))
            {
                Application.OpenURL(Link1);
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.Label("TheBlackArms Version 1.0.3");
            GUILayout.Space(2);
            GUILayout.Label("TBAUDK imported correctly if you are seeing this");
            changeLogScroll = GUILayout.BeginScrollView(changeLogScroll, GUILayout.Width(390));

            GUILayout.Label(
                @"
== The Black Arms Ultimate Development Kit ==

This Unity Kit is hopefully providing everything you need

------------------------------------------------------------
∞∞∞∞∞∞∞∞∞∞∞∞Information∞∞∞∞∞∞∞∞∞∞∞∞
This unity kit provides tools and scripts for you
I am not responsible for misuse of these tools and scripts
The goal is to become the main package anyone needs
If you have issues visit the github repository issues tab
Updates can be done from within unity itself (or manually)
Bugs/Issues can be reported via github issues
There is not a discord for TheBlackArms

------------------------------------------------------------
∞∞∞∞∞∞∞Contributors to TheBlackArms Unity Kit∞∞∞∞∞∞∞
> Developer: PhoenixAceVFX
- Contributor : WTFBlaze -Made the import system
============================================
");
            GUILayout.EndScrollView();
            GUILayout.Space(4);

            GUILayout.Box("", TheBlackArmsBottomHeader);
            TheBlackArmsBottomHeaderLearnMoreButton = EditorStyles.miniButton;
            TheBlackArmsBottomHeaderLearnMoreButton.normal.textColor = Color.black;
            TheBlackArmsBottomHeaderLearnMoreButton.fontSize = 10;
            TheBlackArmsBottomHeaderLearnMoreButton.border = new RectOffset(10, 10, 10, 10);
            TheBlackArmsBottomHeaderLearnMoreButton.normal.background = texture;
            TheBlackArmsBottomHeaderLearnMoreButton.active.background = texture;

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            EditorPrefs.SetBool("TheBlackArms_ShowInfoPanel",
                GUILayout.Toggle(EditorPrefs.GetBool("TheBlackArms_ShowInfoPanel"), "Show at startup"));
            GUILayout.EndHorizontal();
        }
    }
}