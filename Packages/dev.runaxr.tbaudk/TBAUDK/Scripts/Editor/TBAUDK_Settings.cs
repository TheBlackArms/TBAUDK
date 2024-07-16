using UnityEngine;
using UnityEditor;
using System.IO;
using System;
//using Amazon.S3.Model;
using UnityEngine.Serialization;

namespace TheBlackArms
{

    [InitializeOnLoad]
    public class TheBlackArms_Settings : EditorWindow
    {
        private const string Url = "https://github.com/TheBlackArms/TBAUDK/";
        private const string Url1 = "https://trigon.systems/";
        private const string Link = "";
        private const string Link1 = "";

        public static string projectConfigPath = "Packages/Toolkit/TheBlackArms/Configs/";
        private string backgroundConfig = "BackgroundVideo.txt";
        private static string projectDownloadPath = "Packages/Toolkit/TheBlackArms/Assets/";
        private static GUIStyle ToolkitHeader;
        public Color TBAUDKColor = Color.white;
        public static bool UITextRainbow { get; set; }
        //public Gradient TBAUDKGRADIENT;

        [MenuItem("TheBlackArms/Settings", false, 501)]
        public static void OpenSplashScreen()
        {
            GetWindow<TheBlackArms_Settings>(true);
        }

        public static string getAssetPath()
        {
            if (EditorPrefs.GetBool("TheBlackArms_onlyProject", false))
            {
                return projectDownloadPath;
            }

            var assetPath = EditorPrefs.GetString("TheBlackArms_customAssetPath", "%appdata%/TheBlackArms/")
                .Replace("%appdata%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))
                .Replace("/", "\\");

            if (!assetPath.EndsWith("\\"))
            {
                assetPath += "\\";
            }

            Directory.CreateDirectory(assetPath);
            return assetPath;
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("TheBlackArms Settings");

            maxSize = new Vector2(400, 600);
            minSize = maxSize;

            ToolkitHeader = new GUIStyle
            {
                normal =
                {
                    background = Resources.Load("TheBlackArmsUDKHeader") as Texture2D,
                    textColor = Color.white
                },
                fixedHeight = 200
            };
            
            if (!EditorPrefs.HasKey("TBA_discordRPC"))
            {
                EditorPrefs.SetBool("TBA_discordRPC", true);
            }

            if (!File.Exists(projectConfigPath + backgroundConfig) || !EditorPrefs.HasKey("TheBlackArms_background"))
            {
                EditorPrefs.SetBool("TheBlackArms_background", false);
                File.WriteAllText(projectConfigPath + backgroundConfig, "False");
            }
        }

        public void OnGUI()
        {
            GUILayout.Box("", ToolkitHeader);
            GUILayout.Space(4);
            GUI.backgroundColor = new Color(
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_R"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_G"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_B"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_A")
        );
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("TheBlackArms"))
            {
                Application.OpenURL(Url);
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Trigon.Systems"))
            {
                Application.OpenURL(Url1);
            }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = new Color(
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_R"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_G"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_B"),
            UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_A")
        );

            GUILayout.Space(4);
            EditorGUILayout.BeginVertical();
            GUI.backgroundColor = new Color(
           UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_R"),
           UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_G"),
           UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_B"),
           UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_A")
       );

            EditorGUILayout.LabelField("TheBlackArms Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space(10);
            //if (GUILayout.Button("Set Color"))
            //{
            //    UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_R", TBAUDKColor.r);
            //    UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_G", TBAUDKColor.g);
            //    UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_B", TBAUDKColor.b);
            //    UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_A", TBAUDKColor.a);
            //}

            EditorGUI.BeginChangeCheck();

            TBAUDKColor = EditorGUI.ColorField(new Rect(3, 270, position.width - 6, 15), "Kit UI Color", TBAUDKColor);
            //TBAUDKGRADIENT = EditorGUI.GradientField(new Rect(3, 360, position.width - 6, 15), "TBAUDK Gradient", TBAUDKGRADIENT);

            if (EditorGUI.EndChangeCheck())
            {
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_R", TBAUDKColor.r);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_G", TBAUDKColor.g);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_B", TBAUDKColor.b);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_A", TBAUDKColor.a);
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Reset Color"))
            {
                Color TBAUDKColor = Color.gray;

                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_R", TBAUDKColor.r);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_G", TBAUDKColor.g);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_B", TBAUDKColor.b);
                UnityEditor.EditorPrefs.SetFloat("TBAUDKColor_A", TBAUDKColor.a);
            }

