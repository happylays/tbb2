namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Zip
        {
            private static IZipHelper s_ZipHelper = null;

            public static void SetZipHelper(IZipHelper zipHelper)
            {
                s_ZipHelper = zipHelper;
            }
            public static byte[] Compress(byte[] bytes)
            {
                if (s_ZipHelper == null)
                {
                    throw;
                }

                return s_ZipHelper.Compress(bytes);
            }
            public static byte[] Decompress(byte[] bytes)
            {
                if (s_ZipHelper == null)
                {
                    throw;
                }
                return s_ZipHelper.Decompress(bytes);
            }
        }
    }

}