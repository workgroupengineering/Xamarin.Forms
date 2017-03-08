using System;
using System.Globalization;
using System.Linq;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.InputTransparent)]
#endif

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.None, 8675309, "Test InputTransparent true/false on various controls")]
	public class InputTransparentTests : TestNavigationPage
	{
		const string TargetAutomationId = "inputtransparenttarget";

#if UITEST
		[TestCase("Image")]
		[TestCase("Label")]
		public void VerifyInputTransparent(string menuItem)
		{
			RunningApp.WaitForElement(q => q.Marked(menuItem));
			RunningApp.Tap(q => q.Marked(menuItem));

			// Find the start label
			RunningApp.WaitForElement(q => q.Marked("Start"));

			// Find the control we're testing
			var result = RunningApp.WaitForElement(q => q.Marked(TargetAutomationId));
			var target = result.First().Rect;

			// Tap the control
			RunningApp.TapCoordinates(target.CenterX, target.CenterY);

			// Since InputTransparent is set to false, the start label should not have changed
			RunningApp.WaitForElement(q => q.Marked("Start"));

			// Switch to InputTransparent == true
			RunningApp.Tap(q => q.Marked("Toggle InputTransparent"));

			// Tap the control
			RunningApp.TapCoordinates(target.CenterX, target.CenterY);

			// Since InputTransparent is set to true, the start label should now show a single tap
			RunningApp.WaitForElement(q => q.Marked("Taps registered: 1"));
		}
#endif

		ContentPage CreateTestPage(View view)
		{
			var layout = new Grid();
			layout.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
			layout.RowDefinitions.Add(new RowDefinition());

			var abs = new AbsoluteLayout();
			var box = new BoxView { Color = Color.Red };

			var label = new Label { BackgroundColor = Color.Green, Text = "Start" };

			var taps = 0;

			abs.Children.Add(box, new Rectangle(0, 0, 1, 1), AbsoluteLayoutFlags.All);

			abs.Children.Add(label, new Rectangle(0, 0, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize), AbsoluteLayoutFlags.PositionProportional);

			box.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(() =>
				{
					taps += 1;
					label.Text = $"Taps registered: {taps}";
				})
			});

			view.InputTransparent = false;
			abs.Children.Add(view, new Rectangle(.5, .5, .5, .5), AbsoluteLayoutFlags.All);

			var toggleButton = new Button { Text = "Toggle InputTransparent" };
			toggleButton.Clicked += (sender, args) => view.InputTransparent = !view.InputTransparent;

			layout.Children.Add(toggleButton);
			layout.Children.Add(abs);

			Grid.SetRow(abs, 1);

			return new ContentPage() {Content = layout};
		}

		ContentPage Menu()
		{
			var layout = new StackLayout();

			var buttonImage = new Button { Text = "Image" };
			buttonImage.Clicked +=
				(sender, args) =>
					PushAsync(CreateTestPage(new Image { AutomationId = TargetAutomationId, Source = ImageSource.FromFile("oasis.jpg") }));

			var buttonLabel = new Button { Text = "Label" };
			buttonLabel.Clicked +=
				(sender, args) =>
					PushAsync(
						CreateTestPage(new Label
						{
							AutomationId = TargetAutomationId,
							LineBreakMode = LineBreakMode.WordWrap,
							Text =
								"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum."
						}));

			layout.Children.Add(buttonImage);
			layout.Children.Add(buttonLabel);

			return new ContentPage { Content = layout };
		}

		protected override void Init()
		{
			PushAsync(Menu());
		}
	}
}
