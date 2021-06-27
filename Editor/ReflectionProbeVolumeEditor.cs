using UnityEditor;
using UnityEngine;

namespace Agoxandr.Utils
{
    public class ReflectionProbeVolumeEditor : MonoBehaviour
    {
        [CustomEditor(typeof(ReflectionProbeVolume), true)]
        public class EventManagerEditor : Editor
        {
            private SerializedProperty resolution;
            private SerializedProperty offset;
            private SerializedProperty threshold;

            private void OnEnable()
            {
                resolution = serializedObject.FindProperty("resolution");
                offset = serializedObject.FindProperty("offset");
                threshold = serializedObject.FindProperty("threshold");
            }

            public override void OnInspectorGUI()
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(resolution);
                EditorGUILayout.PropertyField(offset);
                EditorGUILayout.PropertyField(threshold);
                serializedObject.ApplyModifiedProperties();
                EditorGUILayout.Space();
                if (GUILayout.Button("Place"))
                {
                    Place();
                }
            }

            private void Place()
            {
                var voxelSize = this.resolution.intValue;
                var offset = this.offset.floatValue;
                var threshold = this.threshold.floatValue;
                ReflectionProbeVolume reflectionProbeVolume = (ReflectionProbeVolume)target;
                var transform = reflectionProbeVolume.transform;
                //Gizmos.DrawWireCube(transform.position, transform.localScale);
                var startPos = transform.position + transform.right * transform.localScale.x / 2 + transform.up * transform.localScale.y / 2 + transform.forward * transform.localScale.z / 2;
                var xVar = transform.localScale.x / voxelSize / 2f;
                var zVar = transform.localScale.z / voxelSize / 2f;
                var data = new Vector3[voxelSize, voxelSize];
                //Gizmos.color = Color.blue;
                for (int x = 0; x < voxelSize; x++)
                {
                    for (int z = 0; z < voxelSize; z++)
                    {
                        var rayPos = startPos - transform.right * xVar - transform.forward * zVar;
                        if (Physics.Raycast(rayPos, Vector2.down, out RaycastHit hit))
                        {
                            var point = hit.point + Vector3.up * offset;
                            data[x, z] = point;
                        }
                        zVar += transform.localScale.z / voxelSize;
                    }
                    xVar += transform.localScale.x / voxelSize;
                    zVar = transform.localScale.z / voxelSize / 2f;
                }

                CheckSquare(data, 5, voxelSize, threshold, transform);
                CheckSquare(data, 4, voxelSize, threshold, transform);
                CheckSquare(data, 3, voxelSize, threshold, transform);
                CheckSquare(data, 2, voxelSize, threshold, transform);

                for (int x = 0; x < voxelSize; x++)
                {
                    for (int z = 0; z < voxelSize; z++)
                    {
                        if (data[x, z].x != float.PositiveInfinity)
                        {
                            var go = new GameObject("Reflection Probe Size: 1");
                            go.transform.SetParent(transform);
                            go.transform.position = data[x, z];
                            var reflectionProbe = go.AddComponent(typeof(ReflectionProbe)) as ReflectionProbe;
                            reflectionProbe.resolution = 16;
                            reflectionProbe.center = new Vector3(0f, transform.position.y - data[x, z].y, 0f);
                            reflectionProbe.size = new Vector3(transform.localScale.x / voxelSize, transform.localScale.y, transform.localScale.z / voxelSize);
                        }
                    }
                }
            }

            private void CheckSquare(Vector3[,] data, int size, int voxelSize, float threshold, Transform transform)
            {
                for (int x = 0; x < voxelSize; x++)
                {
                    for (int z = 0; z < voxelSize; z++)
                    {
                        if (x + size <= voxelSize && z + size <= voxelSize)
                        {
                            bool valid = true;
                            var point = Vector3.zero;
                            for (int i = 0; i < size; i++)
                            {
                                for (int k = 0; k < size; k++)
                                {
                                    var pos = data[x + i, z + k];
                                    if (pos != Vector3.positiveInfinity)
                                    {
                                        if (Compare(data[x, z].y, pos.y, threshold))
                                        {
                                            point += pos;

                                        }
                                        else
                                        {
                                            valid = false;
                                        }
                                    }
                                }
                            }
                            if (valid)
                            {
                                point /= size * size;

                                var go = new GameObject("Reflection Probe Size: " + size);
                                go.transform.SetParent(transform);
                                go.transform.position = point;
                                var reflectionProbe = go.AddComponent(typeof(ReflectionProbe)) as ReflectionProbe;
                                if (size == 2) reflectionProbe.resolution = 16;
                                else if (size == 3) reflectionProbe.resolution = 32;
                                else if (size == 4) reflectionProbe.resolution = 32;
                                else if (size == 5) reflectionProbe.resolution = 64;
                                reflectionProbe.center = new Vector3(0f, transform.position.y - point.y, 0f);
                                reflectionProbe.size = new Vector3(transform.localScale.x / voxelSize * size, transform.localScale.y, transform.localScale.z / voxelSize * size);

                                for (int i = 0; i < size; i++)
                                {
                                    for (int k = 0; k < size; k++)
                                    {
                                        data[x + i, z + k] = Vector3.positiveInfinity;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private bool Compare(float arg0, float arg1, float threshold)
            {
                return Mathf.Abs(arg0 - arg1) < threshold;
            }
        }
    } 
}
