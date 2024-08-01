using System;
using System.Collections.Generic;
using System.Text;
using DSharpPlus.Net;
using Newtonsoft.Json.Linq;

namespace DSharpPlus.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when an internal server error occurs.
    /// </summary>
    public class InternalServerException : Exception
    {
        /// <summary>
        /// Gets the request that caused the exception.
        /// </summary>
        public BaseRestRequest WebRequest { get; internal set; }

        /// <summary>
        /// Gets the response to the request.
        /// </summary>
        public RestResponse WebResponse { get; internal set; }

        /// <summary>
        /// Gets the JSON received.
        /// </summary>
        public string JsonMessage { get; internal set; }

        internal InternalServerException(BaseRestRequest request, RestResponse response) : base($"Internal server error.")
        {
            this.WebRequest = request;
            this.WebResponse = response;

            try
            {
                JObject j = JObject.Parse(response.Response);

                if (j["message"] != null)
                    JsonMessage = j["message"].ToString();
            }
            catch (Exception) { }
        }
    }
}
