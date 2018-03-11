using System.IO;
using System.Net;

namespace CompanyLocator
{
    /// <summary>
/// This class contains methods to call API and get response from API
/// </summary>
    public class ConnectionToAPI
    {
        #region Public Methods

        public string Connection(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            return responseFromServer;
        }
        #endregion
    }
}