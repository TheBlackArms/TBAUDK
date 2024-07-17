using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    [InitializeOnLoad]
    public class TheBlackArms_ImportPanel_Shaders : EditorWindow
    {
        private const string Url = "https://github.com/TheBlackArms/TBAUDK/";
        private const string Url1 = "https://trigon.systems/";
        private const string Link = "";
        private const string Link1 = "";

        private static GUIStyle _chillHeader;
        private static Dictionary<string, string> assets = new Dictionary<string, string>();
        private static int _sizeX = 400;
        private static int _sizeY = 5000;
        private static Vector2 _changeLogScroll;


        [MenuItem("TheBlackArms/Import panel/Shaders", false, 501)]
        public static void OpenImportPanel()
        {
            GetWindow<TheBlackArms_ImportPanel_Shaders>(true);
        }

        public void OnEnable()
        {
            titleContent = new GUIContent("TBAUDK Shaders Importer");

            TheBlackArms_ImportManager_Shaders.checkForConfigUpdate();
            LoadJson();

            maxSize = new Vector2(_sizeX, _sizeY);
            minSize = maxSize;

            _chillHeader = new GUIStyle
            {
                normal =
                {
                    background = Resources.Load("TheBlackArmsUDKHeader") as Texture2D,
                    textColor = Color.white
                },
                fixedHeight = 200
            };
        }

        public static void LoadJson()
        {
            assets.Clear();

            dynamic configJson =
                JObject.Parse(File.ReadAllText(TheBlackArms_Settings.projectConfigPath +
                                               TheBlackArms_ImportManager_Shaders.configName));

            Debug.Log("Server Asset Url is: " + configJson["config"]["serverUrl"]);
            TheBlackArms_ImportManager_Shaders.serverUrl = configJson["config"]["serverUrl"].ToString();
            _sizeX = (int)configJson["config"]["window"]["sizeX"];
            _sizeY = (int)configJson["config"]["window"]["sizeY"];

            foreach (JProperty x in configJson["assets"])
            {
                var value = x.Value;

                var buttonName = "";
                var file = "";

                foreach (var jToken in value)
                {
                    var y = (JProperty)jToken;
                    switch (y.Name)
                    {
                        case "name":
                            buttonName = y.Value.ToString();
                            break;
                        case "file":
                            file = y.Value.ToString();
                            break;
                    }
                }

                assets[buttonName] = file;
            }
        }

        public void OnGUI()
        {
            GUILayout.Box("", style: _chillHeader);
            GUILayout.Space(4);
            GUI.backgroundColor = new Color(
                EditorPrefs.GetFloat("TBAUDKColor_R"),
                EditorPrefs.GetFloat("TBAUDKColor_G"),
                EditorPrefs.GetFloat("TBAUDKColor_B"),
                EditorPrefs.GetFloat("TBAUDKColor_A")
            );
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Check for Updates"))
            {
                TheBlackArms_AutomaticUpdateAndInstall.AutomaticTBAUDKInstaller();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
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
            GUILayout.Space(4);
            //Update assets
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Update assets (config)"))
            {
                TheBlackArms_ImportManager.updateConfig();
            }

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            GUILayout.Space(4);


            //Imports V!V
            GUI.backgroundColor = new Color(
                EditorPrefs.GetFloat("TBAUDKColor_R"),
                EditorPrefs.GetFloat("TBAUDKColor_G"),
                EditorPrefs.GetFloat("TBAUDKColor_B"),
                EditorPrefs.GetFloat("TBAUDKColor_A")
            );
            _changeLogScroll = GUILayout.BeginScrollView(_changeLogScroll, GUILayout.Width(_sizeX));
            GUI.backgroundColor = new Color(
                EditorPrefs.GetFloat("TBAUDKColor_R"),
                EditorPrefs.GetFloat("TBAUDKColor_G"),
                EditorPrefs.GetFloat("TBAUDKColor_B"),
                EditorPrefs.GetFloat("TBAUDKColor_A")
            );
            foreach (var asset in assets)
            {
                GUILayout.BeginHorizontal();
                if (asset.Value == "")
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(asset.Key);
                    GUILayout.FlexibleSpace();
                }
                else
                {
                    if (GUILayout.Button(
                            (File.Exists(TheBlackArms_Settings.getAssetPath() + asset.Value) ? "Import" : "Download") +
                            " " + asset.Key))
                    {
                        TheBlackArms_ImportManager_Shaders.downloadAndImportAssetFromServer(asset.Value);
                    }

                    if (GUILayout.Button("Del", GUILayout.Width(40)))
                    {
                        TheBlackArms_ImportManager_Shaders.deleteAsset(asset.Value);
                    }
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndScrollView();
            GUILayout.BeginHorizontal();
            EditorPrefs.SetBool("TheBlackArms_ShowInfoPanel",
                GUILayout.Toggle(EditorPrefs.GetBool("TheBlackArms_ShowInfoPanel"), "Show at startup"));
            GUILayout.EndHorizontal();
        }
    }
}