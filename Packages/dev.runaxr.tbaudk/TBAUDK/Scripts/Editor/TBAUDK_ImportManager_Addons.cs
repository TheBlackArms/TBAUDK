using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    public class TheBlackArms_ImportManager_Addons
    {
        private const string V = "https://c0dera.in/tbaudk/api/assets/addons";
        public static string configName = "importConfig_addons.json";
        public static string serverUrl = V;
        public static string internalServerUrl = V;

        public static void downloadAndImportAssetFromServer(string assetName)
        {
            if (File.Exists(TheBlackArms_Settings.getAssetPath() + assetName))
            {
                TheBlackArmsLog(assetName + " exists. Importing it..");
                importDownloadedAsset(assetName);
            }
            else
            {
                TheBlackArmsLog(assetName + " does not exist. Starting download..");
                downloadFile(assetName);
            }
        }

        private static void downloadFile(string assetName)
        {
            WebClient w = new WebClient();
            w.Headers.Set(HttpRequestHeader.UserAgent, "Webkit Gecko wHTTPS (Keep Alive 55)");
            w.QueryString.Add("assetName", assetName);
            w.DownloadFileCompleted += fileDownloadCompleted;
            w.DownloadProgressChanged += fileDownloadProgress;
            string url = serverUrl + assetName;
            w.DownloadFileAsync(new Uri(url), TheBlackArms_Settings.getAssetPath() + assetName);
        }

        public static void deleteAsset(string assetName)
        {
            File.Delete(TheBlackArms_Settings.getAssetPath() + assetName);
        }

        public static void updateConfig()
        {
            WebClient w = new WebClient();
            w.Headers.Set(HttpRequestHeader.UserAgent, "Webkit Gecko wHTTPS (Keep Alive 55)");
            w.DownloadFileCompleted += configDownloadCompleted;
            w.DownloadProgressChanged += fileDownloadProgress;
            string url = internalServerUrl + configName;
            w.DownloadFileAsync(new Uri(url), TheBlackArms_Settings.projectConfigPath + "update_" + configName);
        }

        private static void configDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //var updateFile = File.ReadAllText(TheBlackArms_Settings.projectConfigPath + "update_" + configName);
                File.Delete(TheBlackArms_Settings.projectConfigPath + configName);
                File.Move(TheBlackArms_Settings.projectConfigPath + "update_" + configName,
                    TheBlackArms_Settings.projectConfigPath + configName);
                TheBlackArms_ImportPanel.LoadJson();

                EditorPrefs.SetInt("TheBlackArms_configImportLastUpdated",
                    (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TheBlackArmsLog("Import Config has been updated!");
            }
            else
            {
                TheBlackArmsLog("Import Config could not be updated!");
            }
        }

        private static void fileDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string assetName = ((WebClient)sender).QueryString["assetName"];
            if (e.Error == null)
            {
                TheBlackArmsLog("Download of file " + assetName + " completed!");
            }
            else
            {
                deleteAsset(assetName);
                TheBlackArmsLog("Download of file " + assetName + " failed!");
            }
        }

        private static void fileDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            var progress = e.ProgressPercentage;
            var assetName = ((WebClient)sender).QueryString["assetName"];
            if (progress < 0) return;
            if (progress >= 100)
            {
                EditorUtility.ClearProgressBar();
            }
            else
            {
                EditorUtility.DisplayProgressBar("Download of " + assetName,
                    "Downloading " + assetName + ". Currently at: " + progress + "%",
                    (progress / 100F));
            }
        }

        public static void checkForConfigUpdate()
        {
            if (EditorPrefs.HasKey("TheBlackArms_configImportLastUpdated"))
            {
                var lastUpdated = EditorPrefs.GetInt("TheBlackArms_configImportLastUpdated");
                var currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                if (currentTime - lastUpdated < 3600)
                {
                    Debug.Log("Not updating config: " + (currentTime - lastUpdated));
                    return;
                }
            }

            TheBlackArmsLog("Updating import config");
            updateConfig();
        }

        private static void TheBlackArmsLog(string message)
        {
            Debug.Log("[TheBlackArms] AssetDownloadManager: " + message);
        }

        public static void importDownloadedAsset(string assetName)
        {
            AssetDatabase.ImportPackage(TheBlackArms_Settings.getAssetPath() + assetName, true);
        }
    }
}