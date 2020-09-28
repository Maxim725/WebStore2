using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebStore.Clients.Base
{
    public abstract class BaseClient : IDisposable
    {
        protected readonly string _serviceAddress;
        protected readonly HttpClient _client;

        protected BaseClient(IConfiguration configuration, string ServiceAddress)
        {
            _serviceAddress = ServiceAddress;
            _client = new HttpClient
            {
                BaseAddress = new Uri(configuration["WebApiUrl"]),
                DefaultRequestHeaders =
                {
                    Accept = { new MediaTypeWithQualityHeaderValue("application/json") }
                }
            };
        }

        protected T Get<T>(string url) => GetAsync<T>(url).Result;

        protected async Task<T> GetAsync<T>(string url, CancellationToken cancel = default)
        {
            var response = await _client.GetAsync(url, cancel);

            return await response
                .EnsureSuccessStatusCode() // Убеждаемя, что в ответе 200-й код иначе ошибка
                .Content // В ответе есть содержимое с которым можно работать
                .ReadAsAsync<T>(cancel); // десереализация объекта

        }

        protected HttpResponseMessage Post<T>(string url, T item) => PostAsync<T>(url, item).Result;
        protected async Task<HttpResponseMessage> PostAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client.PostAsJsonAsync(url, item, cancel);
            return response.EnsureSuccessStatusCode();
        }


        protected HttpResponseMessage Put<T>(string url, T item) => PutAsync<T>(url, item).Result;
        protected async Task<HttpResponseMessage> PutAsync<T>(string url, T item, CancellationToken cancel = default)
        {
            var response = await _client.PutAsJsonAsync(url, item, cancel);
            return response.EnsureSuccessStatusCode();
        }

        protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;
        protected async Task<HttpResponseMessage> DeleteAsync(string url, CancellationToken cancel = default)
        {
            var response = await _client.DeleteAsync(url, cancel);

            if (cancel.IsCancellationRequested)
            {
                // непонятные опреации
                // шото убрать, шото логировать
            }
            // Нужно периодически вызывать этот метод, если через токен пришла отмена операции, 
            // то выбрасывается исключение
            cancel.ThrowIfCancellationRequested();
            return response;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Здесь можно выполнить освобождение неуправляемых ресурсов
            if (disposing)
            {
                // Здесь можно выполнить освобождение управляемых ресурсов

                _client.Dispose();
            }
        }

        #endregion
    }
}
