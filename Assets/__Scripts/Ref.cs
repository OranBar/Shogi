
using System;

namespace Shogi {
	public class RefAction
	{
		public Action Value { get; set; } = () => { };

	}

	public class RefAction<T>
	{
		public Action<T> Value { get; set; } = ( _)  => { };

	}

	public class RefAction<T, U>
	{
		public Action<T, U> Value { get; set; } = ( _1, _2 ) => { };

	}

	public class RefAction<T, U, V>
	{
		public Action<T, U, V> Value { get; set; } = ( _1, _2, _3 ) => { };

	}

	public class RefAction<T, U, V, Z>
	{
		public Action<T, U, V, Z> Value { get; set; } = ( _1, _2, _3, _4 ) => { };

	}


}