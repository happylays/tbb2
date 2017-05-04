/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:IStaticDataBase.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.02.27
					Modify: 
******************************************************************************/

using LoveDance.Client.Common;

namespace LoveDance.Client.Data
{
	public interface IStaticDataBase : IXQFileLoadable
	{
		uint ID { get; }
	}

	public interface IStaticDataBase<TKey> : IXQFileLoadable
	{
		TKey Key { get; }

		bool IsDataValid { get; }
	}

	public interface IXQFileLoadable
	{
		bool Load(XQFileStream file);
	}
}
