
namespace CompanyLocator.Models
{
    /// <summary>
    /// This is a model class for company details.
    /// </summary>
    public class Company
    {
        #region Variables
        
        private string _mname;
        private string _maddress;
        #endregion

        #region Properties
        public string Name
        {
            get { return _mname; }
            set { _mname = value; }
        }
        
        public string Address
        {
            get { return _maddress; }
            set { _maddress = value; }
        }
        #endregion
    }
}   