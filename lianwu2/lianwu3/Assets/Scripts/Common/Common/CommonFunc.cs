using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.Text.RegularExpressions;
using System.Collections;

namespace LoveDance.Client.Common
{
	public class CommonFunc
	{
		private static DateTime s_UnixStartTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));

        /**支持汉语、韩语、日文等**/
        public static List<byte[]> GetString2Bytes(string strVal, Encoding charset)
        {
            List<byte[]> list = new List<byte[]>();

            if (Encoding.UTF8.Equals(charset))
            {
                byte[] b = charset.GetBytes(strVal);
                int length = b.Length;

                int index = 0;
                while (index < length)
                {
                    int split = 0;

                    byte val0 = b[index];
                    if (val0 > 0x7F)
                    {
                        if (val0 >= 0xC0 && val0 <= 0xDF)
                        {
                            split = 2;
                        }
                        else if (val0 >= 0xE0 && val0 <= 0xEF)
                        {
                            split = 3;
                        }
                        else if (val0 >= 0xF0 && val0 <= 0xF7)
                        {
                            split = 4;
                        }
                        else if (val0 >= 0xF8 && val0 <= 0xFB)
                        {
                            split = 5;
                        }
                        else if (val0 >= 0xFC)
                        {
                            split = 6;
                        }
                        else
                        {
                            split = 1;
                        }
                    }
                    else
                    {
                        split = 1;
                    }

                    byte[] dest = new byte[split];
                    Array.Copy(b, index, dest, 0, dest.Length);
                    list.Add(dest);

                    index = index + split;
                }
            }
            else
            {
                Debug.LogError("UnKnown charset encoding," + charset);
            }

            return list;
        }

        public static List<Transform> GetChildren(Transform transParent, bool includeInactive)
        {
            List<Transform> res = new List<Transform>();

            Transform trans = null;
            for (int i = 0; i < transParent.childCount; ++i)
            {
                trans = transParent.GetChild(i);
                if (trans != null)
                {
                    if (includeInactive)
                    {
                        res.Add(trans);
                    }
                    else
                    {
                        if (trans.gameObject.activeInHierarchy)
                        {
                            res.Add(trans);
                        }
                    }
                }
            }

            return res;
        }

        public static int SortTransformByName(Transform a, Transform b)
        {
            return string.Compare(a.name, b.name);
        }

        public static Encoding GetCharsetEncoding()
        {
            return Encoding.GetEncoding("UTF-8");
        }
	}
}