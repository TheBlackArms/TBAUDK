using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    [InitializeOnLoad]
    public class TheBlackArmsInfo : EditorWindow
    {
        private const string Url = "https://github.com/TheBlackArms/TBAUDK/";
        private const string Url1 = "https://trigon.systems/";
        private const string Link = "";
        private const string Link1 = "https://trigonstatus.statuspage.io";

        static TheBlackArmsInfo()
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

        private static Vector2 _changeLogScroll;
        private static GUIStyle _toolkitHeader;
        private static GUIStyle _theBlackArmsBottomHeader;
        private static GUIStyle _theBlackArmsHeaderLearnMoreButton;
        private static GUIStyle _theBlackArmsBottomHeaderLearnMoreButton;

        [MenuItem("TheBlackArms/Info", false, 500)]
        public static void OpenSplashScreen()
        {
            GetWindow<TheBlackArmsInfo>(true);
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
            _theBlackArmsBottomHeader = new GUIStyle();
            _toolkitHeader = new GUIStyle
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
            GUILayout.Box("", _toolkitHeader);
            _theBlackArmsHeaderLearnMoreButton = EditorStyles.miniButton;
            _theBlackArmsHeaderLearnMoreButton.normal.textColor = Color.black;
            _theBlackArmsHeaderLearnMoreButton.fontSize = 12;
            _theBlackArmsHeaderLearnMoreButton.border = new RectOffset(10, 10, 10, 10);
            Texture2D texture = AssetDatabase.GetBuiltinExtraResource<Texture2D>("UI/Skin/UISprite.psd");
            _theBlackArmsHeaderLearnMoreButton.normal.background = texture;
            _theBlackArmsHeaderLearnMoreButton.active.background = texture;
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
            _changeLogScroll = GUILayout.BeginScrollView(_changeLogScroll, GUILayout.Width(390));

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

            GUILayout.Box("", _theBlackArmsBottomHeader);
            _theBlackArmsBottomHeaderLearnMoreButton = EditorStyles.miniButton;
            _theBlackArmsBottomHeaderLearnMoreButton.normal.textColor = Color.black;
            _theBlackArmsBottomHeaderLearnMoreButton.fontSize = 10;
            _theBlackArmsBottomHeaderLearnMoreButton.border = new RectOffset(10, 10, 10, 10);
            _theBlackArmsBottomHeaderLearnMoreButton.normal.background = texture;
            _theBlackArmsBottomHeaderLearnMoreButton.active.background = texture;

            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            EditorPrefs.SetBool("TheBlackArms_ShowInfoPanel",
                GUILayout.Toggle(EditorPrefs.GetBool("TheBlackArms_ShowInfoPanel"), "Show at startup"));
            GUILayout.EndHorizontal();
        }
    }
}