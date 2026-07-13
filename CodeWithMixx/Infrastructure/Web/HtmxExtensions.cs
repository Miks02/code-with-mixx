using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace CodeWithMixx.Infrastructure.Web;

public static class HtmxExtensions
{
    public static void ShowToast(this HttpResponse response, string message, string type = "success")
    {
        var payload = new { message, type };

        var triggerData = new
        {
            showToast = payload
        };
        
        var json = JsonSerializer.Serialize(triggerData);
        response.Headers.Append("HX-Trigger", json);
    }
} 