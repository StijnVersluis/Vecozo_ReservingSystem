using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using ViewLayer.Models;

namespace ViewLayer.Util
{
    public static class ControllerExtensions
    {
        public static void SendResponse(this Controller controller, bool result, string title, string message)
        {
            controller.TempData["JsonResponse"] = JsonConvert.SerializeObject(new ResponseViewModel(result, title, message));
        }

        public static void GetResponse(this Controller controller)
        {
            var reponse = controller.TempData["JsonResponse"]?.ToString();
            if (!String.IsNullOrEmpty(reponse))
            {
                controller.ViewData["ResponseModel"] = JsonConvert.DeserializeObject<ResponseViewModel>(reponse);
            }
        }
    }
}
