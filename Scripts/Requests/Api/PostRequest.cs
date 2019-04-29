using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Ebenit.Requests.Api
{
    /// <summary>
    /// Abstract class for POST requests.
    /// For concrete implementation it is needed to inherit this class.
    /// </summary>
    public abstract class APostRequest
    {
        /// <summary>
        /// Sends POST request. The call is asynchronous.
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="encoding">Data Encoding</param>
        /// <param name="data">Data</param>
        /// <param name="content_type">Content-Type header</param>
        /// <param name="result_output">If additional object is needed for handling data after request process.</param>
        /// <returns></returns>
        public IEnumerator sendRequest(string url, Encoding encoding, string data, string content_type, int timeout, object result_output = null)
        {
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(encoding.GetBytes(data));
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", content_type);
            request.timeout = timeout;

            yield return request.SendWebRequest();

            handleResponse(request, result_output);
        }

        /// <summary>
        /// Handles done WebRequest. May or may not be successful.
        /// </summary>
        /// <param name="request">Done WebRequest</param>
        /// <param name="result_output">Additional object passed in SendRequest method, if any was passed.</param>
        public abstract void handleResponse(UnityWebRequest request, object result_output = null);
    }
}
