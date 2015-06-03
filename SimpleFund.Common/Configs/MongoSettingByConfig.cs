using System.Configuration;
using MongoDB.Driver;
using SimpleFund.Infrastructure.Mongo;

namespace SimpleFund.Common.Configs
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>http://mongorepository.codeplex.com/</remarks>
    public class MongoSettingByConfig : MongoSetting
    {
        /// <summary>
        /// placeholder for server settings tag in app/web.config (use to build the connectionstring)
        /// </summary>
        private const string SettingsKey = "MongoServerSettings";

        /// <summary>
        /// the database tag name in app/web.config (use to build the connectionstring)
        /// </summary>
        private const string DbNameKey = "MongoDBName";

        /// <summary>
        /// Constring field
        /// </summary>
        private string _connString;

        /// <summary>
        /// The database (in use) name
        /// </summary>
        private string _database;

        /// <summary>
        /// Gets or sets the server name
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the port on which MongoDB server is bind to.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the user name for the db.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password for the db.
        /// </summary>
        public string Password { get; set; }

        public override string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connString))
                {
                    string settings = ConfigurationManager.AppSettings[SettingsKey];
                    if (string.IsNullOrWhiteSpace(settings))
                    {
                        throw new MongoException("No connection string is provided.");
                    }

                    _connString = settings;
                }

                return _connString;
            }
        }


        public override string DatabaseName
        {
            get
            {
                if (string.IsNullOrEmpty(_database))
                {
                    var db = ConfigurationManager.AppSettings[DbNameKey];
                    if (string.IsNullOrWhiteSpace(db))
                    {
                        throw new MongoException("No database name is specified.");
                    }

                    _database = db;
                }

                return _database;
            }
        }
    }
}
