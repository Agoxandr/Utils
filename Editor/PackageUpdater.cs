using System.IO;
using UnityEditor;
using UnityEngine;

namespace Agoxandr.Utils
{
    public class PackgeUpdater
    {
        private static string ManifestPath
        {
            get
            {
                var projectPath = Directory.GetParent(Application.dataPath).FullName;
                var manifestPath = Path.Combine(projectPath, "Packages", "manifest.json");
                return manifestPath;
            }
        }

        public static string GetIdentifier(string input, char charFrom, char charTo)
        {
            var first = input.IndexOf(charFrom);
            var second = input.IndexOf(charTo, first + 1);
            var posFrom = input.IndexOf(charFrom, second + 1);
            if (posFrom != -1) //if found char
            {
                var posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1) //if found char
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }

        [MenuItem("Window/Package Manager/Update Git Packages", false, 1000)]
        private static void UpdateGitPackages()
        {
            var lines = File.ReadAllLines(ManifestPath);
            foreach (var line in lines)
            {
                if (line.Contains("git"))
                {
                    var identifier = GetIdentifier(line, '"', '"');
                    Debug.Log("Checking for updates " + identifier);
                    UnityEditor.PackageManager.Client.Add(identifier);
                }
            }
        }
    }
}
