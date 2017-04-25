
using System.Collections.Generic;

//物品相关
namespace LoveDance.Client.Common
{
    /////////////////////////////////////////////////////////////////////
    //一级类型
    /////////////////////////////////////////////////////////////////////

    //装备，道具
    public enum ItemClass_Type : byte
    {
        ItemClassType_None = 0,

        ItemClassType_Equip,		//装备
        ItemClassType_Expendable,	//道具(消耗品)
        ItemClassType_Numerical,    //数值类（金钱，经验，荣誉等数值类奖励）

        ItemClassType_MaxNumber
    };

    ////////////////////////////////////////////////////////////////////
    //二级类型
    ////////////////////////////////////////////////////////////////////

    //子类型，装备之衣服，首饰等
    public enum ItemEquip_Type : byte
    {
        ItemEquipType_None = 0,

        ItemEquipType_Cloth,		//衣服
        ItemEquipType_Badge,		//徽章
        ItemEquipType_Vehicle,		//座驾

        ItemEquipType_MaxNumber
    };
}