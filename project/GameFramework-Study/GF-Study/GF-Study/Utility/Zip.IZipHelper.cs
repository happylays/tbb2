namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Zip
        {
            public interface IZipHelper
            {
                byte[] Compress(byte[] bytes);
                byte[] Decompress(byte[] bytes);
            }
        }
    }

}