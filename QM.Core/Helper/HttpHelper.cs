namespace QM.Core.Common
{
    public static class HttpHelper
    {
        public static async Task<string> PostAsync(string url, HttpContent content)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"POST request to {url} failed with status code {response.StatusCode}.");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}