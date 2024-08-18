using System;
using System.Threading.Tasks;
using UnityEngine;

namespace JorisHoef.API.Services.Base
{
    /// <summary>
    /// Don't have to create a new apiservice for each call, can just call ApiServices class which will instantiate these services when required
    /// </summary>
    /// <typeparam name="TResponse">Represents type we expect in response from Backend</typeparam>
    /// <remarks>
    /// TReponse type does not have to match the Body type, the Body we send has nothing to do with this type!
    /// </remarks>
    public abstract class ApiService<TResponse>
    {
        protected abstract HttpMethod HttpMethod { get; }
        
        public virtual async Task<ApiCallResult<TResponse>> ExecuteAsync(string endpoint, object data = null, bool requiresAuthentication = false)
        {
            try
            {
                var apiCall = new ApiCall<TResponse>(endpoint, this.HttpMethod, data, requiresAuthentication);

                ApiCallResult<TResponse> result = await apiCall.Execute();
                
                if (!result.IsSuccess)
                {
                    this.HandleApiFailure(result, endpoint);
                }
                
                return new ApiCallResult<TResponse>
                {
                        IsSuccess = result.IsSuccess,
                        Data = result.IsSuccess ? result.Data : default,
                        ErrorMessage = result.ErrorMessage,
                        Exception = result.Exception, 
                        HttpMethod = this.HttpMethod
                };
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);

                return new ApiCallResult<TResponse>
                {
                        IsSuccess = false,
                        ErrorMessage = ex.Message,
                        Exception = ex,
                        HttpMethod = this.HttpMethod
                };
            }
        }

        protected void HandleApiFailure(ApiCallResult<TResponse> result, string endpoint)
        {
            if(result == null)
            {
                Debug.LogError($"Error making API call to {endpoint} : Result is null, likely due to a failed API call or a null response.");
            }
            else
            {
                Debug.LogError($" Error making API call to {endpoint} and {result.HttpMethod} - Message: {result.ErrorMessage} - Exception: {result.Exception}");
            }
        }
    }
}