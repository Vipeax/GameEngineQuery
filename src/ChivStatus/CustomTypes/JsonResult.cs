using System;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChivStatus.CustomTypes
{
    public class JsonResult : Microsoft.AspNetCore.Mvc.JsonResult
    {
        public JsonResult(string value) : base(value)
        {
        }

        public JsonResult(object value) : base(value)
        {
        }

        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            var serializedObject = JsonConvert.SerializeObject(Value);
            response.WriteAsync(serializedObject);
        }
    }
}
