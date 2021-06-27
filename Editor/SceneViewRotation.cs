using UnityEditor;
using UnityEngine;

namespace Agoxandr.Utils
{
    [InitializeOnLoad]
    public static class SceneViewRotation
    {
        private static bool toggleDir;

        static SceneViewRotation()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (sceneView.isRotationLocked)
            {
                return;
            }
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Keypad1)
                    {
                        sceneView.rotation = Quaternion.LookRotation(toggleDir ? Vector3.forward : Vector3.back);
                    }
                    else if (e.keyCode == KeyCode.Keypad2)
                    {
                        var angles = sceneView.rotation.eulerAngles;
                        sceneView.rotation = Quaternion.Euler(angles.x - 15f, angles.y, angles.z);
                    }
                    else if (e.keyCode == KeyCode.Keypad3)
                    {
                        sceneView.rotation = Quaternion.LookRotation(toggleDir ? Vector3.left : Vector3.right);
                    }
                    else if (e.keyCode == KeyCode.Keypad4)
                    {
                        var angles = sceneView.rotation.eulerAngles;
                        sceneView.rotation = Quaternion.Euler(angles.x, angles.y + 15f, angles.z);
                    }
                    else if (e.keyCode == KeyCode.Keypad5)
                    {
                        sceneView.orthographic = !sceneView.orthographic;
                    }
                    else if (e.keyCode == KeyCode.Keypad6)
                    {
                        var angles = sceneView.rotation.eulerAngles;
                        sceneView.rotation = Quaternion.Euler(angles.x, angles.y - 15f, angles.z);
                    }
                    else if (e.keyCode == KeyCode.Keypad7)
                    {
                        sceneView.rotation = new Quaternion(0f, toggleDir ? -.7f : .7f, -.7f, 0f);
                    }
                    else if (e.keyCode == KeyCode.Keypad8)
                    {
                        var angles = sceneView.rotation.eulerAngles;
                        sceneView.rotation = Quaternion.Euler(angles.x + 15f, angles.y, angles.z);
                    }
                    //else if (e.keyCode == KeyCode.Keypad9)
                    //{
                    //    //sceneView.rotation = Quaternion.LookRotation(-sceneView.camera.transform.forward);
                    //    sceneView.rotation = Quaternion.Inverse(sceneView.rotation);
                    //}
                    else if (e.keyCode == KeyCode.LeftControl)
                    {
                        toggleDir = true;
                    }
                    break;
                case EventType.KeyUp:
                    if (e.keyCode == KeyCode.LeftControl)
                    {
                        toggleDir = false;
                    }
                    break;
            }
        }
    }
}
