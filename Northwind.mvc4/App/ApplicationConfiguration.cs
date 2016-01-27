using System;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;

namespace AppCore
{
    public sealed class ApplicationConfiguration : ConfigurationSection
    {
        public static ApplicationConfiguration instance = null;
        public static ApplicationConfiguration Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (ApplicationConfiguration)WebConfigurationManager
                         .GetSection("ApplicationConfiguration");
                }
                return instance;
            }
        }

        #region Public Properties
        // Create a "remoteOnly" attribute
        [ConfigurationProperty("remoteOnly", DefaultValue = "false", IsRequired = false)]
        public bool RemoteOnly
        {
            get { return (bool)this["remoteOnly"]; }
            set { this["remoteOnly"] = value; }
        }

        // Create a "defaultsettings" element
        [ConfigurationProperty("defaultSettings")]
        public DefaultSettingsElement DefaultSettings
        {
            get { return (DefaultSettingsElement)this["defaultSettings"]; }
            set { this["defaultSettings"] = value; }
        } 

        // Create a "facebook" element
        [ConfigurationProperty("facebook")]
        public FacebookElement Facebook
        {
            get { return (FacebookElement)this["facebook"]; }
            set { this["facebook"] = value; }
        }

        // Create a "twitter" element
        [ConfigurationProperty("twitter")]
        public TwitterElement Twitter
        {
            get { return (TwitterElement)this["twitter"]; }
            set { this["twitter"] = value; }
        }

        // Create a "microsoft" element
        [ConfigurationProperty("microsoft")]
        public MicrosoftElement Microsoft
        {
            get { return (MicrosoftElement)this["microsoft"]; }
            set { this["microsoft"] = value; }
        }

        // Create a "google" element        
        [ConfigurationProperty("google")]
        public GoogleElement Google
        {
            get { return (GoogleElement)this["google"]; }
            set { this["google"] = value; }
        }
        #endregion

        #region Constructors and Destructors

        #endregion

        #region Functions and Methods
        public static void Save(ApplicationConfiguration appconfig)
        {
            try
            {
                // Get the current configuration file.
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
               

                // Create the custom section entry  
                // in <configSections> group and the 
                // related target section in <configuration>.
                if (config.Sections["ApplicationConfiguration"] == null)
                {
                    config.Sections.Add("ApplicationConfiguration", appconfig);
                }

                // Save the configuration file.
                appconfig.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
        #endregion 
    }

    public class DefaultSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("connectionString", DefaultValue = "DefaultConnection", IsRequired = true)]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string ConnectionString
        {
            get { return (string)this["connectionString"]; }
            set { this["connectionString"] = value; }
        }
        [ConfigurationProperty("dbType", DefaultValue = "MySQL", IsRequired = true)]
        public string DBType
        {
            get { return (string)this["dbType"]; }
            set { this["dbType"] = value; }
        }
        [ConfigurationProperty("defaultRole", DefaultValue = "", IsRequired = true)]
        public String DefaultRole
        {
            get { return (string)this["defaultRole"]; }
            set { this["defaultRole"] = value; }
        }
        [ConfigurationProperty("supportEmail", DefaultValue = "", IsRequired = true)]
        public string SupportEmail
        {
            get { return (string)this["supportEmail"]; }
            set { this["supportEmail"] = value; }
        }
    }
    public class FacebookElement : ConfigurationElement
    {
        [ConfigurationProperty("appId", DefaultValue = "", IsRequired = false)]
        public string AppId
        {
            get { return (string)this["appId"]; }
            set { this["appId"] = value; }
        }
        [ConfigurationProperty("appSecret", DefaultValue = "", IsRequired = false)]
        public string AppSecret
        {
            get { return (string)this["appSecret"]; }
            set { this["appSecret"] = value; }
        }
    }
    public class TwitterElement : ConfigurationElement
    {
        [ConfigurationProperty("consumerKey", DefaultValue = "", IsRequired = false)]
        public string ConsumerKey
        {
            get { return (string)this["consumerKey"]; }
            set { this["consumerKey"] = value; }
        }
        [ConfigurationProperty("consumerSecret", DefaultValue = "", IsRequired = false)]
        public string ConsumerSecret
        {
            get { return (string)this["consumerSecret"]; }
            set { this["consumerSecret"] = value; }
        }
    }
    public class MicrosoftElement : ConfigurationElement
    {
        [ConfigurationProperty("clientId", DefaultValue = "", IsRequired = false)]
        public string ClientID
        {
            get { return (string)this["clientId"]; }
            set { this["clientId"] = value; }
        }
        [ConfigurationProperty("clientSecret", DefaultValue = "", IsRequired = false)]
        public string ClientSecret
        {
            get { return (string)this["clientSecret"]; }
            set { this["clientSecret"] = value; }
        }
    }
    public class GoogleElement : ConfigurationElement
    {
        [ConfigurationProperty("clientId", DefaultValue = "", IsRequired = false)]
        public string ClientID
        {
            get { return (string)this["clientId"]; }
            set { this["clientId"] = value; }
        }
        [ConfigurationProperty("clientSecret", DefaultValue = "", IsRequired = false)]
        public string ClientSecret
        {
            get { return (string)this["clientSecret"]; }
            set { this["clientSecret"] = value; }
        }
    }


}