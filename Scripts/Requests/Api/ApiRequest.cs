using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

namespace Ebenit.Requests.Api
{
    /// <summary>
    /// Class for handling everything related to sending and retrieving of Ebenit API requests.
    /// </summary>
    public class ApiRequest : APostRequest
    {
        // ----------------------------
        // json field constants - start
        // ----------------------------
        private static readonly string SIGNATURE_FIELD_NAME = "signature";
        private static readonly string SIGNATURE_REGEX = ",{0,1}\"" + SIGNATURE_FIELD_NAME + "\":[^,}]*,{0,1}";

        private static readonly string TOKEN_ID_FIELD_NAME = "token_id";
        // ----------------------------
        // json field constants - end
        // ----------------------------

        /// <summary>
        /// Token identifier. Identifies token in Ebenit API.
        /// </summary>
        private string m_token_id = null;
        /// <summary>
        /// Token itself. Used for verification purposes.
        /// </summary>
        private string m_token = null;

        /// <summary>
        /// Request timeout
        /// </summary>
        private int m_timeout = 0;

        /// <summary>
        /// Content-Type header of request.
        /// </summary>
        public string p_content_type = "application/json";


        // ----------------------------
        // static methods - start
        // ----------------------------
        /// <summary>
        /// Encodes UTF8 string into Base64 string.
        /// </summary>
        /// <param name="source">UTF8 string</param>
        /// <returns>Base64 string or null if source is null or empty</returns>
        public static string encodeBase64(string source) {
            if (string.IsNullOrEmpty(source)) {
                return null;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(source));
        }

        /// <summary>
        /// Decodes Base64 string into UTF8 string.
        /// </summary>
        /// <param name="source">Base64 string</param>
        /// <returns>UTF8 string or null on error</returns>
        public static string decodeBase64(string source) {
            try {
                return Encoding.UTF8.GetString(Convert.FromBase64String(source));
            } catch (Exception) {
                return null;
            }
        }

        /// <summary>
        /// Creates HMACSHA512 output based on message and token.
        /// </summary>
        /// <param name="message">Content to be encoded.</param>
        /// <param name="token">Secret key.</param>
        /// <returns>HMACSHA512 string</returns>
        private static string hmac(string message, string token) {
            string result = null;

            if (!string.IsNullOrEmpty(message) && !string.IsNullOrEmpty(token)) {
                HMACSHA512 hmac = new HMACSHA512(Encoding.UTF8.GetBytes(token));
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(message));

                result = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            }

            return result;
        }
        // ----------------------------
        // static methods - end
        // ----------------------------

        // ----------------------------
        // constructors - start
        // ----------------------------
        public ApiRequest(string token_id, string token, int timeout) {
            this.m_token_id = token_id;
            this.m_token = token;
            this.m_timeout = timeout;
        }
        // ----------------------------
        // constructors - end
        // ----------------------------

        // ----------------------------
        // private methods - start
        // ----------------------------
        /// <summary>
        /// Removes the signature field from message if there is any.
        /// </summary>
        /// <param name="message">Message that may or may not contain signature field.</param>
        /// <returns>Message without signature field. Or null if message is null or empty.</returns>
        private string getMessageWithoutSignature(string message) {
            if (string.IsNullOrEmpty(message)) {
                return null;
            }

            return Regex.Replace(message, SIGNATURE_REGEX, string.Empty);
        }
        // ----------------------------
        // private methods - end
        // ----------------------------

        // ----------------------------
        // public methods - start
        // ----------------------------
        /// <summary>
        /// Parses fields into the result object.
        /// </summary>
        /// <param name="message">Message to be parsed in Base64 string.</param>
        /// <param name="result">ApiRequestResult object into which the parsed parameters will be saved.</param>
        /// <returns>True/False - True if the parsing was successful; False - otherwise</returns>
        public bool parseResponseBase64(string message, ref ApiRequestResult result) {
            return parseResponse(decodeBase64(message), ref result);
        }

