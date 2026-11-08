using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace InvoiceTracker.Application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            var field = value
                .GetType()
                .GetField(value.ToString());

            return field?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName()
                ?? value.ToString();
        }

        public static string GetDisplayDescription(this Enum value)
        {
            var field = value
                .GetType()
                .GetField(value.ToString());

            var displayAttribute = field?
                .GetCustomAttribute<DisplayAttribute>();

            return displayAttribute?.GetDescription()
                ?? displayAttribute?.GetName()
                ?? value.ToString();
        }
    }
}
