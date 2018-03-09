
using System;
using System.Text;

namespace GameFramework
{
    public static partial class Utility
    {
        public static class Converter
        {
            private const float InchesToCentimeters = 2.54f;
            private const float CentimetersToInches = 1f / InchesToCentimeters;

            public static bool IsLittleEndian
            {
                get
                {
                    return BitConverter.IsLittleEndian;
                }
            }
            public static float ScreenDpi { get; set; }
            public static float GetCentimetersFromPixels(float pixels) { }
            public static float GetInchesFromPixeds(float pixels)
            {
                if (ScreenDpi <= 0)
                {
                    throw;
                }
                return pixels / ScreenDpi;
            }
            public static byte[] GetBytes(bool value)
            {
                return BitConverter.GetBytes(value);
            }
            public static bool GetBoolean(byte[] value)
            {
                return BitConverter.ToBoolean(value, 0);
            }
            public static bool GetBoolean(byte[], int startIndex)
            {
                return BitConverter.ToBoolean(value, startIndex);
            }
            public static byte[] GetBytes(char value)
            {
                return BitConverter.GetBytes(value);
            }
            public static char GetChar(byte[] value)
            {
                return BitConverter.ToChar(value, 0);
            }
            public static byte[] GetBytes(short value)
            {
                return BitConverter.GetBytes(value);
            }
            public static short GetInt16(byte[] value)
            {
                return BitConverter.ToInt16(value, 0);
            }
            public static byte[] GetBytes(ushort value)
            {
                return BitConverter.GetBytes(value);
            }
            public static ushort GetUInt16(byte[] value)
            {
                return BitConverter.ToUInt16(value, 0);
            }
            public static byte[] GetBytes(int value)
            {
                return BitConverter.GetBytes(value);
            }
            public static short GetInt32(byte[] value)
            {
                return BitConverter.ToInt32(value, 0);
            }
            public static byte[] GetBytes(uint value)
            {
                return BitConverter.GetBytes(value);
            }
            public static uint GetUInt32(byte[] value)
            {
                return BitConverter.ToInt32(value, 0);
            }
            public static byte[] GetBytes(long value)
            {
                return BitConverter.GetBytes(value);
            }
            public static uint GetInt64(byte[] value)
            {
                return BitConverter.ToInt64(value, 0);
            }
            public static byte[] GetBytes(float value)
            {
                return BitConverter.GetBytes(value);
            }
            public static float GetSingle(byte[] value)
            {
                return BitConverter.ToSingle(value, 0);
            }
            public static byte[] GetBytes(double value)
            {
                return BitConverter.GetBytes(value);
            }
            public static double GetDouble(byte[] value)
            {
                return BitConverter.ToDouble(value, 0);
            }
            public static byte[] GetBytes(string value)
            {
                return Encoding.UTF8.GetBytes(value);
            }
            public static string GetString(byte[] value)
            {
                if (value == null)
                {

                }
                return Encoding.UTF8.GetString(value, 0, value.Length);
            }
        }
    }
}