using System.IO;
using System.Net;
using System.Runtime.Remoting;
using System.Security.Policy;
using BubelSoft.IntegrationTests.UserTests;
using Newtonsoft.Json;

namespace BubelSoft.IntegrationTests
{
    public class RestClient
    {
        private const string BaseUrl = "http://localhost:5000/api";
        private readonly string _userName;
        private readonly string _password;
        private readonly string _token;

        public RestClient(string userName, string password)
        {
            _userName = userName;
            _password = password;
            _token = GetToken();
        }

        public T Get<T>(string action)
        {
            var request = WebRequest.Create(GetUrl(action));
            
            request.Method = "GET";
            request.Headers["authorization"] = "bearer " + _token;

            var response = (HttpWebResponse)request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
                throw new ServerException($"Action: {action}");

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
            var request = WebRequest.Create(GetUrl(action));

            request.ContentType = "application/json";
            request.Method = "POST";
            request.Headers["authorization"] = "bearer " + _token;

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
            var request = WebRequest.Create(GetUrl(action));

            request.ContentType = "application/json";
            request.Method = "PUT";
            request.Headers["authorization"] = "bearer " + _token;

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
            var request = WebRequest.Create(GetUrl("user/register"));

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

        private static string GetUrl(string action) => $"{BaseUrl}/{action}";

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
                throw new ServerException("Action: LOGIN");

            using (var stream = new StreamReader(response.GetResponseStream()))
            {
                var json = stream.ReadToEnd();
                return JsonConvert.DeserializeObject<TokenResponse>(json).Token;
            }
        }

        private class TokenResponse
        {
            public string Token { get; set; }
        }

        private class IdResponse
        {
            public int Id { get; set; }
        }
    }
}