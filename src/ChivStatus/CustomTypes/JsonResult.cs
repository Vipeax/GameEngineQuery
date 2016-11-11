using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ChivStatus.CustomTypes
{
    public class JsonResult : Microsoft.AspNetCore.Mvc.JsonResult
    {
        public JsonResult() : base(string.Empty)
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

            var serializedObject = JsonConvert.SerializeObject(Value, this.CreateSerializerSettings());

            response.WriteAsync(serializedObject);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            response.ContentType = !string.IsNullOrEmpty(ContentType)
                ? ContentType
                : "application/json";

            var serializedObject = JsonConvert.SerializeObject(Value, this.CreateSerializerSettings());

            return response.WriteAsync(serializedObject);
        }

        private JsonSerializerSettings CreateSerializerSettings()
        {
            return new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = { new StringEnumConverter() }
            };
        }
    }
}
