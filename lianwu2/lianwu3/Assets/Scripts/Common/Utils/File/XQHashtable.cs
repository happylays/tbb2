using System.Collections;

namespace LoveDance.Client.Common
{
	public class XQHashtable : Hashtable
	{

		private ArrayList list = new ArrayList();


		public override void Add(object key, object value)
		{
			if (!base.Contains(key))
			{
				base.Add(key, value);
				list.Add(key);
			}
			else
			{
			}
		}

		public override void Clear()
		{
			base.Clear();
			list.Clear();
		}

		public override void Remove(object key)
		{
			base.Remove(key);
			list.Remove(key);
		}

		public override ICollection Keys
		{
			get
			{
				return list;
			}
		}

		public void Sort()
		{
			list.Sort();
		}
	};
}