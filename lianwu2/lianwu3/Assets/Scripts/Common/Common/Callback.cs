// MessengerUnitTest.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
//
// Delegates used in Messenger.cs.
namespace LoveDance.Client.Common
{
	public delegate void Callback();
	public delegate void Callback<T>(T arg1);
	public delegate void Callback<T, U>(T arg1, U arg2);
	public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);
	public delegate void Callback<T, U, V, W>(T arg1, U arg2, V arg3, W arg4);
	public delegate void Callback<T, U, V, W, X>(T arg1, U arg2, V arg3, W arg4, X arg5);
	public delegate void Callback<T, U, V, W, X, Y>(T arg1, U arg2, V arg3, W arg4, X arg5, Y arg6);

	public delegate T CallbackReturn<T>();
	public delegate T CallbackReturn<T, U>(U arg1);
}