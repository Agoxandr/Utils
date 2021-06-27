using System.Diagnostics;
using UnityEngine;

namespace Agoxandr.Utils
{
    public class ReflectionProbeVolume : MonoBehaviour
    {
        [Min(1)]
        public int resolution = 4;
        public float offset = 2;
        [Min(0)]
        public float threshold = .5f;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(transform.position, transform.localScale);
            var startPos = transform.position + transform.right * transform.localScale.x / 2 + transform.up * transform.localScale.y / 2 + transform.forward * transform.localScale.z / 2;
            var xVar = transform.localScale.x / resolution / 2f;
            var zVar = transform.localScale.z / resolution / 2f;
            var data = new Vector3[resolution, resolution];
            Gizmos.color = Color.blue;
            for (int x = 0; x < resolution; x++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    var rayPos = startPos - transform.right * xVar - transform.forward * zVar;
                    if (Physics.Raycast(rayPos, Vector2.down, out RaycastHit hit))
                    {
                        var point = hit.point + Vector3.up * offset;
                        data[x, z] = point;
                    }
                    zVar += transform.localScale.z / resolution;
                }
                xVar += transform.localScale.x / resolution;
                zVar = transform.localScale.z / resolution / 2f;
            }

            CheckSquare(data, 5);
            CheckSquare(data, 4);
            CheckSquare(data, 3);
            CheckSquare(data, 2);

            for (int x = 0; x < resolution; x++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    if (data[x, z] != Vector3.positiveInfinity)
                    {
                        Gizmos.DrawSphere(data[x, z], .1f);
                    }
                }
            }
        }

        private void CheckSquare(Vector3[,] data, int size)
        {
            for (int x = 0; x < resolution; x++)
            {
                for (int z = 0; z < resolution; z++)
                {
                    if (x + size <= resolution && z + size <= resolution)
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
                                    if (Compare(data[x, z].y, pos.y))
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
                            Gizmos.DrawSphere(point, size * .1f);
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

        private bool Compare(float arg0, float arg1)
        {
            return Mathf.Abs(arg0 - arg1) < threshold;
        }
    }
}
