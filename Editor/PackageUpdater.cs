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

        public static string GetStringBetweenCharacters(string input, char charFrom, char charTo)
        {
            int posFrom = input.IndexOf(charFrom);
            if (posFrom != -1) //if found char
            {
                int posTo = input.IndexOf(charTo, posFrom + 1);
                if (posTo != -1) //if found char
                {
                    return input.Substring(posFrom + 1, posTo - posFrom - 1);
                }
            }

            return string.Empty;
        }

        [MenuItem("Assets/Update Git Packages", false, 20)]
        private static void UpdateGitPackages()
        {
            var lines = File.ReadAllLines(ManifestPath);
            foreach (var line in lines)
            {
                if (line.Contains("git"))
                {
                    var identifier = GetStringBetweenCharacters(line, '"', '"');
                    Debug.Log("Checking for updates " + identifier);
                    UnityEditor.PackageManager.Client.Add(identifier);
                }
            }
        }
    }
}