        /// <summary>
        /// Parses fields into the result object.
        /// </summary>
        /// <param name="message">Message to be parsed.</param>
        /// <param name="result">ApiRequestResult object into which the parsed parameters will be saved.</param>
        /// <returns>True/False - True if the parsing was successful; False - otherwise</returns>
        public bool parseResponse(string message, ref ApiRequestResult result) {
            if (string.IsNullOrEmpty(message) || result == null) {
                return false;
            }

            try {
                var response = JsonUtility.FromJson<ApiRequestResponse>(message);

                if (response != null) {
                    result.setId(response.id);

                    bool success = false;
                    if (response.results != null) {
                        success = response.results.success;
                    }
                    result.setSuccess(success);

                    bool auth_error = true;
                    bool auth_user_error = true;
                    if (response.errors != null) {
                        auth_error = response.errors.auth;
                        auth_user_error = response.errors.authUser;
                    }
                    result.setAuthError(auth_error);
                    result.setAuthUserError(auth_user_error);

                    string message_wo_signature = getMessageWithoutSignature(message);
                    string hmac = ApiRequest.hmac(message_wo_signature, m_token);

                    if (!string.IsNullOrEmpty(hmac)) {
                        result.setVerified(hmac.Equals(response.signature));
                    }

                    return true;
                }
            } catch (Exception) {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Verifies the message to the _Token string.
        /// </summary>
        /// <param name="message">Message to be verified.</param>
        /// <returns>True/False</returns>
        public bool messageVerify(string message) {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(m_token)) {
                return false;
            }

            bool result = false;

            try {
                var signature = JsonUtility.FromJson<ApiRequestResponse>(message);

                string message_wo_signature = getMessageWithoutSignature(message);
                string hmac = ApiRequest.hmac(message_wo_signature, m_token);

                if (!string.IsNullOrEmpty(hmac)) {
                    result = hmac.Equals(signature.signature);
                }
            } catch (Exception) {
                return false;
            }
            
            return result;
        }

        /// <summary>
        /// Verifies the message in Base64 string.
        /// </summary>
        /// <param name="message">Message to be verified.</param>
        /// <returns>True/False</returns>
        public bool messageVerifyBase64(string message) {
            return messageVerify(decodeBase64(message));
        }

        /// <summary>
        /// Creates the signature to sign the message.
        /// </summary>
        /// <param name="message">Message to be signed.</param>
        /// <returns>Signature</returns>
        public string messageCreateSignature(string message) {
            return hmac(getMessageWithoutSignature(message), m_token);
        }

        /// <summary>
        /// Creates the signature to sign the message. Message passed as an object (will be converted into json).
        /// </summary>
        /// <param name="message">Message to be signed.</param>
        /// <returns>Signature</returns>
        public string messageCreateSignature(object message) {
            return messageCreateSignature(JsonUtility.ToJson(message));
        }

        /// <summary>
        /// Creates the final form of message string (result is in Base64).
        /// </summary>
        /// <param name="message">Message in JSON string.</param>
        /// <returns>Base64 string representing the final form of message.</returns>
        public string messageCreateFinal(string message) {
            if (string.IsNullOrEmpty(message) || string.IsNullOrEmpty(m_token)) {
                return null;
            }

            string final = message;

            if (!final.Contains(TOKEN_ID_FIELD_NAME)) {
                final = final.Substring(0, final.Length - 1);
                final += ",\"" + TOKEN_ID_FIELD_NAME + "\":\"" + m_token_id + "\"}";
            }

            if (final.Contains(SIGNATURE_FIELD_NAME)) {
                final = getMessageWithoutSignature(final);
            }

            string signature = messageCreateSignature(final);
            final = final.Substring(0, final.Length - 1);
            final += ",\"" + SIGNATURE_FIELD_NAME + "\":\"" + signature + "\"}";

            return encodeBase64(final);
        }

        /// <summary>
        /// Creates the final form of message string (result is in Base64).
        /// </summary>
        /// <param name="message">Message (will be converted into JSON).</param>
        /// <returns>Base64 string representing the final form of message.</returns>
        public string messageCreateFinal(object message) {
            return messageCreateFinal(JsonUtility.ToJson(message));
        }
        
        /// <summary>
        /// Sends request on Ebenit API.
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="message">Message to be send.</param>
        /// <param name="result">ApiRequestResult object which will contain the result of request.</param>
        /// <returns></returns>
        public IEnumerator send(string url, string message, ref ApiRequestResult result) {
            return this.sendRequest(url, Encoding.UTF8, message, p_content_type, m_timeout, result);
        }

        /// <summary>
        /// Sends request on Ebenit API.
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="message">Message to be send. The final message will be created by MessageCreateFinal method.</param>
        /// <param name="result">ApiRequestResult object which will contain the result of request.</param>
        /// <returns></returns>
        public IEnumerator send(string url, object message, ref ApiRequestResult result) {
            string message_final = messageCreateFinal(message);

            if (string.IsNullOrEmpty(message_final)) {
                return null;
            }

            return send(url, message_final, ref result);
        }

        public override void handleResponse(UnityWebRequest request, object result_output = null) {
            if (result_output != null && result_output is ApiRequestResult) {
                ApiRequestResult result = (ApiRequestResult)result_output;

                if (request.isNetworkError || request.isHttpError || request.downloadedBytes == 0) {
                    result.setWwwError(true);
                } else {
                    result.setWwwError(false);

                    result.setResponseSource(request.downloadHandler.text);
                    result.setResponseDecoded(decodeBase64(result.pt_response_source));
                    parseResponse(result.pt_response_decoded, ref result);
                }

                result.setDone(true);
            }
        }
        // ----------------------------
        // public methods - end
        // ----------------------------
    }
}
