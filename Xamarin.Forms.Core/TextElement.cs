namespace Xamarin.Forms
{
	static class TextElement
	{
		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(ITextElement.TextColor), typeof(Color), typeof(ITextElement), Color.Default,
									propertyChanged: OnTextColorPropertyChanged);

		static void OnTextColorPropertyChanged(BindableObject bindable, BindablePropertyChangedEventArgs arg)
		{
			((ITextElement)bindable).OnTextColorPropertyChanged((Color)arg.OldValue, (Color)arg.NewValue);
		}
	}
}