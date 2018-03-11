using CompanyLocator.Models;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CompanyLocator
{
    public class Parsing
    {
        #region Variables
     
        private String _mKey = "AIzaSyBHXeYijQcqEPEJS8_xMFOtzalNvxt2kcI";
        public static String _mUrl;
        public static String nextPageToken="";
        private int counter = 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// This methods splits the query into words as required in API 
        /// </summary>
        /// <param name="locationBox"> This parameter contains location entered by user.</param>
        /// <returns></returns>
        public String SplitToWords(String locationBox)
        {
            String _mlocationName;
            _mlocationName = "";
            String[] words = locationBox.Split(' ');
            foreach (var iterator in words)
            {
                _mlocationName = _mlocationName + iterator + "+";
            }
            return _mlocationName;
        }

        /// <summary>
        /// This method parses the XML response obtained from API
        /// </summary>
        /// <param name="apiResponse">It contains API response obtained from API</param>
        /// <returns>This method returns a list of company names and addresses.</returns>

        public List<Company> ParsingXMLResponse(String apiResponse)
        {

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(apiResponse);
            XmlNodeList name = doc.GetElementsByTagName("name");
            if (name.Count != 0)
            {

                List<string> nameList = new List<string>();
                XmlNodeList address = doc.GetElementsByTagName("formatted_address");

                if (counter < 2)
                {
                    XmlNodeList pageToken = doc.GetElementsByTagName("next_page_token");

                    nextPageToken = pageToken[0].InnerText;
                }

                List<string> addressList = new List<string>();
                Company[] comp = new Company[20];
                int count = 0;
                foreach (XmlNode nameIterator in name)
                {

                    nameList.Add(nameIterator.InnerText);
                    count++;
                }
                count = 0;
                foreach (XmlNode addressIterator in address)
                {
                    addressList.Add(addressIterator.InnerText);
                    count++;
                }

                var companies = new List<Company>();
                for (int i = 0; i < count; i++)
                {
                    var company = new Company
                    {
                        Name = nameList[i],
                        Address = addressList[i]
                    };
                    companies.Add(company);
                }
                counter++;
                return companies;
            }
            else
            {
                var companiest = new List<Company>();
                var companys = new Company
                {
                    Name = "No Company Found",
                    Address = "No Company Found"
                };
                companiest.Add(companys);
                return companiest;
            }
        }
        /// <summary>
        /// checks for numeric strings in location query
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        /// <summary>
        /// This method takes location from UI and passes it to API class 
        /// </summary>
        /// <param name="location">This string contains location query entered by user</param>
        /// <returns>This method returns list of companies and addresses</returns>
        public List<Company> GetLocation(string location)
        {
            if (location.Length != 0 && !location.Contains("@") && !location.Contains("*") && !location.Contains("#") && !IsDigitsOnly(location))
            {
                String splitLocation = SplitToWords(location);
                ConnectionToAPI con = new ConnectionToAPI();
                _mUrl = "https://maps.googleapis.com/maps/api/place/textsearch/xml?query=IT+company+" + splitLocation + "&hasNextPage=true&nextPage()=true" + "&key=" + _mKey + "&pagetoken=";
                List<Company> companies = ParsingXMLResponse(con.Connection(_mUrl));
                _mUrl = _mUrl + nextPageToken;
                var firstToken = nextPageToken;
                System.Threading.Thread.Sleep(2000);
                List<Company> nextCompanies = ParsingXMLResponse(con.Connection(_mUrl));
                companies.AddRange(nextCompanies);
                var originalString = "&pagetoken=" + firstToken;
                _mUrl = _mUrl.Replace(originalString, "&pagetoken=");
                _mUrl = _mUrl + nextPageToken;
                System.Threading.Thread.Sleep(2000);
                List<Company> lastCompanies = ParsingXMLResponse(con.Connection(_mUrl));
                nextPageToken = "";
                companies.AddRange(lastCompanies);
                return companies;
            }
            else {
                List<Company> noresult = new List<Company>();
                return noresult;

            }
        }
    }
        #endregion
}