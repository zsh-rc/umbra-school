namespace Umbra.School.Shared
{
    public static class UtilityHelper
    {
        public static bool ContainsChinese(string text)
        {
            // Unicode range for Chinese characters
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"[\u4e00-\u9fa5]");
        }
    }
}
