使用方法

例子：在按下拍照按钮后通知刷新好友面板
步骤1、添加事件字段，该字段具有唯一性
		在MessangerEventDef脚本（可以每个功能都有一个事件字段脚本，类似于消息）中添加字段"Demo_EventType".

步骤2、广播事件：在按下拍照按钮方法中调用
		Messenger.Broadcast(MessangerEventDef.Demo_EventType);

步骤3、在OnEnable()方法中注册事件：在FriendUI_Ctrl中的OnEnable方法中添加
		Messenger.AddListener(MessangerEventDef.Demo_EventType, OnCall);

步骤4、在OnDisable()方法中移除事件：在FriendUI_Ctrl中的OnDisable方法中添加
		Messenger.RemoveListener(MessangerEventDef.Demo_EventType, OnCall);

		void OnCall()
		{
			Debug.LogError("===OnCall==");
			//TODO 刷新好友相关代码
		}

注意事项
	1、AddListener和RemoveListener必须成对出现。
	2、有什么疑问及时和我沟通。


														--Add by 熊晖