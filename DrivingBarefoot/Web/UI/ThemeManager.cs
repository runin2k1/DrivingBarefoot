using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DrivingBarefoot.Web.UI
{
    public class ThemeManager
    {
        public static List<Theme> GetThemes()
        {
            DirectoryInfo dInfo = new DirectoryInfo(
              System.Web.HttpContext.Current.Server.MapPath("~/App_Themes"));
            DirectoryInfo[] dArrInfo = dInfo.GetDirectories();
            List<Theme> list = new List<Theme>();
            foreach (DirectoryInfo sDirectory in dArrInfo)
            {
                Theme temp = new Theme(sDirectory.Name);
                list.Add(temp);
            }
            return list;
        }
    }
}
