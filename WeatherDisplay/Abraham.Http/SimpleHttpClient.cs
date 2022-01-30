using System.Text;
using System.Net;

namespace Abraham.Http
{
	/// <summary>
	/// Class for REST requests
	/// </summary>
	public class SimpleHttpClient
    {
        /// <summary>
        /// Makes a HTTP GET request and returns the page content (UTF8 encoded)
        /// </summary>
        public string DownloadString(string url)
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.CookieContainer = new CookieContainer();
            httpRequest.AllowAutoRedirect = true;
            string page = "";

            HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream httpContentData = httpResponse.GetResponseStream();
                using (httpContentData)
                {
                    Encoding enc = Encoding.UTF8;
                    int byteCounter;
                    byte[] pageContent = new byte[100000];
                    do
                    {
                        byteCounter = httpContentData.Read(pageContent, 0, pageContent.Length);
                        page += enc.GetString(pageContent, 0, byteCounter);
                    }
                    while (byteCounter > 0);
                }
            }
            httpResponse.Close();
            return page;
        }
    }
}
