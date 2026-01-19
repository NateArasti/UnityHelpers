using System;
using System.Collections.Generic;
using UnityEngine;

namespace HelperFunctions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Checks if layer mask has layer enabled
        /// </summary>
        public static bool HasLayer(this LayerMask mask, int layer)
        {
            return (mask.value & (1 << layer)) != 0;
        }

        /// <summary>
        /// Adds layer to layer mask
        /// </summary>
        public static LayerMask AddLayer(this LayerMask mask, int layer)
        {
            mask.value |= 1 << layer;
            return mask;
        }

        /// <summary>
        /// Removes layer from layer mask
        /// </summary>
        public static LayerMask RemoveLayer(this LayerMask mask, int layer)
        {
            mask.value &= ~(1 << layer);
            return mask;
        }

        /// <summary>
        /// Counts triangles count in mesh
        /// </summary>
        public static int CountTriangles(this Mesh mesh)
        {
            var trianglesCount = 0;

            var subMeshCount = mesh.subMeshCount;

            for (var i = 0; i < subMeshCount; i++)
            {
                trianglesCount += mesh.GetTriangles(i).Length / 3;
            }

            return trianglesCount;
        }

        /// <summary>
        /// Calculates volume of Bounds
        /// </summary>
        public static double GetVolume(this Bounds bounds)
        {
            var size = bounds.size;
            return (double)size.x * size.y * size.z;
        }

        /// <summary>
        /// Calculates global bounds for all objects
        /// </summary>
        public static Bounds GetGlobalBounds<T>(IEnumerable<T> objects, Func<T, Bounds> getObjBounds)
        {
            Bounds bounds = default;
            foreach (var obj in objects)
            {
                if (bounds == default)
                {
                    bounds = getObjBounds(obj);
                }
                else
                {
                    bounds.Encapsulate(getObjBounds(obj));
                }
            }
            return bounds;
        }

        /// <summary>
        /// Checks transparency on renderer
        /// </summary>
        public static bool HasTransparency(this Renderer renderer)
        {
            return HasTransparency(renderer.sharedMaterials);
        }

        /// <summary>
        /// Checks transparency in materials
        /// </summary>
        public static bool HasTransparency(this Material[] materials)
        {
            foreach (var material in materials)
            {
                if (material != null && material.color.a < 1) // render queue doesn't work in dedicated server, so hacking...
                {
                    return true;
                }
            }

            return false;
        }
    }
}
