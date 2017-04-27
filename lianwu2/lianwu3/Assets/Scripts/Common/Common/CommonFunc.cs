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

        public static string GetCharsetType()
        {
            return "UTF-8";
        }

        public static Encoding GetCharsetEncoding()
        {
            return Encoding.GetEncoding("UTF-8");
        }

        public static DateTime GetDateTimeByInt32From1970(int nConvertTime)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, nConvertTime);

            DateTime nowTime = Convert.ToDateTime("1970-1-1 8:00:00");
            DateTime convertTime = nowTime + timeSpan;

            return convertTime;
        }

        public static DateTime GetDateTimeByUnsignedInt32From1970(uint nConvertTime)
        {
            uint nFirstTime = nConvertTime;
            uint nSecondTime = 0;
            if (nFirstTime > int.MaxValue)
            {
                nFirstTime = int.MaxValue;
                nSecondTime = nConvertTime - nFirstTime;
            }
            TimeSpan timeSpan = new TimeSpan(0, 0, (int)nFirstTime);
            DateTime nowTime = Convert.ToDateTime("1970-1-1 8:00:00");
            DateTime convertTime = nowTime + timeSpan;

            if (nSecondTime > 0)
            {
                TimeSpan timeSpan2 = new TimeSpan(0, 0, (int)nSecondTime);
                convertTime += timeSpan2;
            }

            return convertTime;
        }

        public static DateTime GetDateTimeByInt64From1970(Int64 nConvertTime)
        {
            Int64 nFirstTime = nConvertTime * TimeSpan.TicksPerSecond;
            TimeSpan timeSpan = new TimeSpan(nFirstTime);
            timeSpan = timeSpan.Duration();
            DateTime nowTime = Convert.ToDateTime("1970-1-1 8:00:00");
            DateTime convertTime = nowTime + timeSpan;

            return convertTime;
        }

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

        /// <summary>
        /// 获取字节数
        /// </summary>
        /// <param name="strVal"></param>
        /// <returns></returns>
        public static int GetTextCharCount(string strVal)
        {
            int wordLength = 0;

            List<byte[]> wordList = GetString2Bytes(strVal, GetCharsetEncoding());
            int wordListCount = wordList.Count;
            for (int i = 0; i < wordListCount; ++i)
            {
                byte[] arr = wordList[i];
                if (arr.Length > 2)
                {
                    wordLength += 2;
                }
                else
                {
                    wordLength += arr.Length;
                }
            }

            return wordLength;
        }

        public static Vector3 GetRootScale(GameObject go)
        {
            Vector3 initScale = Vector3.one;
            Transform trans = go.transform;
            while (trans.parent != null)
            {
                trans = trans.parent;
                initScale.x *= trans.localScale.x;
                initScale.y *= trans.localScale.y;
                initScale.z *= trans.localScale.z;
            }

            Vector3 scale = new Vector3(1.0f / initScale.x, 1.0f / initScale.y, 1.0f / initScale.z);
            return scale;
        }

        public static Transform GetRootGameObject(Transform go)
        {
            Transform trans = go;
            while (trans.parent != null)
            {
                trans = trans.parent;
            }

            return trans;
        }

        public static bool IsExpGene(ushort geneId)
        {
            if (geneId == 1001 || geneId == 7001)
            {
                return true;
            }

            return false;
        }

        public static bool IsSpecialGene(ushort geneId)
        {
            if (!IsExpGene(geneId))
            {
                if (geneId == 7003 || geneId == 7004 || geneId == 7005)
                {
                    return true;
                }
            }

            return false;
        }

        public static Color GetColorHelp(float fr, float fg, float fb, float fbr, float fbg, float fbb)
        {
            float f_r = (float)(fr / 255);
            float f_g = (float)(fg / 255);
            float f_b = (float)(fb / 255);

            //float f_br = (float)(fbr / 255);
            //float f_bg = (float)(fbg / 255);
            //float f_bb = (float)(fbb / 255);

            return new Color(f_r, f_g, f_b, 1f);
        }

        public static void SetLayer(GameObject target, GameLayer layer, bool bRecursive, GameLayer SrcLayer)
        {
            if (target != null)
            {
                if (SrcLayer == GameLayer.NONE || target.layer == (int)SrcLayer)
                {
                    target.layer = (int)layer;
                }

                if (bRecursive)
                {
                    foreach (Transform tran in target.transform)
                    {
                        SetLayer(tran.gameObject, layer, bRecursive, SrcLayer);
                    }
                }
            }
        }

        public static bool HasChinese(string strContent)
        {
            byte[] arAccount = GetCharsetEncoding().GetBytes(strContent);
            if (arAccount.Length > strContent.Length)
            {
                return true;
            }

            return false;
        }

        public static bool IsAlphabetOrNumStr(string strValue)
        {
            if (strValue != null)
            {
                string strRegx = @"^[A-Za-z0-9]+$";
                Regex r = new Regex(strRegx);

                bool result = r.IsMatch(strValue);
                return result;
            }

            return false;
        }

        public static int GetTextLength(string strContent)
        {
            byte[] arAccount = GetCharsetEncoding().GetBytes(strContent);

            return arAccount.Length;
        }

        public static ContinuousBeatBonus CalcuPerfectBonus(int perfectCount)
        {
            ContinuousBeatBonus perfectBonus = ContinuousBeatBonus.None;
            if (perfectCount >= 8)
            {
                perfectBonus = ContinuousBeatBonus.Lv5;
            }
            else if (perfectCount >= 5)
            {
                perfectBonus = ContinuousBeatBonus.Lv4;
            }
            else if (perfectCount >= 3)
            {
                perfectBonus = ContinuousBeatBonus.Lv3;
            }
            else if (perfectCount >= 2)
            {
                perfectBonus = ContinuousBeatBonus.Lv2;
            }
            else if (perfectCount >= 1)
            {
                perfectBonus = ContinuousBeatBonus.Lv1;
            }

            return perfectBonus;
        }

        public static float GetMeshFilterWidth(MeshFilter meshFilter)
        {
            float width = 0;

            if (meshFilter != null)
            {
                Mesh ringMesh = meshFilter.mesh;
                if (ringMesh != null)
                {
                    Vector3 xyz = GetMeshxyz(ringMesh.vertices);
                    width = xyz.x > xyz.y ? xyz.x : xyz.y;
                    if (width < xyz.z)
                    {
                        width = xyz.z;
                    }
                }
            }
            else
            {
                Debug.LogError("meshFilter can not be null");
            }

            return width;
        }

        public static Vector3 GetMeshxyz(Vector3[] vertices)
        {
            Vector3 width = new Vector3();
            if (vertices != null && vertices.Length >= 1)
            {
                Vector3 max = vertices[0];
                Vector3 min = vertices[0];

                for (int t = 1; t < vertices.Length; ++t)
                {
                    Vector3 vert = vertices[t];
                    if (max.x < vert.x)
                    {
                        max.x = vert.x;
                    }
                    if (min.x > vert.x)
                    {
                        min.x = vert.x;
                    }
                    if (max.y < vert.y)
                    {
                        max.y = vert.y;
                    }
                    if (min.y > vert.y)
                    {
                        min.y = vert.y;
                    }
                    if (max.z < vert.z)
                    {
                        max.z = vert.z;
                    }
                    if (min.z > vert.z)
                    {
                        min.z = vert.z;
                    }
                }
                width.x = max.x - min.x;
                width.y = max.y - min.y;
                width.z = max.z - min.z;
            }

            return width;
        }

        public static float CalcAngle(Vector2 scr, Vector2 des, bool isYfowardDown)
        {
            Vector2 d = des - scr;
            if (isYfowardDown)
            {
                d.y *= -1;
            }
            float fAngle = Vector2.Angle(Vector2.up, d);
            if (d.x < 0)
            {
                fAngle = 360 - fAngle;
            }
            return fAngle;
        }

        /// <summary>
        /// 获取货币类型名称
        /// </summary>
        /// <param name="moneyType"></param>
        /// <returns></returns>
        public static string GetMoneyName(Money_Type moneyType)
        {
            if (moneyType == Money_Type.GoldCoin)
            {
                return "Goods_Gold";
            }
            else if (moneyType == Money_Type.MB)
            {
                return "Goods_Mcoin";
            }
            else if (moneyType == Money_Type.MBbind)
            {
                return "Goods_MbindCoin";
            }

            return "";
        }

        /// <summary>
        /// 计算Texture的尺寸(忽略UITexture的UIRect)
        /// 解决调用MakePixelPerfect时,Mathf.Round对于0.5在IOS设备上四舍五入,有1px的位移
        /// </summary>
        /// <param name="tex"></param>
        /// <returns>Texture的原始尺寸</returns>
        public static Vector3 GetTextureScale(Texture tex)
        {
            Vector3 scale = Vector3.one;

            if (tex != null)
            {
                scale = new Vector3(tex.width, tex.height, 1f);
            }
            else
            {
                Debug.LogError("CommonFunc.GetTextureScale. tex can not be null");
            }

            return scale;
        }

        /// <summary>
        /// 是否是iPhone正版
        /// </summary>
        /// <returns></returns>
        public static bool IsiPhoneOfficial()
        {
            bool isOfficial = false;

#if UNITY_IPHONE && !VERSION_IOS_ESCAPE
             isOfficial = true;
#endif
            return isOfficial;
        }

        /// <summary>
        /// 获取商城货币类型图标
        /// </summary>
        public static string GetMallCoinSpriteName(Money_Type moneyType)
        {
            string res = string.Empty;

            switch (moneyType)
            {
                case Money_Type.MB:
                    res = "shangcheng_M";
                    break;
                case Money_Type.GoldCoin:
                    res = "shangcheng_quan";
                    break;
                case Money_Type.MBbind:
                    res = "shangcheng_M_bd";
                    break;
            }

            return res;
        }

        /// <summary>
        /// 将Unix时间戳转换为DateTime类型时间
        /// </summary>
        /// <param name="d">double 型数字</param>
        /// <returns>DateTime</returns>
        public static DateTime ConvertSecondToDateTime(double d)
        {
            DateTime time = s_UnixStartTime.AddSeconds((double)d);
            return time;
        }

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>double</returns>
        public static uint ConvertDateTimeToSecond(DateTime time)
        {
            double intResult = 0;
            intResult = (time - s_UnixStartTime).TotalSeconds;
            return (uint)intResult;
        }

        /// <summary>
        /// 获取父节点下一级所有节点
        /// </summary>
        /// <param name="transParent">父节点Transform</param>
        /// <param name="includeInactive">true 包括隐藏节点</param>
        /// <returns>父节点下一级所有节点</returns>
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

        /// <summary>
        /// 判断集合是否为空
        /// </summary>
        /// <param name="collection">需要判断的集合</param>
        /// <returns>true 集合为null或者空元素</returns>
        public static bool IsCollectionNullOrEmpty(ICollection collection)
        {
            return collection == null || collection.Count <= 0;
        }

        public static int SortTransformByName(Transform a, Transform b)
        {
            return string.Compare(a.name, b.name);
        }

        /// <summary>
        /// int转化为uint,去处负数
        /// </summary>
        /// <param name="i">int类型的输入数</param>
        /// <returns>非正数全部返回0</returns>
        public static uint Int2UInt(int i)
        {
            return (uint)(i > 0 ? i : 0);
        }

        public static ushort Short2UShort(short s)
        {
            if (s > 0)
            {
                return (ushort)s;
            }

            return 0;
        }

        public static string GetCorrectWWWDir(string outFile, string InFile)
        {
            string wwwDir = "file://" + outFile;

            if (!System.IO.File.Exists(outFile))
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    wwwDir = "jar:file://" + InFile;
                }
                else
                {
                    wwwDir = "file://" + InFile;
                }
            }

            return wwwDir;
        }

        public static bool IgnoreGuide()
        {
            //Ingore All NewGuide
            if (System.IO.File.Exists(CommonValue.ConfDir + CommonDef.SD_IGNORENEWGUIDE))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 将trans移动到baseTrans节点偏移localOffset的位置,并且保持trans本地坐标z不变
        /// </summary>
        /// <param name="trans">移动节点</param>
        /// <param name="baseTrans">参照节点</param>
        /// <param name="localOffset">参照节点偏移量</param>
        public static void SetTransOffset(Transform trans, Transform baseTrans, Vector3 localOffset)
        {
            if (trans != null && baseTrans != null)
            {
                float z = trans.localPosition.z;
                trans.position = baseTrans.TransformPoint(localOffset);
                Vector3 localPos = trans.localPosition;
                localPos.z = z;
                trans.localPosition = localPos;
            }
        }

        public static int GetLength(string str)
        {
            if (str != null)
            {
                return str.Length;
            }

            return 0;
        }
    }
}