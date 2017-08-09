using Xamarin.Forms.Internals;

namespace Xamarin.Forms
{
	static class FontElement
	{
		public static readonly BindableProperty FontProperty =
			BindableProperty.Create("Font", typeof(Font), typeof(IFontElement), default(Font),
									propertyChanged: OnFontPropertyChanged);

		public static readonly BindableProperty FontFamilyProperty =
			BindableProperty.Create("FontFamily", typeof(string), typeof(IFontElement), default(string),
									propertyChanged: OnFontFamilyChanged);

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create("FontSize", typeof(double), typeof(IFontElement), -1.0,
									propertyChanged: OnFontSizeChanged,
									defaultValueCreator: FontSizeDefaultValueCreator);

		public static readonly BindableProperty FontAttributesProperty =
			BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(IFontElement), FontAttributes.None,
									propertyChanged: OnFontAttributesChanged);

		static readonly BindableProperty CancelEventsProperty =
			BindableProperty.Create("CancelEvents", typeof(bool), typeof(FontElement), false);

		static bool GetCancelEvents(BindableObject bindable) => (bool)bindable.GetValue(CancelEventsProperty);
		static void SetCancelEvents(BindableObject bindable, bool value)
		{
			bindable.SetValue(CancelEventsProperty, value);
		}

		static void OnFontPropertyChanged(BindableObject bindable, BindablePropertyChangedEventArgs arg)
		{
			if (GetCancelEvents(bindable))
				return;

			SetCancelEvents(bindable, true);

			var font = (Font)arg.NewValue;
			if (font == Font.Default) {
				bindable.ClearValue(FontFamilyProperty);
				bindable.ClearValue(FontSizeProperty);
				bindable.ClearValue(FontAttributesProperty);
			} else {
				bindable.SetValue(FontFamilyProperty, font.FontFamily);
				if (font.UseNamedSize)
					bindable.SetValue(FontSizeProperty, Device.GetNamedSize(font.NamedSize, bindable.GetType(), true));
				else
					bindable.SetValue(FontSizeProperty, font.FontSize);
				bindable.SetValue(FontAttributesProperty, font.FontAttributes);
			}
			SetCancelEvents(bindable, false);
		}

		static void OnFontFamilyChanged(BindableObject bindable, BindablePropertyChangedEventArgs arg)
		{
			if (GetCancelEvents(bindable))
				return;

			SetCancelEvents(bindable, true);

			var values = bindable.GetValues(FontSizeProperty, FontAttributesProperty);
			var fontSize = (double)values[0];
			var fontAttributes = (FontAttributes)values[1];
			var fontFamily = (string)arg.NewValue;

			if (fontFamily != null)
				bindable.SetValue(FontProperty, Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes));
			else
				bindable.SetValue(FontProperty, Font.SystemFontOfSize(fontSize, fontAttributes));

			SetCancelEvents(bindable, false);
			((IFontElement)bindable).OnFontFamilyChanged((string)arg.OldValue, (string)arg.NewValue);
		}

		static void OnFontSizeChanged(BindableObject bindable, BindablePropertyChangedEventArgs arg)
		{
			if (GetCancelEvents(bindable))
				return;

			SetCancelEvents(bindable, true);

			var values = bindable.GetValues(FontFamilyProperty, FontAttributesProperty);
			var fontSize = (double)arg.NewValue;
			var fontAttributes = (FontAttributes)values[1];
			var fontFamily = (string)values[0];

			if (fontFamily != null)
				bindable.SetValue(FontProperty, Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes));
			else
				bindable.SetValue(FontProperty, Font.SystemFontOfSize(fontSize, fontAttributes));

			SetCancelEvents(bindable, false);
			((IFontElement)bindable).OnFontSizeChanged((double)arg.OldValue, (double)arg.NewValue);
		}

		static object FontSizeDefaultValueCreator(BindableObject bindable)
		{
			return ((IFontElement)bindable).FontSizeDefaultValueCreator();
		}

		static void OnFontAttributesChanged(BindableObject bindable, BindablePropertyChangedEventArgs arg)
		{
			if (GetCancelEvents(bindable))
				return;

			SetCancelEvents(bindable, true);

			var values = bindable.GetValues(FontFamilyProperty, FontSizeProperty);
			var fontSize = (double)values[1];
			var fontAttributes = (FontAttributes)arg.NewValue;
			var fontFamily = (string)values[0];

			if (fontFamily != null)
				bindable.SetValue(FontProperty, Font.OfSize(fontFamily, fontSize).WithAttributes(fontAttributes));
			else
				bindable.SetValue(FontProperty, Font.SystemFontOfSize(fontSize, fontAttributes));

			SetCancelEvents(bindable, false);
			((IFontElement)bindable).OnFontAttributesChanged((FontAttributes)arg.OldValue, (FontAttributes)arg.NewValue);
		}
	}
}