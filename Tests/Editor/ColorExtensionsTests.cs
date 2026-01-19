using NUnit.Framework;
using UnityEngine;

namespace HelperFunctions.Tests.Editor
{
    public class ColorExtensionsTests
    {
        private static readonly Color BaseColor = new Color(0.2f, 0.4f, 0.6f, 0.8f);
        private const float Epsilon = 0.0001f;

        [Test]
        public void WithAlpha_ChangesOnlyAlpha()
        {
            const float newAlpha = 0.3f;
            var result = BaseColor.WithAlpha(newAlpha);

            Assert.AreEqual(BaseColor.r, result.r);
            Assert.AreEqual(BaseColor.g, result.g);
            Assert.AreEqual(BaseColor.b, result.b);
            Assert.AreEqual(newAlpha, result.a);
        }

        [Test]
        public void WithRed_ChangesOnlyRed()
        {
            const float newRed = 1f;
            var result = BaseColor.WithRed(newRed);

            Assert.AreEqual(newRed, result.r);
            Assert.AreEqual(BaseColor.g, result.g);
            Assert.AreEqual(BaseColor.b, result.b);
            Assert.AreEqual(BaseColor.a, result.a);
        }

        [Test]
        public void WithGreen_ChangesOnlyGreen()
        {
            const float newGreen = 0f;
            var result = BaseColor.WithGreen(newGreen);

            Assert.AreEqual(BaseColor.r, result.r);
            Assert.AreEqual(newGreen, result.g);
            Assert.AreEqual(BaseColor.b, result.b);
            Assert.AreEqual(BaseColor.a, result.a);
        }

        [Test]
        public void WithBlue_ChangesOnlyBlue()
        {
            const float newBlue = 1f;
            var result = BaseColor.WithBlue(newBlue);

            Assert.AreEqual(BaseColor.r, result.r);
            Assert.AreEqual(BaseColor.g, result.g);
            Assert.AreEqual(newBlue, result.b);
            Assert.AreEqual(BaseColor.a, result.a);
        }

        [Test]
        public void Lighten_ZeroAmount_ReturnsOriginal()
        {
            const float amount = 0f;
            var result = BaseColor.Lighten(amount);
            Assert.AreEqual(BaseColor, result);
        }

        [Test]
        public void Lighten_FullAmount_ReturnsWhitePreservingAlpha()
        {
            const float amount = 1f;
            var result = BaseColor.Lighten(amount);
            Assert.AreEqual(Color.white.WithAlpha(BaseColor.a), result);
        }

        [Test]
        public void Darken_ZeroAmount_ReturnsOriginal()
        {
            const float amount = 0f;
            var result = BaseColor.Darken(amount);
            Assert.AreEqual(BaseColor, result);
        }

        [Test]
        public void Darken_FullAmount_ReturnsBlackPreservingAlpha()
        {
            const float amount = 1f;
            var result = BaseColor.Darken(amount);
            Assert.AreEqual(Color.black.WithAlpha(BaseColor.a), result);
        }

        [Test]
        public void Saturate_IncreasesSaturationAndPreservesAlpha()
        {
            const float amount = 0.5f;
            var gray = Color.gray;
            var result = gray.Saturate(amount);

            Color.RGBToHSV(result, out _, out var s, out _);
            Assert.Greater(s, 0f);
            Assert.AreEqual(gray.a, result.a);
        }

        [Test]
        public void Invert_InvertsRGBAndPreservesAlpha()
        {
            var result = BaseColor.Invert();

            Assert.AreEqual(1f - BaseColor.r, result.r, Epsilon);
            Assert.AreEqual(1f - BaseColor.g, result.g, Epsilon);
            Assert.AreEqual(1f - BaseColor.b, result.b, Epsilon);
            Assert.AreEqual(BaseColor.a, result.a);
        }

        [Test]
        public void ToGrayscale_SetsAllChannelsEqualAndPreservesAlpha()
        {
            var result = BaseColor.ToGrayscale();

            Assert.AreEqual(result.r, result.g, Epsilon);
            Assert.AreEqual(result.g, result.b, Epsilon);
            Assert.AreEqual(BaseColor.a, result.a);
        }

        [Test]
        public void GetContrastingColor_Bright_ReturnsBlack()
        {
            var bright = Color.white;
            Assert.AreEqual(Color.black, bright.GetContrastingColor());
        }

        [Test]
        public void GetContrastingColor_Dark_ReturnsWhite()
        {
            var dark = Color.black;
            Assert.AreEqual(Color.white, dark.GetContrastingColor());
        }

        [Test]
        public void ToHex_WithoutAlpha_ReturnsCorrectString()
        {
            var color = new Color(1f, 0f, 0f, 0.5f);
            Assert.AreEqual("#FF0000", color.ToHex());
        }

        [Test]
        public void ToHex_WithAlpha_ReturnsCorrectString()
        {
            var color = new Color(1f, 0f, 0f, 0.5f);
            Assert.AreEqual("#FF000080", color.ToHex(true));
        }

        [Test]
        public void MultiplyRGB_MultipliesChannelsAndPreservesAlpha()
        {
            const float multiplier = 2f;
            var result = BaseColor.MultiplyRGB(multiplier);

            Assert.AreEqual(BaseColor.r * multiplier, result.r, Epsilon);
            Assert.AreEqual(BaseColor.g * multiplier, result.g, Epsilon);
            Assert.AreEqual(BaseColor.b * multiplier, result.b, Epsilon);
            Assert.AreEqual(BaseColor.a, result.a);
        }
    }
}
