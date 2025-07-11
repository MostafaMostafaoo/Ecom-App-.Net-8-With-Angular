﻿namespace Ecom.Api.Helper
{
    public class ResponseAPI
    {

        public ResponseAPI(int statusCode , string message=null)
        {
            StatusCode = statusCode;
            Message = message ?? GetMessageFormStatusCode(statusCode);
        }

        private string GetMessageFormStatusCode(int statuscode)
        {
            return statuscode switch
            {
                200 => "Done",
                400 => "Bad Request",
                401 => "Un Authorized",
                404 => "not found resourses",
                500 => "Server Error",
                _ => null,
            };
        }
        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
