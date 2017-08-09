using System;

namespace Xamarin.Forms
{
	public struct BindablePropertyChangedEventArgs
	{
		public object NewValue { get; internal set; }
		public object OldValue { get; internal set; }
		public BindableProperty Property { get; internal set; }
	}
}
