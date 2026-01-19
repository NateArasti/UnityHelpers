using NUnit.Framework;
using UnityEngine;

namespace HelperFunctions.Tests.Editor
{
    public class VectorExtensionsTests
    {
        [Test]
        public void ToXZVector2_MapsXZCorrectly()
        {
            var v = new Vector3(1f, 2f, 3f);
            var result = v.ToXZVector2();
            Assert.AreEqual(new Vector2(1f, 3f), result);
        }

        [Test]
        public void ToYZVector2_MapsYZCorrectly()
        {
            var v = new Vector3(1f, 2f, 3f);
            var result = v.ToYZVector2();
            Assert.AreEqual(new Vector2(2f, 3f), result);
        }

        [Test]
        public void GetXY_ReturnsCorrectVector()
        {
            var v = new Vector3(1f, 2f, 3f);
            var result = v.GetXY(5f);
            Assert.AreEqual(new Vector3(1f, 2f, 5f), result);
        }

        [Test]
        public void GetXZ_ReturnsCorrectVector()
        {
            var v = new Vector3(1f, 2f, 3f);
            var result = v.GetXZ(9f);
            Assert.AreEqual(new Vector3(1f, 9f, 3f), result);
        }

        [Test]
        public void GetYZ_ReturnsCorrectVector()
        {
            var v = new Vector3(1f, 2f, 3f);
            var result = v.GetYZ(7f);
            Assert.AreEqual(new Vector3(7f, 2f, 3f), result);
        }

        [Test]
        public void WithX_Y_Z_SetsComponentsCorrectly()
        {
            var v = new Vector3(1f, 2f, 3f);
            Assert.AreEqual(new Vector3(10f, 2f, 3f), v.WithX(10f));
            Assert.AreEqual(new Vector3(1f, 20f, 3f), v.WithY(20f));
            Assert.AreEqual(new Vector3(1f, 2f, 30f), v.WithZ(30f));
        }

        [Test]
        public void AddX_Y_Z_AddsComponentsCorrectly()
        {
            var v = new Vector3(1f, 2f, 3f);
            Assert.AreEqual(new Vector3(11f, 2f, 3f), v.AddX(10f));
            Assert.AreEqual(new Vector3(1f, 12f, 3f), v.AddY(10f));
            Assert.AreEqual(new Vector3(1f, 2f, 13f), v.AddZ(10f));
        }

        [Test]
        public void ToXZVector3_CreatesVectorCorrectly()
        {
            var v = new Vector2(4f, 5f);
            var result = v.ToXZVector3(2f);
            Assert.AreEqual(new Vector3(4f, 2f, 5f), result);
        }

        [Test]
        public void ToXYVector3_CreatesVectorCorrectly()
        {
            var v = new Vector2(4f, 5f);
            var result = v.ToXYVector3(3f);
            Assert.AreEqual(new Vector3(4f, 5f, 3f), result);
        }

        [Test]
        public void Clamp_Vector3_ClampsComponents()
        {
            var v = new Vector3(-1f, 5f, 10f);
            var result = v.Clamp(Vector3.zero, Vector3.one * 6f);
            Assert.AreEqual(new Vector3(0f, 5f, 6f), result);
        }

        [Test]
        public void Clamp_Vector2_ClampsComponents()
        {
            var v = new Vector2(-1f, 5f);
            var result = v.Clamp(Vector2.zero, Vector2.one * 4f);
            Assert.AreEqual(new Vector2(0f, 4f), result);
        }

        [Test]
        public void Abs_Vector3AndVector2_ReturnsAbsoluteValues()
        {
            var v3 = new Vector3(-1f, -2f, 3f).Abs();
            Assert.AreEqual(new Vector3(1f, 2f, 3f), v3);

            var v2 = new Vector2(-4f, 5f).Abs();
            Assert.AreEqual(new Vector2(4f, 5f), v2);
        }

        [Test]
        public void Multiply_Divide_ComponentWise()
        {
            var a = new Vector3(2f, 3f, 4f);
            var b = new Vector3(5f, 2f, 1f);

            Assert.AreEqual(new Vector3(10f, 6f, 4f), a.Multiply(b));
            Assert.AreEqual(new Vector3(0.4f, 1.5f, 4f), a.Divide(b));
        }

        [Test]
        public void DirectionTo_ReturnsNormalizedVector()
        {
            var from = new Vector3(0f, 0f, 0f);
            var to = new Vector3(3f, 0f, 4f);
            var dir = from.DirectionTo(to);

            Assert.AreEqual(new Vector3(0.6f, 0f, 0.8f), dir);
            Assert.AreEqual(1f, dir.magnitude, 1e-4f);
        }

        [Test]
        public void ClosestPointOnLine_ReturnsCorrectPoint()
        {
            var start = new Vector3(0f, 0f, 0f);
            var end = new Vector3(10f, 0f, 0f);

            // point in the middle
            var point = new Vector3(5f, 5f, 0f);
            var closest = point.ClosestPointOnLine(start, end);
            Assert.AreEqual(new Vector3(5f, 0f, 0f), closest);

            // point beyond line
            point = new Vector3(-5f, 0f, 0f);
            closest = point.ClosestPointOnLine(start, end);
            Assert.AreEqual(start, closest);

            point = new Vector3(15f, 0f, 0f);
            closest = point.ClosestPointOnLine(start, end);
            Assert.AreEqual(end, closest);
        }

        [Test]
        public void Approximately_ReturnsTrueForCloseVectors()
        {
            const float tolerance = 0.0001f;

            var a = new Vector3(1f, 2f, 3f);
            var b = new Vector3(1f + tolerance / 2f, 2f + tolerance / 2f, 3f + tolerance / 2f);

            // Should be true because difference is within tolerance
            Assert.IsTrue(a.Approximately(b, tolerance));

            // Should be false if difference is bigger than tolerance
            b = new Vector3(1f + tolerance * 2f, 2f + tolerance * 2f, 3f + tolerance * 2f);
            Assert.IsFalse(a.Approximately(b, tolerance));
        }
    }
}
