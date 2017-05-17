using System;
using System.Text;

namespace BudgetManager.Core.Extensions
{
    public static class ExceptionExtensions
    {
        public static string ToDetail(this Exception e)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine(e.Message);

            if (e.InnerException != null && !e.InnerException.Message.Equals(e.Message))
            {
                stringBuilder.AppendLine($"{Environment.NewLine}{ToDetail(e.InnerException)}");
            }

            return stringBuilder.ToString();
        }
    }
}