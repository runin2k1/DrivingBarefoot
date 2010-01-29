using System;
using System.Text;

namespace DrivingBarefoot.Web.UI
{
    public class Theme
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public Theme(string name)
        {
            Name = name;
        }
    }
}
