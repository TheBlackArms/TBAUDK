using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using System.Net.Http;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Threading.Tasks;


namespace TheBlackArms
{



    public class TheBlackArmsAutomaticUpdateAndInstall : MonoBehaviour
    {

        //get version from server
        private static string _versionURL = "https://c0dera.in/tbaudk/api/version.txt";
        //get download url
        private static string _unitypackageUrl = "https://c0dera.in/tbaudk/api/assets/latest/TBAUDK.unitypackage"; //This fucker is case sensitive... LMAO it took me 3 updates to figure it out

        //GetVersion
        private static string _currentVersion = File.ReadAllText("Packages/dev.runaxr.tbaudk/TBAUDKUpdater/editor/TBAUDKVersion.txt");


        //select where to be imported (TBAUDK)
        public static string AssetPath = "Assets\\"; //We put the unitypackage here temporarily
        //Custom name for downloaded unitypackage
        private static string _assetName = "tbaudk.unitypackage"; //We name it this because yes
        //gets Toolkit Directory Path
        private static string _toolkitPath = "Packages\\TBAUDK\\"; //This is the directory so the updater can kaboom the old files
        public async static void AutomaticTbaudkInstaller()
        {
            //Starting Browser
            HttpClient httpClient = new HttpClient();
            //Reading Version data
            var result = await httpClient.GetAsync(_versionURL);
            var strServerVersion = await result.Content.ReadAsStringAsync();
            var serverVersion = strServerVersion;

            var thisVersion = _currentVersion;

            try
            {
                //Checking if Uptodate or not
                if (serverVersion == thisVersion)
                {
                    //up to date
                    TheBlackArmsLog("you are using the newest version of TheBlackArms!");
                    EditorUtility.DisplayDialog("You are up to date",
                        "Current TBAUDK version: " + _currentVersion,
                        "Okay"
                        );
                }
                else
                {
                    //not up to date
                    TheBlackArmsLog("There is an Update Available");
                    //start download
                    await DownloadTheBlackArms();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("[TheBlackArms] AssetDownloadManager:" + ex.Message);
            }
        }

        private static async Task DownloadTheBlackArms()
        {
            TheBlackArmsLog("Asking for Approval..");
            if (EditorUtility.DisplayDialog("TheBlackArms Updater", "Your Version (V" + _currentVersion.ToString() + ") is Outdated!" + " do you want to Download and Import the Newest Version?", "Yes", "No"))
            {
                //starting deletion of old TBAUDK
                await DeleteAndDownloadAsync();
            }
            else
            {
                //canceling the whole process
                TheBlackArmsLog("You pressed no.");
            }
        }

        private static async Task DeleteAndDownloadAsync()
        {
            try
            {
                if (EditorUtility.DisplayDialog("TheBlackArms_Automatic_DownloadAndInstall", "The Old Toolset will Be Deleted and the New update Will be imported!", "Okay"))
                {
                    try
                    {
                        //gets every file in Toolkit folder
                        string[] toolkitDir = Directory.GetFiles(_toolkitPath, "*.*");

                        try
                        {
                            //Deletes All Files in Toolkit folder, I moved the DiscordRPC to a separate package because unity would hit a fatal error trying to remove its dll
                            await Task.Run(() =>
                            {
                                foreach (string f in toolkitDir)
                                {
                                    TheBlackArmsLog($"{f} - Deleted");
                                    File.Delete(f);
                                }
                            });
                        }
                        catch (Exception ex)
                        {
                            EditorUtility.DisplayDialog("Error Deleting Toolset", ex.Message, "Okay");
                        }
                    }
                    catch //catch nothing because removing this breaks it... would remove this if it didn't break shit
                    {
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                EditorUtility.DisplayDialog("Error Deleting Files", "Error wihle trying to find Toolkit Folder.", "Ignore");
            }
            AssetDatabase.Refresh();


            if (EditorUtility.DisplayDialog("TheBlackArms_Automatic_DownloadAndInstall", "The New devkit Will be imported now!", "Nice!"))
            {
                //Creates WebClient to Download latest .unitypackage
                WebClient w = new WebClient();
                w.Headers.Set(HttpRequestHeader.UserAgent, "Webkit Gecko wHTTPS (Keep Alive 55)");
                w.DownloadFileCompleted += new AsyncCompletedEventHandler(FileDownloadComplete);
                w.DownloadProgressChanged += FileDownloadProgress;
                string url = _unitypackageUrl;
                w.DownloadFileAsync(new Uri(url), _assetName);
            }
        }

        private static void FileDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            //Creates A ProgressBar
            var progress = e.ProgressPercentage;
            if (progress < 0) return;
            if (progress >= 100)
            {
                EditorUtility.ClearProgressBar();
            }
            else
            {
                EditorUtility.DisplayProgressBar("Download of " + _assetName,
                    "Downloading " + _assetName + " " + progress + "%",
                    (progress / 100F));
            }
        }

        private static void FileDownloadComplete(object sender, AsyncCompletedEventArgs e)
        {
            //Checks if Download is complete
            if (e.Error == null)
            {
                TheBlackArmsLog("Download completed!");
                //Opens .unitypackage
                Process.Start(_assetName);
            }
            else
            {
                //Asks to open Download Page Manually
                TheBlackArmsLog("Download failed!");
                if (EditorUtility.DisplayDialog("TheBlackArms_Automatic_DownloadAndInstall", "TheBlackArms Failed to Download to latest Version", "Open URL instead", "Cancel"))
                {
                    Application.OpenURL(_unitypackageUrl);
                }
            }
        }

        private static void TheBlackArmsLog(string message)
        {
            //Our Logger
            Debug.Log("[TheBlackArms] AssetDownloadManager: " + message);
        }
    }
}
