using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using BubelSoft.IntegrationTests.UserTests;
using Newtonsoft.Json;

namespace BubelSoft.IntegrationTests
{
    public class RestClient
    {
        private readonly string _userName;
        private readonly string _password;
        private readonly HttpClient _client;

        public RestClient()
        {
            _client = CreateHttpClient();
        }


        public RestClient(string userName, string password)
        {
            _userName = userName;
            _password = password;
            _client = CreateHttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", GetToken());
        }
        
        public async Task<ActionResult<T>> GetAsync<T>(string action) 
            => new ActionResult<T>(await _client.GetAsync(action).ConfigureAwait(false));

        public T Get<T>(string action)
        {
            var request = WebRequest.Create(action);
            
            request.Method = "GET";
            request.Headers["authorization"] = "bearer " + "";
            
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServerException($"Action: {action}. Result: {response.StatusCode}");

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var json = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public TOutput Post<TOutput, T>(string action, T data)
        {
            var response = Post(action, data);

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var json = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<TOutput>(json);
            }
        }

        public HttpWebResponse Post<T>(string action, T data)
        {
            var request = WebRequest.Create(action);

            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["authorization"] = "bearer ";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(data);
                stream.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if(response.StatusCode != HttpStatusCode.OK)
                throw new ServerException($"Action: {action}");

            return response;
        }

        public int? Put<T>(string action, T data)
        {
            var request = WebRequest.Create(action);

            request.ContentType = "application/json";
            request.Method = "PUT";
            request.Headers["authorization"] = "bearer ";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(data);
                stream.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if(response.StatusCode != HttpStatusCode.OK)
                throw new ServerException($"Action: {action}");
            
            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var json = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<int?>(json);
            }
        }

        public static void RegistryUser(UserRegisterInfo user)
        {
            var request = WebRequest.Create("user/register");

            request.ContentType = "application/json";
            request.Method = "POST";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(user);
                stream.Write(json);
            }

            var response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServerException("Action: user / register");
        }

        private static HttpClient CreateHttpClient() 
            => new HttpClient { BaseAddress = new Uri("http://localhost:5000/") };

        private string GetToken()
        {
            var request = WebRequest.Create("http://localhost:5000/api/user/login");

            request.ContentType = "application/json";
            request.Method = "POST";

            using (var stream = new StreamWriter(request.GetRequestStream()))
            {
                var json = JsonConvert.SerializeObject(new
                {
                    UserName = _userName,
                    Password = _password
                });
                
                stream.Write(json);
            }
            
            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServerException($"Action: LOGIN. Result: {response.StatusCode}");

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                return stream.ReadToEnd();
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }
    }

    public class ActionResult<T>
    {
        public ActionResult(HttpResponseMessage httpResponseMessage)
        {
            ResponseMessage = httpResponseMessage;
            var contentString = ResponseMessage.Content.ReadAsStringAsync().Result;
            Data = JsonConvert.DeserializeObject<T>(contentString);
        }

        public T Data { get; set; }
        public HttpStatusCode StatusCode => ResponseMessage.StatusCode;
        public HttpResponseMessage ResponseMessage { get; }
    }
}