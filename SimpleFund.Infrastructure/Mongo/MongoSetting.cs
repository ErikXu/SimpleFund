using System;

namespace SimpleFund.Infrastructure.Mongo
{
    public abstract class MongoSetting
    {
        private static MongoSetting _current;

        public static MongoSetting Current
        {
            get { return _current; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _current = value;
            }
        }

        public abstract string ConnectionString { get; }
        public abstract string DatabaseName { get; }
    }
}
