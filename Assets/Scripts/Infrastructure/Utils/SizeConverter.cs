namespace FormForge.Utils
{
    /// <summary>
    /// A utility class for converting between different data sizes such as
    /// bytes, kilobytes, megabytes, and gigabytes.
    /// </summary>
    public static class SizeConverter
    {
        #region Bytes Conversion

        /// <summary>
        /// Converts a size in bytes to kilobytes.
        /// </summary>
        /// <param name="bytes">The size in bytes.</param>
        /// <returns>The size in kilobytes.</returns>
        public static float BytesToKilobytes(long bytes)
        {
            const float bytesInKilobyte = 1024f;
            return bytes / bytesInKilobyte;
        }

        /// <summary>
        /// Converts a size in bytes to megabytes.
        /// </summary>
        /// <param name="bytes">The size in bytes.</param>
        /// <returns>The size in megabytes.</returns>
        public static float BytesToMegabytes(long bytes)
        {
            const float bytesInMegabyte = 1024f * 1024f;
            return bytes / bytesInMegabyte;
        }

        /// <summary>
        /// Converts a size in bytes to gigabytes.
        /// </summary>
        /// <param name="bytes">The size in bytes.</param>
        /// <returns>The size in gigabytes.</returns>
        public static float BytesToGigabytes(long bytes)
        {
            const float bytesInGigabyte = 1024f * 1024f * 1024f;
            return bytes / bytesInGigabyte;
        }

        #endregion

        #region Kilobytes Conversion

        /// <summary>
        /// Converts a size in kilobytes to bytes.
        /// </summary>
        /// <param name="kilobytes">The size in kilobytes.</param>
        /// <returns>The size in bytes.</returns>
        public static long KilobytesToBytes(float kilobytes)
        {
            const long bytesInKilobyte = 1024L;
            return (long)(kilobytes * bytesInKilobyte);
        }

        /// <summary>
        /// Converts a size in kilobytes to megabytes.
        /// </summary>
        /// <param name="kilobytes">The size in kilobytes.</param>
        /// <returns>The size in megabytes.</returns>
        public static float KilobytesToMegabytes(float kilobytes)
        {
            const float kilobytesInMegabyte = 1024f;
            return kilobytes / kilobytesInMegabyte;
        }

        /// <summary>
        /// Converts a size in kilobytes to gigabytes.
        /// </summary>
        /// <param name="kilobytes">The size in kilobytes.</param>
        /// <returns>The size in gigabytes.</returns>
        public static float KilobytesToGigabytes(float kilobytes)
        {
            const float kilobytesInGigabyte = 1024f * 1024f;
            return kilobytes / kilobytesInGigabyte;
        }

        #endregion

        #region Megabytes Conversion

        /// <summary>
        /// Converts a size in megabytes to bytes.
        /// </summary>
        /// <param name="megabytes">The size in megabytes.</param>
        /// <returns>The size in bytes.</returns>
        public static long MegabytesToBytes(float megabytes)
        {
            const long bytesInMegabyte = 1024L * 1024L;
            return (long)(megabytes * bytesInMegabyte);
        }

        /// <summary>
        /// Converts a size in megabytes to kilobytes.
        /// </summary>
        /// <param name="megabytes">The size in megabytes.</param>
        /// <returns>The size in kilobytes.</returns>
        public static float MegabytesToKilobytes(float megabytes)
        {
            const float megabytesInKilobyte = 1024f;
            return megabytes * megabytesInKilobyte;
        }

        /// <summary>
        /// Converts a size in megabytes to gigabytes.
        /// </summary>
        /// <param name="megabytes">The size in megabytes.</param>
        /// <returns>The size in gigabytes.</returns>
        public static float MegabytesToGigabytes(float megabytes)
        {
            const float megabytesInGigabyte = 1024f;
            return megabytes / megabytesInGigabyte;
        }

        #endregion

        #region Gigabytes Conversion

        /// <summary>
        /// Converts a size in gigabytes to bytes.
        /// </summary>
        /// <param name="gigabytes">The size in gigabytes.</param>
        /// <returns>The size in bytes.</returns>
        public static long GigabytesToBytes(float gigabytes)
        {
            const long bytesInGigabyte = 1024L * 1024L * 1024L;
            return (long)(gigabytes * bytesInGigabyte);
        }

        /// <summary>
        /// Converts a size in gigabytes to kilobytes.
        /// </summary>
        /// <param name="gigabytes">The size in gigabytes.</param>
        /// <returns>The size in kilobytes.</returns>
        public static float GigabytesToKilobytes(float gigabytes)
        {
            const float gigabytesInKilobyte = 1024f * 1024f;
            return gigabytes * gigabytesInKilobyte;
        }

        /// <summary>
        /// Converts a size in gigabytes to megabytes.
        /// </summary>
        /// <param name="gigabytes">The size in gigabytes.</param>
        /// <returns>The size in megabytes.</returns>
        public static float GigabytesToMegabytes(float gigabytes)
        {
            const float gigabytesInMegabyte = 1024f;
            return gigabytes * gigabytesInMegabyte;
        }

        #endregion
    }
}