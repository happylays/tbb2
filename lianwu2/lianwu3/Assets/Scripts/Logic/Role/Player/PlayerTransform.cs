using UnityEngine;
using System;
using System.Collections.Generic;
//using LoveDance.Client.Network.Player;
//using LoveDance.Client.Data.Transform;
//using LoveDance.Client.Data;


namespace LoveDance.Client.Logic.Role
{
	public class PlayerTransform : ICloneable
	{
		public uint TransformID
		{
			get
			{
				return m_TransformD;
			}
			set
			{
				m_TransformD = value;
			}
		}

		public uint TransformTitle
		{
			get
			{
				return m_TransformTitle;
			}
			set
			{
				m_TransformTitle = value;
			}
		}
        		
		uint m_TransformD = 0;
		uint m_TransformTitle = 0;

	}
}
