// Messenger.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
//
// Inspired by and based on Rod Hyde's Messenger:
// http://www.unifycommunity.com/wiki/index.php?title=CSharpMessenger
//
// This is a C# messenger (notification center). It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other. The major improvement from Hyde's implementation is that
// there is more extensive error detection, preventing silent bugs.
//
// Usage example:
// Messenger<float>.AddListener("myEvent", MyEventHandler);
// ...
// Messenger<float>.Broadcast("myEvent", 1.0f);
 
 
using System;
using System.Collections.Generic;

namespace LoveDance.Client.Common.Messengers
{
	public enum MessengerMode : byte
	{
		DONT_REQUIRE_LISTENER,
		REQUIRE_LISTENER,
	}


	static internal class MessengerInternal
	{
		static public Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
		static public readonly MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

		static public void OnListenerAdding(string eventType, Delegate listenerBeingAdded)
		{
			if (!eventTable.ContainsKey(eventType))
			{
				eventTable.Add(eventType, null);
			}

			Delegate d = eventTable[eventType];
			if (d != null && d.GetType() != listenerBeingAdded.GetType())
			{
				UnityEngine.Debug.Log(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
			}
		}

		static public void OnListenerRemoving(string eventType, Delegate listenerBeingRemoved)
		{
			if (eventTable.ContainsKey(eventType))
			{
				Delegate d = eventTable[eventType];

				if (d == null)
				{
					UnityEngine.Debug.Log(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
				}
				else if (d.GetType() != listenerBeingRemoved.GetType())
				{
					UnityEngine.Debug.Log(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
				}
			}
			else
			{
				UnityEngine.Debug.Log(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
			}
		}

		static public void OnListenerRemoved(string eventType)
		{
			if (eventTable.ContainsKey(eventType))
			{
				if (eventTable[eventType] == null)
				{
					eventTable.Remove(eventType);
				}
			}
		}

		static public void OnBroadcasting(string eventType, MessengerMode mode)
		{
			if (mode == MessengerMode.REQUIRE_LISTENER && !eventTable.ContainsKey(eventType))
			{
				UnityEngine.Debug.Log(string.Format("Broadcasting message {0} but no listener found.", eventType));
			}
		}

		static public string CreateBroadcastSignatureException(string eventType)
		{
			return string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType);
		}
	}


	// No parameters
	static public class Messenger
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback)eventTable[eventType] - handler;
			}
			
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType)
		{
			Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback callback = d as Callback;
				if (callback != null)
				{
					callback();
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}

	// One parameter
	static public class Messenger<T>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if(eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;
			}
			
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1)
		{
			Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T> callback = d as Callback<T>;
				if (callback != null)
				{
					callback(arg1);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}


	// Two parameters
	static public class Messenger<T, U>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T, U> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T, U> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;
			}
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1, U arg2)
		{
			Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T, U> callback = d as Callback<T, U>;
				if (callback != null)
				{
					callback(arg1, arg2);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}


	// Three parameters
	static public class Messenger<T, U, V>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T, U, V> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T, U, V> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;
			}	
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3)
		{
			Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T, U, V> callback = d as Callback<T, U, V>;
				if (callback != null)
				{
					callback(arg1, arg2, arg3);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}

	// Four parameters
	static public class Messenger<T, U, V, W>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T, U, V, W> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T, U, V, W>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T, U, V, W> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T, U, V, W>)eventTable[eventType] - handler;
			}	
			
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4)
		{
			Broadcast(eventType, arg1, arg2, arg3, arg4, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T, U, V, W> callback = d as Callback<T, U, V, W>;
				if (callback != null)
				{
					callback(arg1, arg2, arg3, arg4);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}

	// Five parameters
	static public class Messenger<T, U, V, W, X>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T, U, V, W, X> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T, U, V, W, X>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T, U, V, W, X> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T, U, V, W, X>)eventTable[eventType] - handler;
			}	
			
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5)
		{
			Broadcast(eventType, arg1, arg2, arg3, arg4, arg5, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T, U, V, W, X> callback = d as Callback<T, U, V, W, X>;
				if (callback != null)
				{
					callback(arg1, arg2, arg3, arg4, arg5);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}

	// Six parameters
	static public class Messenger<T, U, V, W, X, Y>
	{
		private static Dictionary<string, Delegate> eventTable = MessengerInternal.eventTable;

		static public void AddListener(string eventType, Callback<T, U, V, W, X, Y> handler)
		{
			MessengerInternal.OnListenerAdding(eventType, handler);
			eventTable[eventType] = (Callback<T, U, V, W, X, Y>)eventTable[eventType] + handler;
		}

		static public void RemoveListener(string eventType, Callback<T, U, V, W, X, Y> handler)
		{
			MessengerInternal.OnListenerRemoving(eventType, handler);
			if (eventTable.ContainsKey(eventType))
			{
				eventTable[eventType] = (Callback<T, U, V, W, X, Y>)eventTable[eventType] - handler;
			}	
			
			MessengerInternal.OnListenerRemoved(eventType);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6)
		{
			Broadcast(eventType, arg1, arg2, arg3, arg4, arg5, arg6, MessengerInternal.DEFAULT_MODE);
		}

		static public void Broadcast(string eventType, T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6, MessengerMode mode)
		{
			MessengerInternal.OnBroadcasting(eventType, mode);
			Delegate d;
			if (eventTable.TryGetValue(eventType, out d))
			{
				Callback<T, U, V, W, X, Y> callback = d as Callback<T, U, V, W, X, Y>;
				if (callback != null)
				{
					callback(arg1, arg2, arg3, arg4, arg5, arg6);
				}
				else
				{
					UnityEngine.Debug.Log(MessengerInternal.CreateBroadcastSignatureException(eventType));
				}
			}
		}
	}
}