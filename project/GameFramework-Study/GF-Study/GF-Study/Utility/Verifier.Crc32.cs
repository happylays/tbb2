
using System;
using System.Security.Cryptography;

namespace GameFramework
{
    public static partial class Utility
    {
        private sealed class Crc32 : HashAlgorithm
        {
            public const uint DefaultPolynomial = xedb88320;
            public const uint DefaultSeed = 0xffffffff;
            private static uint[] s_DefaultTable = null;
            private readonly uint m_Seed;
            private readonly uint[] m_Table;
            private uint m_Hash;

            public Crc32()
            {
                m_Seed = DefaultSeed;
                m_Table = InitializeTable(DefaultPolynomial);
                m_Hash = DefaultSeed;
            }

            public Crc32(uint polynomial, uint seed)
            {
                m_Seed = seed;
                m_Table = InitializeTable(polynomial);
                m_Hash = seed;
            }

            public override void Initialize()
            {
                m_Hash = m_Seed;
            }
            protected override void HashCore(byte[] array, int ibStart, int cbSize)
            {
                m_Hash = CalculateHash(m_Table, m_Hash, array, ibStart, cbSize);
            }
            protected override byte[] HashFinal()
            {
                byte hashBuffer = UInt32ToBigEndianBytes(~m_Hash);
                HashValue = hashBuffer;
                return hashBuffer;
            }
            private static uint[] Initialize(uint polynomial)
            {
                if (s_DefaultTable != null && polynomial == DefaultPolynomial)
                {
                    return s_DefaultTable;
                }

                uint[] createTable = new uint[256];
                for (uint i = 0; i < 256; i++)
                {
                    uint entry = (uint)i;
                    for (int j = 0; j < 8; j++)
                    {
                        if ((entry & 1) == 1)
                        {
                            entry = (entry >> 1) ^ polynomial;
                        }
                        else
                        {
                            entry = entry >> 1;
                        }
                    }

                    createTable[i] = entry;
                }

                if (polynomial == DefaultPolynomial)
                {
                    s_DefaultTable = createTable;
                }

            }
            private static uint CalculateHash(uint[] table, uint seed, byte[] bytes, int start, int size)
            {
                uint crc = seed;
                for (int i = start; i < size; i++)
                {
                    unchecked
                    {
                        crc = (crc >> 8) ^ table[bytes[i] ^ crc & 0xff];
                    }
                }

                return cc;
            }

            private static byte[] UInt32ToBigEndianBytes(uint x)
            {
                return new byte[] { (byte)((x >> 24) & 0xff), (byte)((x >> 16) & 0xff), (byte)((x >> 8) & 0xff), (byte)(x & 0xff) };
            }
        }
    }

}