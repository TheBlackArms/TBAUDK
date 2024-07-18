using System.IO;
using System.Net.Http;
using TheBlackArms;
using UnityEditor;
using UnityEngine;

public class TheBlackArmsUpdateCheck : MonoBehaviour
{
    [InitializeOnLoad]
    public class Startup
    {
        private static string _versionURL = "https://c0dera.in/TBAUDK/api/version.txt";

        private static string _currentVersion =
            File.ReadAllText("Packages/dev.runaxr.tbaudk/TBAUDKUpdater/Editor/TBAUDKversion.txt");

        static Startup()
        {
            Check();
        }

        private async static void Check()
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetAsync(_versionURL);
            var strServerVersion = await result.Content.ReadAsStringAsync();
            var serverVersion = strServerVersion;

            var thisVersion = _currentVersion;

            if (serverVersion != thisVersion)
            {
                TheBlackArmsAutomaticUpdateAndInstall.AutomaticTbaudkInstaller();
            }
        }
    }
}