using NUnit.Framework;

namespace HelperFunctions.Tests.Editor
{
    public class FormatExtensionsTests
    {
        [Test]
        public void FormatBytes_ZeroBytes_ReturnsB()
        {
            const long bytes = 0;
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("0 B", result);
        }

        [Test]
        public void FormatBytes_LessThanKB_ReturnsB()
        {
            const long bytes = 512;
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("512 B", result);
        }

        [Test]
        public void FormatBytes_ExactlyOneKB_ReturnsKB()
        {
            const long bytes = 1024;
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("1 KB", result);
        }

        [Test]
        public void FormatBytes_KBRange_ReturnsKB()
        {
            const long bytes = 5 * 1024 + 200; // 5.2 KB
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("5.2 KB", result);
        }

        [Test]
        public void FormatBytes_MBRange_ReturnsMB()
        {
            const long bytes = 3 * 1024 * 1024 + 500_000; // ~3.48 MB
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("3.48 MB", result);
        }

        [Test]
        public void FormatBytes_GBRange_ReturnsGB()
        {
            const long bytes = 7L * 1024 * 1024 * 1024 + 200_000_000; // ~7.19 GB
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("7.19 GB", result);
        }

        [Test]
        public void FormatBytes_NegativeValue_ClampedToZero()
        {
            const long bytes = -1234;
            var result = FormatExtensions.FormatBytes(bytes);
            Assert.AreEqual("0 B", result);
        }

        [Test]
        public void FormatSeconds_LessThanOneMinute_ReturnsMMSS()
        {
            const float seconds = 42f;
            var result = FormatExtensions.FormatSeconds(seconds);
            Assert.AreEqual("00:42", result);
        }

        [Test]
        public void FormatSeconds_ExactMinutes_ReturnsMMSS()
        {
            const float seconds = 180f; // 3 min
            var result = FormatExtensions.FormatSeconds(seconds);
            Assert.AreEqual("03:00", result);
        }

        [Test]
        public void FormatSeconds_HoursAndMinutes_ReturnsHHMMSS()
        {
            const float seconds = 3671f; // 1 h 1 min 11 sec
            var result = FormatExtensions.FormatSeconds(seconds);
            Assert.AreEqual("01:01:11", result);
        }

        [Test]
        public void FormatSeconds_NegativeValue_ClampedToZero()
        {
            const float seconds = -50f;
            var result = FormatExtensions.FormatSeconds(seconds);
            Assert.AreEqual("00:00", result);
        }

        [Test]
        public void FormatSeconds_ExactlyOneHour_ReturnsHHMMSS()
        {
            const float seconds = 3600f;
            var result = FormatExtensions.FormatSeconds(seconds);
            Assert.AreEqual("01:00:00", result);
        }
    }
}
