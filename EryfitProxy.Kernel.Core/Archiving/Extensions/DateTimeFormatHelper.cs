

using System;

namespace EryfitProxy.Kernel.Archiving.Extensions
{
    internal static class DateTimeFormatHelper
    {
        public static string? FormatWithLocalKind(this DateTime date)
        {
            return date == DateTime.MinValue ? null : DateTime.SpecifyKind(date, DateTimeKind.Local).ToString("o");
        }
    }
}
