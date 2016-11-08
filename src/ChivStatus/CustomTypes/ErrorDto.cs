using System.Net;
using Newtonsoft.Json;

namespace ChivStatus.CustomTypes
{
    public class ErrorDto
    {
        public ErrorDto(HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
            this.Code = code;
        }

        public HttpStatusCode Code { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
