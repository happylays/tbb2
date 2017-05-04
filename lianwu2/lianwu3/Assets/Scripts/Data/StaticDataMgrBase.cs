/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:StaticDataMgrBase.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.07.09
					Modify: 
******************************************************************************/

using UnityEngine;
using System.Collections.Generic;
using LoveDance.Client.Common;
using System;

namespace LoveDance.Client.Data
{
	public class StaticDataMgrBase
	{
		protected void _Load<T>(XQFileStream file, Dictionary<uint, T> dic, Action<T, ushort> action) where T : IStaticDataBase, new()
		{
			dic.Clear();

			ushort usNumber = 0;
			file.ReadUShort(ref usNumber);
			for (ushort i = 0; i < usNumber; i++)
			{
				T info = new T();
				info.Load(file);
				if (action != null)
				{
					action(info, i);
				}
				if (!dic.ContainsKey(info.ID))
				{
					if(info.ID != 0)
					{
						dic.Add(info.ID, info);
					}
				}
				else
				{
					Debug.LogError(info.GetType() + " Has SameKey : " + info.ID);
				}
			}
		}

		protected void _Load<T>(XQFileStream file, Dictionary<uint, T> dic) where T : IStaticDataBase, new()
		{
			_Load<T>(file, dic, null);
		}

		protected void _Load<T>(XQFileStream file, XQHashtable hash, Action<T, ushort> action) where T : IStaticDataBase, new()
		{
			hash.Clear();

			ushort usNumber = 0;
			file.ReadUShort(ref usNumber);
			for (ushort i = 0; i < usNumber; i++)
			{
				T info = new T();
				info.Load(file);
				if (action != null)
				{
					action(info, i);
				}
				if (!hash.ContainsKey(info.ID))
				{
					if (info.ID != 0)
					{
						hash.Add(info.ID, info);
					}
				}
				else
				{
					Debug.LogError(info.GetType() + " Has SameKey : " + info.ID);
				}
			}
		}

		protected void _Load<T>(XQFileStream file, XQHashtable hash) where T : IStaticDataBase, new()
		{
			_Load<T>(file, hash, null);
		}

		protected void _Load<T>(XQFileStream file, List<T> list, Action<T, ushort> action) where T : IStaticDataBase, new()
		{
			list.Clear();

			ushort usNumber = 0;
			file.ReadUShort(ref usNumber);
			for (ushort i = 0; i < usNumber; i++)
			{
				T info = new T();
				info.Load(file);
				if (action != null)
				{
					action(info,i);
				}

				if (info.ID != 0)
				{
					list.Add(info);
				}
			}
		}

		protected void _Load<T>(XQFileStream file, List<T> list) where T : IStaticDataBase, new()
		{
			_Load<T>(file, list, null);
		}

		protected void _Load<TKey, TValue>(XQFileStream file, Dictionary<TKey, TValue> dic, Action<TValue, ushort> action) where TValue : IStaticDataBase<TKey>, new()
		{
			if (dic == null)
			{
				dic = new Dictionary<TKey, TValue>();
			}
			else
			{
				dic.Clear();
			}

			ushort usNumber = file.ReadUShort();
			TValue info = default(TValue);
			for (ushort i = 0; i < usNumber; i++)
			{
				info = new TValue();
				info.Load(file);

				if (action != null)
				{
					action(info, i);
				}

				if (!dic.ContainsKey(info.Key))
				{
					if (info.IsDataValid)
					{
						dic.Add(info.Key, info);
					}
				}
				else
				{
					Debug.LogError(info.GetType() + " Has SameKey : " + info.Key);
				}
			}
		}
	}
}