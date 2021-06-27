using UnityEditor;
using UnityEngine;

namespace Agoxandr.Utils
{
    [CustomEditor(typeof(EventManager), true)]
    public class EventManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EventManager myTarget = (EventManager)target;
            GUILayout.BeginHorizontal();
            GUILayout.Label("Update");
            if (Application.isPlaying)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(myTarget.UpdateLength().ToString());
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("FixedUpdate");
            if (Application.isPlaying)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(myTarget.FixedUpdateLength().ToString());
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("LateUpdate");
            if (Application.isPlaying)
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label(myTarget.LateUpdateLength().ToString());
            }
            GUILayout.EndHorizontal();
        }
    }
}
