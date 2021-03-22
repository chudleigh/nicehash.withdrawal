using NLog;
using NLog.LayoutRenderers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nicehash.Withdrawal.Logger
{
    [LayoutRenderer("description")]
    public class DescriptionLayoutRenderer : CallSiteLayoutRenderer
    {
        protected override void Append(StringBuilder builder, LogEventInfo logEvent)
        {
            var type = Type.GetType(logEvent.CallerClassName);

            if (Attribute.GetCustomAttribute(type, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                builder.Append($"[{attribute.Description}]:");
            }
            else
            {
                builder.Append($"[{logEvent.CallerClassName.Split(".").Last()}]:");
            }
        }
    }
}