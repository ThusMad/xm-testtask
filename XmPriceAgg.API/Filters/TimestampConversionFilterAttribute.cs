using Microsoft.AspNetCore.Mvc.Filters;
using XmPriceAgg.API.Helpers;

namespace XmPriceAgg.API.Filters;

public class TimestampConversionFilterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.TryGetValue("timestamp", out var timestamp))
        {
            if (timestamp is long timestampRaw)
            {
                context.ActionArguments["timestamp"] = TimeHelper.GetHourPrecisionTimestamp(timestampRaw);
            }
        }

        if (context.ActionArguments.TryGetValue("startStamp", out var startStamp))
        {
            if (startStamp is long startStampRaw)
            {
                context.ActionArguments["startStamp"] = TimeHelper.GetHourPrecisionTimestamp(startStampRaw);
            }
        }

        if (context.ActionArguments.TryGetValue("endStamp", out var endStamp))
        {
            if (endStamp is long endStampRaw)
            {
                context.ActionArguments["endStamp"] = TimeHelper.GetHourPrecisionTimestamp(endStampRaw);
            }
        }
    }
}