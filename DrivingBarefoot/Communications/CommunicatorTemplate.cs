using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
using System.Collections;
using System.Web.Configuration;

namespace DrivingBarefoot.Communications
{
    /// <summary>
    /// 
    /// </summary>
    public class CommunicatorTemplate
    {
        private string _fileName;

        public string FileName
        {
            set { _fileName = value; LoadFileTemplate(); }
        }

        private string _fileTemplate;

        public string FileTemplate
        {
            get { return _fileTemplate; }
        }

        private Hashtable _templateFields = new Hashtable();

        public Hashtable TemplateFields
        {
            get { return _templateFields; }
        }

        private string templateDirectory;

        public CommunicatorTemplate()
        {
            CheckTemplateDirectory();
        }

        public CommunicatorTemplate(string fileName)
        {
            CheckTemplateDirectory();

            FileName = fileName;
        }

        private void CheckTemplateDirectory()
        {
            
            templateDirectory = HttpContext.Current.Server.MapPath(WebConfigurationManager.AppSettings["TemplateDir"]);

            if (templateDirectory == null || !(new DirectoryInfo(templateDirectory)).Exists)
                throw new DirectoryNotFoundException("could not find template directory, or directory not specified in Web.Config->AppSettings->TemplateDir");
        }

        public void FillTemplate()
        {
            foreach (String key in _templateFields.Keys)
            {
                _fileTemplate = _fileTemplate.Replace(key, (string)_templateFields[key]);
            }
        }
        private void LoadFileTemplate()
        {
            _fileTemplate = File.ReadAllText(templateDirectory + _fileName);
        }

        public String GetFilledTemplate()
        {
            return _fileTemplate;
        }
    }
}
