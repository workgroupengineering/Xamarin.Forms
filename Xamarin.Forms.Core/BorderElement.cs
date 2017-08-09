namespace Xamarin.Forms
{
	static class BorderElement
	{
		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create("BorderColor", typeof(Color), typeof(IBorderElement), Color.Default,
									propertyChanged: OnBorderColorPropertyChanged);

		static void OnBorderColorPropertyChanged(BindableObject bindable, BindablePropertyChangedEventArgs args)
		{
			((IBorderElement)bindable).OnBorderColorPropertyChanged((Color)args.OldValue, (Color)args.NewValue);
		}
	}
}