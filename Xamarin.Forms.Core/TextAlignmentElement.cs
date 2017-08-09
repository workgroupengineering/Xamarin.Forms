namespace Xamarin.Forms
{
	static class TextAlignmentElement
	{
		public static readonly BindableProperty HorizontalTextAlignmentProperty =
			BindableProperty.Create(nameof(ITextAlignmentElement.HorizontalTextAlignment), typeof(TextAlignment), typeof(EntryCell), TextAlignment.Start,
									propertyChanged: OnHorizontalTextAlignmentPropertyChanged);

		static void OnHorizontalTextAlignmentPropertyChanged(BindableObject bindable, BindablePropertyChangedEventArgs args)
		{
			((ITextAlignmentElement)bindable).OnHorizontalTextAlignmentPropertyChanged((TextAlignment)args.OldValue, (TextAlignment)args.NewValue);
		}
	}
}