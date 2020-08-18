using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace SMQCoreManager.Services
{
    public class ServicesBase
    {
        protected Settings settings;
        protected HttpClient httpClient;

        public ServicesBase(Task<Settings> getsettingsTask, HttpClient httpClient)
        {
            settings = getsettingsTask.Result;
            this.httpClient = httpClient;
        }

        protected Task<R> Post<P, R>(string url, P payload)
        {
            return Post<P, R>(url, payload);
        }

        protected async Task<R> Post<P, R>(string url, P payload, string token)
        {
            R result = default;

            StringContent content = new StringContent(JsonSerializer.Serialize(payload));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                result = JsonSerializer.Deserialize<R>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        protected async Task<bool> Post<P>(string url, P payload, string token)
        {
            bool result = default;

            StringContent content = new StringContent(JsonSerializer.Serialize(payload));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        protected async Task<R> Get<R>(string url, string token)
        {
            R result = default;

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                result = JsonSerializer.Deserialize<R>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        protected async Task<R> Put<P, R>(string url, P payload, string token)
        {
            R result = default;

            StringContent content = new StringContent(JsonSerializer.Serialize(payload));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                result = JsonSerializer.Deserialize<R>(await response.Content.ReadAsStringAsync());
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        protected async Task<bool> Put<P>(string url, P payload, string token)
        {
            bool result = false;

            StringContent content = new StringContent(JsonSerializer.Serialize(payload));

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.PutAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                result = false;
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        protected async Task<bool> Delete(string url, string token)
        {
            bool result = default;

            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await httpClient.DeleteAsync(url);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                HandleStatusCode(response.StatusCode, response.ReasonPhrase);
            }

            return result;
        }

        private void HandleStatusCode(HttpStatusCode status, string message)
        {
            switch (status)
            {
                case HttpStatusCode.NoContent:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.NotModified:
                    break;

                case HttpStatusCode.Forbidden:
                    throw new AccessViolationException(message);
                case HttpStatusCode.Unauthorized:
                    throw new UnauthorizedAccessException(message);
                default:
                    throw new Exception(message);
            }
        }
    }
}