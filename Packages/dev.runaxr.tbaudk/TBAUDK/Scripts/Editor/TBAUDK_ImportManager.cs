using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEditor;
using UnityEngine;

namespace TheBlackArms
{
    public class TheBlackArmsImportManager
    {
        private const string V = "https://c0dera.in/tbaudk/api/assets/";
        public static string ConfigName = "importConfig.json";
        public static string ServerUrl = V;
        private static string _internalServerUrl = V;

        public static void DownloadAndImportAssetFromServer(string assetName)
        {
            if (File.Exists(TheBlackArmsSettings.GetAssetPath() + assetName))
            {
                TheBlackArmsLog(assetName + " exists. Importing it..");
                ImportDownloadedAsset(assetName);
            }
            else
            {
                TheBlackArmsLog(assetName + " does not exist. Starting download..");
                DownloadFile(assetName);
            }
        }

        private static void DownloadFile(string assetName)
        {
            WebClient w = new WebClient();
            w.Headers.Set(HttpRequestHeader.UserAgent, "Webkit Gecko wHTTPS (Keep Alive 55)");
            w.QueryString.Add("assetName", assetName);
            w.DownloadFileCompleted += FileDownloadCompleted;
            w.DownloadProgressChanged += FileDownloadProgress;
            string url = ServerUrl + assetName;
            w.DownloadFileAsync(new Uri(url), TheBlackArmsSettings.GetAssetPath() + assetName);
        }

        public static void DeleteAsset(string assetName)
        {
            File.Delete(TheBlackArmsSettings.GetAssetPath() + assetName);
        }

        public static void UpdateConfig()
        {
            WebClient w = new WebClient();
            w.Headers.Set(HttpRequestHeader.UserAgent, "Webkit Gecko wHTTPS (Keep Alive 55)");
            w.DownloadFileCompleted += ConfigDownloadCompleted;
            w.DownloadProgressChanged += FileDownloadProgress;
            string url = _internalServerUrl + ConfigName;
            w.DownloadFileAsync(new Uri(url), TheBlackArmsSettings.ProjectConfigPath + "update_" + ConfigName);
        }

        private static void ConfigDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                //var updateFile = File.ReadAllText(TheBlackArms_Settings.projectConfigPath + "update_" + configName);
                File.Delete(TheBlackArmsSettings.ProjectConfigPath + ConfigName);
                File.Move(TheBlackArmsSettings.ProjectConfigPath + "update_" + ConfigName,
                    TheBlackArmsSettings.ProjectConfigPath + ConfigName);
                TheBlackArmsImportPanel.LoadJson();

                EditorPrefs.SetInt("TheBlackArms_configImportLastUpdated",
                    (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                TheBlackArmsLog("Import Config has been updated!");
            }
            else
            {
                TheBlackArmsLog("Import Config could not be updated!");
            }
        }

        private static void FileDownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string assetName = ((WebClient)sender).QueryString["assetName"];
            if (e.Error == null)
            {
                TheBlackArmsLog("Download of file " + assetName + " completed!");
            }
            else
            {
                DeleteAsset(assetName);
                TheBlackArmsLog("Download of file " + assetName + " failed!");
            }
        }

        private static void FileDownloadProgress(object sender, DownloadProgressChangedEventArgs e)
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

        public static void CheckForConfigUpdate()
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
            UpdateConfig();
        }

        private static void TheBlackArmsLog(string message)
        {
            Debug.Log("[TheBlackArms] AssetDownloadManager: " + message);
        }

        private static void ImportDownloadedAsset(string assetName)
        {
            AssetDatabase.ImportPackage(TheBlackArmsSettings.GetAssetPath() + assetName, true);
        }
    }
}