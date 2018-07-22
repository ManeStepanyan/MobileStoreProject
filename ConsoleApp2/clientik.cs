using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class clientik
    {
        public static async Task MainAsync()
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, "SuperAdmin", "secret");


            //   var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("admin888", "administrator11", "UserAPI");
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("example11", "erkinq", "UserAPI");


            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");

            // call api
            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            var response = await client.GetAsync("http://localhost:5003/api/sellerproduct");


            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(Newtonsoft.Json.Linq.JArray.Parse(content));

            }
            Console.Read();
        }
    
    }
}
