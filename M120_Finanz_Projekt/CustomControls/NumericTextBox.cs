using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace M120_Finanz_Projekt.CustomControls
{
    public class NumericTextBox : TextBox
    {
        public NumericTextBox()
        {
            PreviewTextInput += OnTextInput;
            DataObject.AddPastingHandler(this, OnPasting);
        }

        /// <summary>
        /// Called when [text input].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="TextCompositionEventArgs"/> instance containing the event data.</param>
        void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /// <summary>
        /// Determines whether [is text allowed] [the specified text].
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>
        ///   <c>true</c> if [is text allowed] [the specified text]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsTextAllowed(string text)
        {
            var regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        /// <summary>
        /// Called when [pasting].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DataObjectPastingEventArgs"/> instance containing the event data.</param>
        private void OnPasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                var text = (string)e.DataObject.GetData(typeof(string));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
