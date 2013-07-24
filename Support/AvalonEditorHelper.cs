using System;
using System.Windows;
using ICSharpCode.AvalonEdit;

namespace Theodorus2.Support
{
    public static class AvalonEditorHelper
    {

        public static string GetSelectedText(DependencyObject obj)
        {
            return (string)obj.GetValue(SelectedTextProperty);
        }

        public static void SetSelectedText(DependencyObject obj, string value)
        {
            obj.SetValue(SelectedTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectedText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTextProperty =
            DependencyProperty.RegisterAttached(
                "SelectedText",
                typeof(string),
                typeof(AvalonEditorHelper),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedTextChanged));

        private static void SelectedTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var tb = obj as TextEditor;
            if (tb == null) return;

            if (e.OldValue == null && e.NewValue != null)
            {
                tb.TextArea.SelectionChanged += tb_SelectionChanged;
            }
            else if (e.OldValue != null && e.NewValue == null)
            {
                tb.TextArea.SelectionChanged -= tb_SelectionChanged;
            }

            var newValue = e.NewValue as string;

            if (newValue != null && newValue != tb.SelectedText)
            {
                tb.SelectedText = newValue;
            }
        }

        static void tb_SelectionChanged(object sender, EventArgs e)
        {
            var tb = sender as TextEditor;
            if (tb != null)
            {
                SetSelectedText(tb, tb.SelectedText);
            }
        }

    }
}