using Flurl.Http;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using IRO_UNMO.Model;

namespace IRO_UNMO.Mobile
{
    public class APIService
    {
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string UniqueCode { get; set; }
        public static Model.Applicant LoggedUser { get; set; }

        private readonly string _route;
        public APIService(string route)
        {
            _route = route;
        }

        //private readonly string APIUrl = "http://192.168.137.1:5000/api"; // za IIS
        private readonly string APIUrl = "http://localhost:5000/api"; // ZA IIS EXPRESS
        //private readonly string APIUrl = "https://IRO_UNMOwebapi20190717101945.azurewebsites.net/api"; // AZURE, NE RADI VISE

        public async Task<bool> Remove(int id)
        {
            var url = $"{APIUrl}/{_route}/{id}";

            try
            {
                return await url.WithBasicAuth(Username, Password).DeleteAsync().ReceiveJson<bool>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Pogrešan unos podataka ili nemate akaunt!", "OK");
                    return false;
                }
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Niste autorizovani", "OK");
                }

                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, {string.Join(",", error.Value)}");
                }

                await Application.Current.MainPage.DisplayAlert("Greška", "", "OK");
                return false;
            }
        }

        public async Task<T> Get<T>(object search, string actionName = null)
        {
            var url = $"{APIUrl}/{_route}";

            try
            {
                if (actionName != null)
                {
                    url += "/" + actionName;
                }

                if (search != null)
                {
                    url += "?";
                    url += await search.ToQueryString();
                }

                return await url.WithBasicAuth(Username, Password).GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Pogrešan unos podataka ili nemate akaunt!", "OK");
                }
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Niste autorizovani.", "OK");
                    return default(T);
                }
                throw;
            }
        }

        public async Task<T> Insert<T>(object entity, string actionName = null)
        {
            var url = $"{APIUrl}/{_route}";

            if (actionName != null)
            {
                url += "/" + actionName;
            }

            try
            {
                return await url.WithBasicAuth(Username, Password).PostJsonAsync(entity).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Pogrešan unos podataka ili nemate akaunt!", "OK");
                    return default(T);
                }
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Niste autorizovani.", "OK");
                    return default(T);
                }

                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, {string.Join(",", error.Value)}");
                }

                await Application.Current.MainPage.DisplayAlert("Error", stringBuilder.ToString(), "OK");
                return default(T);
            }
        }

        public async Task<T> GetById<T>(int id, string actionName = null)
        {
            var url = $"{APIUrl}/{_route}";

            try
            {
                if (actionName != null)
                    url += "/" + actionName;

                url += "/" + id;
                return await url.WithBasicAuth(Username, Password).GetJsonAsync<T>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Pogrešan unos podataka ili nemate akaunt!", "OK");
                }
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Niste autorizovani.", "OK");
                    return default(T);
                }
                throw;
            }
        }

        public async Task<bool> Delete(int id, string actionName = null)
        {
            var url = $"{APIUrl}/{_route}";

            if (actionName != null)
                url += "/" + actionName;

            url += "/" + id;

            return await url.WithBasicAuth(Username, Password).DeleteAsync().ReceiveJson<bool>();
        }

        public async Task<T> Update<T>(int id, object request)
        {
            var url = $"{APIUrl}/{_route}/{id}";

            try
            {
                return await url.WithBasicAuth(Username, Password).PutJsonAsync(request).ReceiveJson<T>();
            }
            catch (FlurlHttpException ex)
            {
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Pogrešan unos podataka ili nemate akaunt!", "OK");
                    return default(T);
                }
                if (ex.Call.HttpStatus == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Greška", "Niste autorizovani.", "OK");
                    return default(T);
                }

                var errors = await ex.GetResponseJsonAsync<Dictionary<string, string[]>>();

                var stringBuilder = new StringBuilder();
                foreach (var error in errors)
                {
                    stringBuilder.AppendLine($"{error.Key}, {string.Join(",", error.Value)}");
                }

                await Application.Current.MainPage.DisplayAlert("Error", stringBuilder.ToString(), "OK");
                return default(T);
            }
        }
    }
}