
namespace LoveDance.Client.Common
{
	/// <summary>
	/// 服饰属性分值等级
	/// </summary>
	public enum ClothAttrValueLevel : byte
	{
		F,
		E,
		D,
		C,
		B,
		A,
		S,
		SS,
		SSS,
	}
	
	/// <summary>
	/// 服饰光效信息显示类型
	/// </summary>
	public enum ClothAttrViewType : byte
	{
		None,
		StoneInfo,		// 光效石
		OldEffect,		// 旧特效服饰-默认激活属性
		NewEffect,		// 新特效服饰-光效石激活中
		NewEffect_Dis,	// 新特效服饰-未激活
	}
	
	/// <summary>
	/// 服饰属性信息
	/// </summary>
	public class ClothAttributeInfo
	{
		// 属性编号
		public byte m_nAttrID = 0;
		// 属性值
		public uint m_nAttrValue = 0;
		
		public ClothAttributeInfo(byte id, uint val)
		{
			m_nAttrID = id;
			m_nAttrValue = val;
		}

		public static ClothAttributeInfo CopyClothAttributeInfo(ClothAttributeInfo attInfo)
		{
			return new ClothAttributeInfo(attInfo.m_nAttrID, attInfo.m_nAttrValue);
		}
	}
	
	/// <summary>
	/// 光效石加成信息
	/// </summary>
	public class StoneAddedValueInfo
	{
		// 加成属性分组
		public byte m_nGroupID = 0;
		// 加成值
		public uint m_nAddedValue = 0;
		
		public StoneAddedValueInfo(byte groupID, uint addedVal)
		{
			m_nGroupID = groupID;
			m_nAddedValue = addedVal;
		}
	}
}
