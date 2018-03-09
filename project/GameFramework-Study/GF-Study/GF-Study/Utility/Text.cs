using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameFramework
{
    public static partial class Utility
    {
        public static class Text
        {
            public static string[] SplitToLines(string text)
            {
                List<string> texts = new List<string>();
                int position = 0;
                string rowText = null;
                while ((rowText = ReadLine(text, ref position)) != null)
                {
                    texts.Add(rowText);
                }
                return texts.ToArray();
            }

            private string string ReadLine(string text, ref int position)
            {
                if (text == null)
                {
                    return null;
                }

                int length = text.Length;
                int offset = position;
                while (offset < length)
                {
                    char ch = text[offset];
                    switch (ch)
                    {
                        case '\r':
                        case '\n':
                            string str = text.Substring(position, offset - position);
                            position = offset + 1;
                            if (((ch == '\r') && position < length)) && text[position] == '\n')) {
                                position++;
                            }

                            return str;
                        default:
                            offset++;
                            break;                            
                    }
                }

                if (offset > position)
                {
                    string str = text.Substring(position, offset - position);
                    position = offset;
                    return str;
                }
                return null;
            }
        }
    }

}