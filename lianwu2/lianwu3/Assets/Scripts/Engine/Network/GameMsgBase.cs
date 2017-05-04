
namespace LoveDance.Client.Network
{
	public class GameMsgBase : INetWorkMessage
	{
		private GameMsgType m_nMsgType = 0;
		//public string m_strError = "" ;	
		//public GameMsgBase()
		//{

		//}
		public GameMsgBase(GameMsgType MsgType)
		{
			m_nMsgType = MsgType;
		}

		public GameMsgType getMsgType()
		{
			return m_nMsgType;
		}

		//public void setMsgType( ushort value)
		//{
		//    m_nMsgType = value;
		//}

		public bool decodeMessage(NetReadBuffer DataIn)
		{
			//m_strError = DataIn.GetPerfixString() ;
			if (doDecode(DataIn))
			{
				return true;
				//DataIn.Postion = DataIn.Postion + nSize - m_nMsgType ;
			}
			return false;
		}

		public virtual bool doDecode(NetReadBuffer DataIn)
		{
			return false;
		}
		public bool encodeMessage(NetWriteBuffer DataOut)
		{
			int nInitPos = DataOut.getPostion();

			DataOut.PutUShort(0);
			DataOut.PutUShort((ushort)m_nMsgType);
			//DataOut.PutString(m_strError) ; //error string 
			doEncode(DataOut);
			int nPos = DataOut.getPostion();
			DataOut.setPostion(nInitPos);
			DataOut.PutUShort((ushort)(nPos - nInitPos));
			DataOut.setPostion(nPos);

			if (DataOut.IsOverFlow())
			{
				DataOut.setPostion(nInitPos);
				return false;
			}
			return true;
		}
		public virtual bool doEncode(NetWriteBuffer DataOut)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return (int)m_nMsgType;
		}


		/*     static public void initGameMessageFactory()
			{
    
				Type[] types = Assembly.GetExecutingAssembly().GetTypes();
				Type baseType = typeof(GameMsgBase);  
				foreach (Type t in types)
				{
					Type Parent = t.BaseType;
            
					while ( Parent != null )
					{
						if( Parent == baseType )
						{
							MethodInfo MInfo = t.GetMethod( "CreateMsg" );
							if(MInfo!=null)
							{
								GameMsgBase Msg = (GameMsgBase)MInfo.Invoke(null, null);
								if( Msg != null )
								{
									MsgFactory.AddProductLine( MInfo,Msg.getMsgType() );
								}
						   }
						   break;
						}
						else
						{
							Parent = Parent.BaseType;
						}
					}
				}
			}
		*/
	}

	/*
	public class GameMsg_Base<T>: GameMsgBase where T : GameMsgBase , new()
	{
		public static GameMsgBase CreateMsg()
		{
			return new T();
		}
		public GameMsg_Base( ushort MsgType ):base(MsgType)
		{
		
		}
	}
	*/
}