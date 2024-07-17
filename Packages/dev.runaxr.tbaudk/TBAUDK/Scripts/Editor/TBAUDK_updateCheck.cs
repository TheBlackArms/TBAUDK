using System.IO;
using System.Net.Http;
using TheBlackArms;
using UnityEditor;
using UnityEngine;

public class TheBlackArms_updateCheck : MonoBehaviour
{
    [InitializeOnLoad]
    public class Startup
    {
        public static string versionURL = "https://c0dera.in/TBAUDK/api/version.txt";

        public static string currentVersion =
            File.ReadAllText("Packages/dev.runaxr.tbaudk/TBAUDKUpdater/Editor/TBAUDKversion.txt");

        static Startup()
        {
            Check();
        }

        public async static void Check()
        {
            HttpClient httpClient = new HttpClient();
            var result = await httpClient.GetAsync(versionURL);
            var strServerVersion = await result.Content.ReadAsStringAsync();
            var serverVersion = strServerVersion;

            var thisVersion = currentVersion;

            if (serverVersion != thisVersion)
            {
                TheBlackArms_AutomaticUpdateAndInstall.AutomaticTBAUDKInstaller();
            }
        }
    }
}