using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualPinballUtilities.IPDB
{
    public class Utilities
    {
        private const string email = "quincy.fuller@gmail.com";
        private const string password = "jr4hl";
        public const string puid = "40770"; //userid on ipdb

        public static List<Models.ListsItem> listsItems = new List<Models.ListsItem>();

        public class Cache
        {
            public enum CacheTypeIPDB
            {
                ALL = 0,
                ListsItems = 1
            }

            public static void SetIPDBCache(CacheTypeIPDB cacheType = CacheTypeIPDB.ALL, int retryCount = 0)
            {
                //TODO: setup the lists items with name and ids
                if ((cacheType == CacheTypeIPDB.ListsItems || cacheType == CacheTypeIPDB.ALL) && (listsItems == null || listsItems.Count() == 0))
                {
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Game Listing", Id = Models.ListsValue.games });
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Abbreviation Listing", Id = Models.ListsValue.abbrev });
                    //listsItems.Add(new Models.ListsItem { Name = "IPDB Top 300 Rated", Id = Models.ListsValue.top300 });//INFO: need to decypher this one
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Manufacturer Listing (A-M)", Id = Models.ListsValue.mfg1 });
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Manufacturer Listing (N-Z)", Id = Models.ListsValue.mfg2 });
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database MPU System Listing", Id = Models.ListsValue.mpu });
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Designer/Artist/Engineer Name Listing", Id = Models.ListsValue.names });
                    listsItems.Add(new Models.ListsItem { Name = "Pinball Machine Database Designer/Artist/Engineer Last Name Listing", Id = Models.ListsValue.lastnames });
                    /*Anonymous URLs:
                    * 
                    * 
                    * Changes to individual games, have a date scraped, check the changes log when you pull in new data, rescrape if necessary
                    * https://www.ipdb.org/changes.pl
                    * 
                    * machine details
                    * https://www.ipdb.org/machine.cgi?id=4032
                    * <table><tbody>....</tbody><table>
                    * 
                    * Pinball Machine Database Game Listing
                    * https://www.ipdb.org/lists.cgi?puid=40770&list=games
                    * 
                    */
                }
            }
        }

        public static System.Net.Http.HttpClient GetClient()
        {
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            var baseAddress = new Uri("https://www.ipdb.org/lists.cgi");
            var cookieContainer = new System.Net.CookieContainer();
            var handler = new System.Net.Http.HttpClientHandler() { CookieContainer = cookieContainer };
            client = new System.Net.Http.HttpClient(handler) { BaseAddress = baseAddress };

            //usually i make a standard request without authentication, eg: to the home page.
            //by doing this request you store some initial cookie values, that might be used in the subsequent login request and checked by the server
            var homePageResult = client.GetAsync("/");
            homePageResult.Result.EnsureSuccessStatusCode();

            var contentAuth = new System.Net.Http.FormUrlEncodedContent(new[]
            {
                    //the name of the form values must be the name of <input /> tags of the login form, in this case the tag is <input type="text" name="username">
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });
            var loginResult = client.PostAsync("/lists.cgi", contentAuth).Result;
            loginResult.EnsureSuccessStatusCode();

            return client;
        }
    }
}
