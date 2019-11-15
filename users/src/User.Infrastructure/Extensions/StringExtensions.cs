﻿namespace System
{
    public static class StringExtensions
    {
        public static bool IsMissing(this string text)
        {
            return string.IsNullOrEmpty(text)
                   || string.IsNullOrWhiteSpace(text);
        }
    }
}
