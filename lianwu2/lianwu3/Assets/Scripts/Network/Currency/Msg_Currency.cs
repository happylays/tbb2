/******************************************************************************
					Copyright (C), 2014-2015, DDianle Tech. Co., Ltd.
					Name:Msg_Currency.cs
					Author: Caihuijie
					Description: 
					CreateDate: 2015.01.29
					Modify: 
******************************************************************************/

using System.Collections.Generic;
using LoveDance.Client.Common;

namespace LoveDance.Client.Network.Currency
{
    public class GameMsg_S2C_AllowCurrencyList : GameMsgBase
    {
        public List<byte> m_ListCurrency = new List<byte>();

        public GameMsg_S2C_AllowCurrencyList()
            : base(GameMsgType.MSG_S2C_AllowCurrencyList)
        {

        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            ushort nCount = DataIn.GetUShort();

            for (int i = 0; i < nCount; i++)
            {
                byte currency = DataIn.GetByte();
                m_ListCurrency.Add(currency);
            }

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_AllowCurrencyList();
        }
    }

    public class GameMsg_C2S_ClientDeviceInfo : GameMsgBase
    {
        public string m_strDeviceId = "";
        public List<byte> m_ListCurrency = new List<byte>();

        public GameMsg_C2S_ClientDeviceInfo()
            : base(GameMsgType.MSG_C2S_ClientDeviceInfo)
        {

        }

        public override bool doEncode(NetWriteBuffer DataOut)
        {
            DataOut.PutString(m_strDeviceId);

            ushort nCount = (ushort)m_ListCurrency.Count;
            DataOut.PutUShort(nCount);

            for (int i = 0; i < nCount; ++i)
            {
                DataOut.PutByte(m_ListCurrency[i]);
            }

            return true;
        }
    }

    public class GameMsg_S2C_ValidChargeDevice : GameMsgBase
    {
        public bool m_RechargeBlacklist = true;

        public GameMsg_S2C_ValidChargeDevice()
            : base(GameMsgType.MSG_S2C_ValidChargeDevice)
        {

        }

        public override bool doDecode(NetReadBuffer DataIn)
        {
            m_RechargeBlacklist = DataIn.GetBool();

            return true;
        }

        public static GameMsgBase CreateMsg()
        {
            return new GameMsg_S2C_ValidChargeDevice();
        }
    }
}