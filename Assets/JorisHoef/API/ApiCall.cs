using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace JorisHoef.API
{
    public class ApiCall<TResponse>
    {
        protected readonly string _url;
        protected readonly HttpMethod _method;
        protected readonly object _data;
        private readonly bool _requiresAuthentication;

        private string AccessToken => !string.IsNullOrEmpty(ApiSession.Instance.AccessToken) ? ApiSession.Instance.AccessToken : null;

        public ApiCall(string url, HttpMethod method, object data = null, bool requiresAuthentication = true)
        {
            _url = url;
            _method = method;
            _data = data;
            _requiresAuthentication = requiresAuthentication;
        }

        public async Task<ApiCallResult<TResponse>> Execute()
        {
            string tokenToSend = AccessToken;

            // Not logged in return early
            if (string.IsNullOrEmpty(tokenToSend) && _requiresAuthentication)
            {
                return new ApiCallResult<TResponse>
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid token, please login",
                    HttpMethod = _method
                };
            }

            using (UnityWebRequest webRequest = PrepareRequest(tokenToSend))
            {
                try
                {
                    webRequest.SendWebRequest();

                    if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                    {
                        return new ApiCallResult<TResponse>
                        {
                            IsSuccess = false,
                            ErrorMessage = $"Connection error: {webRequest.error}",
                            HttpMethod = _method
                        };
                    }
                    else if (webRequest.result == UnityWebRequest.Result.ProtocolError)
                    {
                        // Parse error message from the response body
                        string errorMessage = await ExtractErrorMessage(webRequest);
                        return new ApiCallResult<TResponse>
                        {
                            IsSuccess = false,
                            ErrorMessage = $"HTTP error, status code {webRequest.responseCode}: {errorMessage}",
                            HttpMethod = _method
                        };
                    }

                    var result = await ExtractResultOrCatchException(webRequest);
                    result.HttpMethod = _method;
                    return result;
                }
                catch (Exception ex)
                {
                    string errorMessage = await TryExtractErrorMessage(webRequest);
                    return new ApiCallResult<TResponse>
                    {
                        IsSuccess = false,
                        ErrorMessage = errorMessage ?? $"Exception during web request: {ex.Message}",
                        Exception = ex,
                        HttpMethod = _method
                    };
                }
            }
        }

        protected virtual UnityWebRequest PrepareRequest(string tokenToSend)
        {
            try
            {
                UnityWebRequest webRequest = new UnityWebRequest(_url, _method.ToString())
                {
                        downloadHandler = new DownloadHandlerBuffer()
                };

                if (_data != null && (_method == HttpMethod.POST || _method == HttpMethod.PUT))
                {
                    string jsonPayload = JsonConvert.SerializeObject(_data);
                    webRequest.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonPayload));
                    webRequest.SetRequestHeader("Content-Type", "application/json");
                }

                webRequest.SetRequestHeader("Accept", "application/json");
        
                if (!string.IsNullOrEmpty(tokenToSend))
                {
                    webRequest.SetRequestHeader("Authorization", $"Bearer {tokenToSend}");
                }

                return webRequest;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in PrepareRequest: {ex.Message}", ex);
            }
        }

        private Task<ApiCallResult<TResponse>> ExtractResultOrCatchException(UnityWebRequest sentRequest)
        {
            try
            {
                if ((int)sentRequest.responseCode >= 200 && (int)sentRequest.responseCode < 300)
                {
                    if (sentRequest.downloadHandler != null && !string.IsNullOrEmpty(sentRequest.downloadHandler.text))
                    {
                        TResponse resultData = ParseResponse(sentRequest.downloadHandler.text);
                        if (resultData != null || sentRequest.responseCode == 204)
                        {
                            return Task.FromResult(new ApiCallResult<TResponse> { IsSuccess = true, Data = resultData });
                        }
                    }
                    else
                    {
                        return Task.FromResult(new ApiCallResult<TResponse> { IsSuccess = true });
                    }
                }

                throw new Exception($"Unexpected response code: {sentRequest.responseCode}");
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ApiCallResult<TResponse> { IsSuccess = false, Exception = ex });
            }
        }

        private TResponse ParseResponse(string rawResponse)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(rawResponse))
                {
                    try
                    {
                        return JsonConvert.DeserializeObject<TResponse>(rawResponse);
                    }
                    catch (JsonSerializationException ex)
                    {
                        throw new Exception($"Error in JSON deserialization: {ex.Message}", ex);
                    }
                }
                else
                {
                    throw new Exception("No response received from the server");
                }
            }
            catch (JsonException ex)
            {
                throw new Exception($"Couldn't parse the JSON response: {ex.Message}", ex);
            }
        }

        private Task<string> ExtractErrorMessage(UnityWebRequest sentRequest)
        {
            try
            {
                if (!string.IsNullOrEmpty(sentRequest.downloadHandler.text))
                {
                    var errorResponse = JsonConvert.DeserializeObject<JObject>(sentRequest.downloadHandler.text);
                    return Task.FromResult(errorResponse["error"]?.ToString() ?? "Unknown error");
                }
                return Task.FromResult("No error message received");
            }
            catch (JsonException ex)
            {
                return Task.FromResult($"Error parsing error message: {ex.Message}");
            }
        }

        private Task<string> TryExtractErrorMessage(UnityWebRequest webRequest)
        {
            try
            {
                if (webRequest != null && webRequest.downloadHandler != null && !string.IsNullOrEmpty(webRequest.downloadHandler.text))
                {
                    var errorResponse = JsonConvert.DeserializeObject<JObject>(webRequest.downloadHandler.text);
                    return Task.FromResult(errorResponse["error"]?.ToString());
                }
            }
            catch
            {
                // Ignore errors in error handling
            }
            return Task.FromResult<string>(null);
        }
    }
}