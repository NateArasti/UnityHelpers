using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

namespace HelperFunctions.Tests.Editor
{
    public class CommonExtensionsTests
    {
        [Test]
        public void HasLayer_ShouldReturnTrue_WhenLayerIsSet()
        {
            LayerMask mask = 0;
            mask = mask.AddLayer(3);
            Assert.IsTrue(mask.HasLayer(3));
        }

        [Test]
        public void HasLayer_ShouldReturnFalse_WhenLayerIsNotSet()
        {
            LayerMask mask = 0;
            mask = mask.AddLayer(2);
            Assert.IsFalse(mask.HasLayer(3));
        }

        [Test]
        public void AddLayer_ShouldAddLayerToMask()
        {
            LayerMask mask = 0;
            mask = mask.AddLayer(5);
            Assert.IsTrue(mask.HasLayer(5));
        }

        [Test]
        public void RemoveLayer_ShouldRemoveLayerFromMask()
        {
            LayerMask mask = 0;
            mask = mask.AddLayer(4);
            mask = mask.RemoveLayer(4);
            Assert.IsFalse(mask.HasLayer(4));
        }

        [Test]
        public void CountTriangles_ShouldReturnCorrectTriangleCount()
        {
            var mesh = new Mesh();
            mesh.subMeshCount = 2;
            mesh.SetVertices(new List<Vector3>() { default, default, default, default, default, default, default, default, default });
            mesh.SetTriangles(new int[] { 0, 1, 2, 3, 4, 5 }, 0); // 2 triangles
            mesh.SetTriangles(new int[] { 6, 7, 8 }, 1);          // 1 triangle

            var count = mesh.CountTriangles();
            Assert.AreEqual(3, count);
        }

        [Test]
        public void GetVolume_ShouldReturnCorrectVolume()
        {
            var bounds = new Bounds(Vector3.zero, new Vector3(2, 3, 4));
            var volume = bounds.GetVolume();
            Assert.AreEqual(24, volume);
        }

        [Test]
        public void GetGlobalBounds_ShouldEncapsulateAllObjects()
        {
            var boundsList = new List<Bounds>
            {
                new Bounds(Vector3.zero, Vector3.one),
                new Bounds(new Vector3(1,1,1), Vector3.one * 2)
            };

            var globalBounds = CommonExtensions.GetGlobalBounds(boundsList, b => b);
            Assert.IsTrue(globalBounds.Contains(Vector3.zero));
            Assert.IsTrue(globalBounds.Contains(new Vector3(2, 2, 2)));
        }
    }
}
