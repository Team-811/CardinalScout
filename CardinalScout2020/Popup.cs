using Android.App;
using Android.Content;
using System;

namespace CardinalScout2020
{    
    /// <summary>
    /// This is a custom class to make using AlertDialogs easier.
    /// </summary>
    public static class Popup
    {
        /// <summary>
        /// Dialog box with just an OK button.
        /// </summary>
        /// <param name="title"> Title of the messag box.</param>
        /// <param name="message"> Message in the box.</param>
        /// <param name="button"> Text on the button.</param>
        /// <param name="context"> Where the box is (usually "this").</param>
        public static void Single(string title, string message, string button, Context context)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            AlertDialog popup = dialog.Create();
            popup.SetTitle(title);
            popup.SetMessage(message);
            popup.SetButton(button, (c, ev) =>
            {
            });
            popup.Show();
        }

        /// <summary>
        /// Dialog box with two buttons. Usually yes or no.
        /// </summary>
        /// <param name="title"> Title of the messag box.</param>
        /// <param name="message"> Message in the box.</param>
        /// <param name="button1"> Text on the first button.</param>
        /// <param name="button2"> Text on the second button.</param>
        /// <param name="context"> Where the box is (usually "this").</param>
        /// <param name="ifYes"> Method to be executed if the first button is clicked</param>
        public static void Double(string title, string message, string button1, string button2, Context context, Action ifYes)
        {
            AlertDialog.Builder dialog = new AlertDialog.Builder(context);
            AlertDialog popup = dialog.Create();
            popup.SetTitle(title);
            popup.SetMessage(message);
            popup.SetButton(button1, (c, ev) =>
            {
                ifYes();
            });
            popup.SetButton2(button2, (c, ev) => { });
            popup.Show();
        }
    }
}