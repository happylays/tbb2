namespace GameFramework
{
    public static partial class Utility
    {
        public static class Nullable
        {
            public static bool IsNullable<T>(T t) { return false; }
            public static bool IsNullable<T>(T? t) where : struct { return true; }
        }
    }

}