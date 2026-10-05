using System;
using System.Linq;
using JetBrains.Annotations;

namespace OpenTabletDriver.Desktop
{
    [PublicAPI]
    public enum DeviceOrientation
    {
        Up,
        Right,
        Down,
        Left,
    }

    [PublicAPI]
    public static class Helpers
    {
        /// <summary>
        /// Distribute <paramref name="count"/> into buckets with max value of <paramref name="maxValuePerBucket"/>.
        /// Useful for distributing a number of elements into rows and columns.
        /// </summary>
        /// <param name="count">Amount to split into buckets</param>
        /// <param name="maxValuePerBucket">Maximum amount per bucket</param>
        /// <returns>Buckets with the amount of count to take for each element</returns>
        public static int[] SplitIntoBuckets(int count, int maxValuePerBucket = 4)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(count);
            if (count <= maxValuePerBucket) return [count];

            int bucketCount = (int)Math.Ceiling((double)count / maxValuePerBucket);

            // initialize number of elements
            int[] rv = Enumerable.Repeat(0, bucketCount).ToArray();

            int remaining = count;
            int index = 0;
            while (remaining-- > 0)
                rv[index++ % bucketCount] += 1;

            return rv;
        }

        /// <summary>
        /// Get the devices 90-degree rotation based on the provided rotation
        /// </summary>
        /// <param name="rot">Current rotation</param>
        /// <param name="compensated">Remaining rotation that the consumer should compensate for</param>
        /// <returns>The direction in which the underlying device is pointing</returns>
        public static DeviceOrientation CompensateRotation(float rot, out float compensated)
        {
            rot %= 360;
            var orientation = (DeviceOrientation)(Math.Round(rot / 90.0) % 4);
            compensated = rot - (int)orientation * 90;
            compensated = compensated < 0 ? 360 + compensated : compensated;
            return orientation;
        }
    }
}
