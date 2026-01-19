using NUnit.Framework;
using UnityEngine;

namespace HelperFunctions.Tests.Editor
{
    public class MathExtensionsTests
    {
        [Test]
        public void ApproximatelyEqual_ShouldReturnTrue_WhenValuesWithinTolerance()
        {
            Assert.IsTrue(MathExtensions.ApproximatelyEqual(1f, 1.0005f));
        }

        [Test]
        public void ApproximatelyEqual_ShouldReturnFalse_WhenValuesExceedTolerance()
        {
            Assert.IsFalse(MathExtensions.ApproximatelyEqual(1f, 1.01f));
        }

        [Test]
        public void RoundValueToStep_ShouldRoundToNearestStep()
        {
            Assert.AreEqual(10, MathExtensions.RoundValueToStep(11f, 5f));
            Assert.AreEqual(15, MathExtensions.RoundValueToStep(13f, 5f));
        }

        [Test]
        public void RoundValueToStep_ShouldThrowException_WhenStepIsZeroOrNegative()
        {
            Assert.Throws<UnityException>(() => MathExtensions.RoundValueToStep(10f, 0f));
            Assert.Throws<UnityException>(() => MathExtensions.RoundValueToStep(10f, -2f));
        }

        [Test]
        public void AddValueToAverage_ShouldReturnCorrectAverage()
        {
            float avg = MathExtensions.AddValueToAverage(5f, 7f, 2f); // (5*2+7)/3 = 5.6666
            Assert.AreEqual(5.666666f, avg, 0.0001f);
        }

        [Test]
        public void Remap_ShouldMapValueCorrectly()
        {
            float value = MathExtensions.Remap(5f, 0f, 10f, 0f, 100f);
            Assert.AreEqual(50f, value, 0.0001f);
        }

        [Test]
        public void Remap_Vector2Overload_ShouldMapValueCorrectly()
        {
            float value = MathExtensions.Remap(5f, new Vector2(0f,10f), new Vector2(0f,100f));
            Assert.AreEqual(50f, value, 0.0001f);
        }

        [Test]
        public void Wrap_ShouldWrapValueWithinRange()
        {
            Assert.AreEqual(2f, MathExtensions.Wrap(12f, 0f, 10f), 0.0001f);
            Assert.AreEqual(8f, MathExtensions.Wrap(-2f, 0f, 10f), 0.0001f);
        }

        [Test]
        public void ExpDecay_ShouldDecayFloatCorrectly()
        {
            float result = MathExtensions.ExpDecay(10f, 0f, 1f, 1f);
            Assert.AreEqual(0f + (10f - 0f) * Mathf.Exp(-1f * 1f), result, 0.0001f);
        }

        [Test]
        public void ExpDecay_ShouldDecayVector3Correctly()
        {
            Vector3 current = new Vector3(10, 10, 10);
            Vector3 target = Vector3.zero;
            Vector3 result = MathExtensions.ExpDecay(current, target, 1f, 1f);
            Assert.AreEqual(target + (current - target) * Mathf.Exp(-1f * 1f), result);
        }

        [Test]
        public void Normalize_ShouldReturnNormalizedValue()
        {
            float result = MathExtensions.Normalize(5f, 0f, 10f);
            Assert.AreEqual(0.5f, result, 0.0001f);
        }

        [Test]
        public void GetPercentage_ShouldReturnPercentage()
        {
            float result = MathExtensions.GetPercentage(5f, 0f, 10f);
            Assert.AreEqual(50f, result, 0.0001f);
        }

        [Test]
        public void InRange_ShouldDetectInclusiveRange()
        {
            Assert.IsTrue(MathExtensions.InRange(5f, 0f, 5f));
            Assert.IsFalse(MathExtensions.InRange(6f, 0f, 5f));
        }

        [Test]
        public void InRangeExclusive_ShouldDetectExclusiveRange()
        {
            Assert.IsTrue(MathExtensions.InRangeExclusive(4f, 0f, 5f));
            Assert.IsFalse(MathExtensions.InRangeExclusive(0f, 0f, 5f));
            Assert.IsFalse(MathExtensions.InRangeExclusive(5f, 0f, 5f));
        }


        [Test]
        public void SnapToGrid_ShouldSnapCorrectly()
        {
            Vector3 result = MathExtensions.SnapToGrid(new Vector3(1.3f, 2.7f, -1.1f), 1f);
            Assert.AreEqual(new Vector3(1f, 3f, -1f), result);
        }

        [Test]
        public void Average_ShouldReturnCorrectAverage()
        {
            float result = MathExtensions.Average(1f, 2f, 3f, 4f);
            Assert.AreEqual(2.5f, result, 0.0001f);
        }

        [Test]
        public void Average_ShouldReturnZero_WhenNoValues()
        {
            float result = MathExtensions.Average();
            Assert.AreEqual(0f, result);
        }

        [Test]
        public void RemapClamped_ShouldClampValueCorrectly()
        {
            float result = MathExtensions.RemapClamped(15f, 0f, 10f, 0f, 100f);
            Assert.AreEqual(100f, result);
            result = MathExtensions.RemapClamped(-5f, 0f, 10f, 0f, 100f);
            Assert.AreEqual(0f, result);
        }

        [Test]
        public void GetFlatIndex2D_ShouldReturnCorrectIndex()
        {
            long index = MathExtensions.GetFlatIndex(2, 3, 10);
            Assert.AreEqual(32, index); // 2 + 10*3
        }

        [Test]
        public void GetFlatIndex3D_ShouldReturnCorrectIndex()
        {
            long index = MathExtensions.GetFlatIndex(1, 2, 3, 10, 10);
            Assert.AreEqual(1 + 10*2 + 10*10*3, index);
        }
    }
}
