
namespace GameFramework
{
    public static partial class Utility
    {
        public static readonly System.Random s_Random = new System.Random();

        public static int GetRandom()
        {
            return s_Random.Next();
        }

        public static int GetRandom(int maxValue)
        {
            return s_Random.Next(maxValue);
        }

        public static void GetRandomBytes(byte[] buffer)
        {
            s_Random.NextBytes(buffer);
        }

    }
}