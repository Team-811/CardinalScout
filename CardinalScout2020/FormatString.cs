using Android.Graphics;
using Android.Text;
using Android.Text.Style;

namespace CardinalScout2020
{
    /// <summary>
    /// This is a custom string formatting class based on SpannableString used to set text style and color for display.
    /// </summary>
    public static class FormatString
    {
        /// <summary>
        /// Set the given string to bold.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A bolded string.</returns>
        public static SpannableString SetBold(string input)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new StyleSpan(Android.Graphics.TypefaceStyle.Bold), 0, input.Length, 0);
            return result;
        }

        /// <summary>
        /// Set the given string to a given color.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <returns>A string with the given color.</returns>
        public static SpannableString SetColor(string input, Color color)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new ForegroundColorSpan(color), 0, input.Length, 0);
            return result;
        }

        /// <summary>
        /// Set the given string to a given color and also bold it.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="color"></param>
        /// <returns>A bolded string with the given color.</returns>
        public static SpannableString SetColorBold(string input, Color color)
        {
            SpannableString result = new SpannableString(input);
            result.SetSpan(new ForegroundColorSpan(color), 0, input.Length, 0);
            result.SetSpan(new StyleSpan(TypefaceStyle.Bold), 0, input.Length, 0);
            return result;
        }

        /// <summary>
        /// Convert the given string to a SpannableString with no formattting.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A normal string in SpannableString format.</returns>
        public static SpannableString SetNormal(string input)
        {
            SpannableString result = new SpannableString(input);
            return result;
        }
    }
}