            //TBAUDKGRADIENT = EditorGUI.GradientField(new Rect(3, 290, position.width - 6, 15), "TBAUDK Gradient", TBAUDKGRADIENT);

            EditorGUILayout.Space(10);
            EditorGUILayout.EndVertical();
            GUILayout.Label("Overall:");
            GUILayout.BeginHorizontal();
            var isDiscordEnabled = EditorPrefs.GetBool("TBA_discordRPC", true);
            var enableDiscord = EditorGUILayout.ToggleLeft("Discord RPC", isDiscordEnabled);
            if (enableDiscord != isDiscordEnabled)
            {
                EditorPrefs.SetBool("TBA_discordRPC", enableDiscord);
            }

            GUILayout.EndHorizontal();
            //Hide Console logs
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            var isHiddenConsole = EditorPrefs.GetBool("TheBlackArms_HideConsole");
            var enableConsoleHide = EditorGUILayout.ToggleLeft("Hide Console Errors", isHiddenConsole);
            if (enableConsoleHide == true)
            {
                EditorPrefs.SetBool("TheBlackArms_HideConsole", true);
                Debug.ClearDeveloperConsole();
                Debug.unityLogger.logEnabled = false;
            }
            else if (enableConsoleHide == false)
            {
                EditorPrefs.SetBool("TheBlackArms_HideConsole", false);
                Debug.ClearDeveloperConsole();
                Debug.unityLogger.logEnabled = true;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.BeginHorizontal();
            var isUITextRainbowEnabled = EditorPrefs.GetBool("TheBlackArms_UITextRainbow", false);
            var enableUITextRainbow = EditorGUILayout.ToggleLeft("Rainbow Text", isUITextRainbowEnabled);
            if (enableUITextRainbow != isUITextRainbowEnabled)
            {
                EditorPrefs.SetBool("TheBlackArms_UITextRainbow", enableUITextRainbow);
                UITextRainbow = true;
            }
            else
            {
                UITextRainbow = false;
            }


            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.Label("Upload panel:");
            GUILayout.BeginHorizontal();
            var isBackgroundEnabled = EditorPrefs.GetBool("TheBlackArms_background", false);
            var enableBackground = EditorGUILayout.ToggleLeft("Custom background", isBackgroundEnabled);
            if (enableBackground != isBackgroundEnabled)
            {
                EditorPrefs.SetBool("TheBlackArms_background", enableBackground);
                File.WriteAllText(projectConfigPath + backgroundConfig, enableBackground.ToString());
            }

            GUILayout.EndHorizontal();


            GUILayout.Space(4);
            GUILayout.Label("Import panel:");
            GUILayout.BeginHorizontal();
            var isOnlyProjectEnabled = EditorPrefs.GetBool("TheBlackArms_onlyProject", false);
            var enableOnlyProject = EditorGUILayout.ToggleLeft("Save files only in project", isOnlyProjectEnabled);
            if (enableOnlyProject != isOnlyProjectEnabled)
            {
                EditorPrefs.SetBool("TheBlackArms_onlyProject", enableOnlyProject);
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(4);
            GUI.backgroundColor = new Color(
             UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_R"),
             UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_G"),
             UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_B"),
             UnityEditor.EditorPrefs.GetFloat("TBAUDKColor_A")
         );
            GUILayout.Label("Asset path:");
            GUILayout.BeginHorizontal();
            var customAssetPath = EditorGUILayout.TextField("",
                EditorPrefs.GetString("TheBlackArms_customAssetPath", "%appdata%/TheBlackArms/"));
            if (GUILayout.Button("Choose", GUILayout.Width(60)))
            {
                var path = EditorUtility.OpenFolderPanel("Asset download folder",
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TheBlackArms");
                if (path != "")
                {
                    Debug.Log(path);
                    customAssetPath = path;
                }
            }

            if (GUILayout.Button("Reset", GUILayout.Width(50)))
            {
                customAssetPath = "%appdata%/TheBlackArms/";
            }

            if (EditorPrefs.GetString("TheBlackArms_customAssetPath", "%appdata%/TheBlackArms/") != customAssetPath)
            {
                EditorPrefs.SetString("TheBlackArms_customAssetPath", customAssetPath);
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            EditorPrefs.SetBool("TheBlackArms_ShowInfoPanel", GUILayout.Toggle(EditorPrefs.GetBool("TheBlackArms_ShowInfoPanel"), "Show at startup"));
            GUILayout.EndHorizontal();
        }
    }
}
// Soph waz 'ere