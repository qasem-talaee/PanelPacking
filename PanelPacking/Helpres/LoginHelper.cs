using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PanelPacking.Helpres
{
    public class LoginHelper
    {

        private HttpClient client = new HttpClient();
        public UserSessionViewModel Login(string username, string password, string url)
        {
            var pas = new UserSessionViewModel()
            {
                UserName = username,
                Password = password,
            };

            client.BaseAddress = new Uri(url);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = client.PostAsJsonAsync("Account/HelpersLogin", pas).Result;
            var returnValue = response.Content.ReadAsAsync<UserSessionViewModel>().Result;
            if (returnValue.IsValid)
            {
                CreateSession(new List<string>()
                {
                    returnValue.SystemUserID,
                    returnValue.FirstName+" "+returnValue.LastName,
                    returnValue.ShiftTitle,
                });
            }
            return returnValue;

        }

        public bool LogOut()
        {
            return RemoveSession();
        }
        private void CreateSession(List<string> sessionValues)
        {
            File.WriteAllLines(Environment.CurrentDirectory + "\\session", sessionValues);
        }
        private bool RemoveSession()
        {
            bool res = false;
            try
            {
                File.Delete(Environment.CurrentDirectory + "\\session");
                res = true;
            }
            catch (Exception)
            {
                res = false;
            }
            return res;
        }

    }
    public class UserSessionViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SystemUserID { get; set; } = string.Empty;
        public string ShiftTitle { get; set; } = string.Empty;

        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
