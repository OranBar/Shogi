
using System;

public class RefAction
{
	public Action Value { get; set; } = () => { };

	public void SubscribeOnce( RefAction a, Action toSubscribe ) {
		Action invokeThenUnsubscribe = null;
		invokeThenUnsubscribe = () =>
		{
			toSubscribe.Invoke();
			a.Value -= invokeThenUnsubscribe;
		};

		a.Value += invokeThenUnsubscribe;
	}

	public void Invoke(){
		Value?.Invoke();
	}

}

public class RefAction<T>
{
	public Action<T> Value { get; set; } = ( _ ) => { };

	public void SubscribeOnce( RefAction<T> a, Action<T> toSubscribe ) {
		Action<T> invokeThenUnsubscribe = null;
		invokeThenUnsubscribe = ( arg ) =>
		{
			toSubscribe.Invoke( arg );
			a.Value -= invokeThenUnsubscribe;
		};

		a.Value += invokeThenUnsubscribe;
	}

	public void Invoke(T arg) {
		Value?.Invoke(arg);
	}
}

public class RefAction<T, U>
{
	public Action<T, U> Value { get; set; } = ( _1, _2 ) => { };

	public void SubscribeOnce( RefAction<T, U> a, Action<T, U> toSubscribe ) {
		Action<T, U> invokeThenUnsubscribe = null;
		invokeThenUnsubscribe = ( arg1, arg2 ) =>
		{
			toSubscribe.Invoke( arg1, arg2 );
			a.Value -= invokeThenUnsubscribe;
		};

		a.Value += invokeThenUnsubscribe;
	}

	public void Invoke( T arg1, U arg2 ) {
		Value?.Invoke( arg1, arg2 );
	}
}

public class RefAction<T, U, V>
{
	public Action<T, U, V> Value { get; set; } = ( _1, _2, _3 ) => { };

	public void SubscribeOnce( RefAction<T, U, V> a, Action<T, U, V> toSubscribe ) {
		Action<T, U, V> invokeThenUnsubscribe = null;
		invokeThenUnsubscribe = ( arg1, arg2, arg3 ) =>
		{
			toSubscribe.Invoke( arg1, arg2, arg3 );
			a.Value -= invokeThenUnsubscribe;
		};

		a.Value += invokeThenUnsubscribe;
	}

	public void Invoke( T arg1, U arg2, V arg3 ) {
		Value?.Invoke( arg1, arg2, arg3);
	}
}

public class RefAction<T, U, V, Z>
{
	public Action<T, U, V, Z> Value { get; set; } = ( _1, _2, _3, _4 ) => { };

	public void SubscribeOnce( RefAction<T, U, V, Z> a, Action<T, U, V, Z> toSubscribe ) {
		Action<T, U, V, Z> invokeThenUnsubscribe = null;
		invokeThenUnsubscribe = ( arg1, arg2, arg3, arg4 ) =>
		{
			toSubscribe.Invoke( arg1, arg2, arg3, arg4 );
			a.Value -= invokeThenUnsubscribe;
		};

		a.Value += invokeThenUnsubscribe;
	}

	public void Invoke( T arg1, U arg2, V arg3, Z arg4 ) {
		Value?.Invoke( arg1, arg2, arg3, arg4 );
	}
}
