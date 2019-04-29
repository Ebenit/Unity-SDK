namespace Ebenit.Requests.Api
{
    /// <summary>
    /// Container with result from Send method of ApiRequest.
    /// </summary>
    public class ApiRequestResult
    {
        /// <summary>
        /// Was the request finished?
        /// </summary>
        public bool pt_done {
            get; protected set;
        }

        /// <summary>
        /// Response as it was retrieved.
        /// </summary>
        public string pt_response_source {
            get; protected set;
        }

        /// <summary>
        /// Response after decoding.
        /// </summary>
        public string pt_response_decoded {
            get; protected set;
        }

        /// <summary>
        /// Was the response successfuly verified?
        /// </summary>
        public bool pt_verified {
            get; protected set;
        }

        public bool pt_success {
            get; protected set;
        }

        public string pt_id {
            get; protected set;
        }

        public bool pt_www_error {
            get; protected set;
        }

        public bool pt_auth_error {
            get; protected set;
        }

        public bool pt_auth_user_error {
            get; protected set;
        }

        public ApiRequestResult() : this(false, null, null, false, false, null, false, false, false) {
        }

        public ApiRequestResult(bool done, string response_source, string response_decoded, bool verified, bool success, string id, bool www_error = false, bool auth_error = false, bool auth_user_error = false) {
            this.pt_done = done;
            this.pt_response_source = response_source;
            this.pt_response_decoded = response_decoded;
            this.pt_verified = verified;
            this.pt_success = success;
            this.pt_id = id;

            this.pt_www_error = www_error;

            this.pt_auth_error = auth_error;
            this.pt_auth_user_error = auth_user_error;
        }

        public void setAuthError(bool auth_error = true) {
            this.pt_auth_error = auth_error;
        }

        public void setAuthUserError(bool auth_user_error = true) {
            this.pt_auth_user_error = auth_user_error;
        }

        public void setWwwError(bool www_error = true) {
            this.pt_www_error = www_error;
        }

        public void setDone(bool done = true) {
            this.pt_done = done;
        }

        public void setResponseSource(string response_source) {
            this.pt_response_source = response_source;
        }

        public void setResponseDecoded(string response_decoded) {
            this.pt_response_decoded = response_decoded;
        }

        public void setVerified(bool verified = true) {
            this.pt_verified = verified;
        }

        public void setSuccess(bool success = true) {
            this.pt_success = success;
        }

        public void setId(string id) {
            this.pt_id = id;
        }

        public override string ToString() {
            return "ApiRequestResult { _Done = " + pt_done + "; ResponseSource = " + pt_response_source + "; ResponseDecoded = " + pt_response_decoded + "; Verified = " + pt_verified + "; }";
        }
    }
}
