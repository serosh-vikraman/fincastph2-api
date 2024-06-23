using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;

namespace TechnipFMC.Finapp.Service.API
{
    public static class CommonResponse
    {
        public static HttpResponseMessage GenerateCatchResponse(this HttpResponseMessage response, Exception ex)
        {
            return null;

        }
    }
    public static class CommonUser
    {
        public static string GetUserName(this string userIdentityName)
        {
            var idx = userIdentityName.IndexOf('\\');
            var userName = "";
            if (idx > 0)
            {
                userName = userIdentityName.Substring(idx + 1);
            }
            return (userName != "" ? userName : "DummyADUser");
        }
    }

    public class APIResponse<TResult>
    {
        private HttpStatusCode internalServerError;
        private object result;

        public int Status { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }
        public object Error { get; set; }
        public string Title { get; set; }
        public string TraceId { get; set; }
        public APIResponse(HttpStatusCode statusCode, TResult result, object error = null, string message = null, string traceId = "", string title = "")
        {
            Status = (int)statusCode;
            Result = result;
            Message = message;
            Error = error;
            TraceId = traceId;
            Title = title;
            Error = error;
        }

        public APIResponse(HttpStatusCode internalServerError, object result, object error, string message, string traceId, string title)
        {
            this.internalServerError = internalServerError;
            this.result = result;
            Error = error;
            Message = message;
            TraceId = traceId;
            Title = title;
        }
    }

}