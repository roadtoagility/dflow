namespace DFlow.Base
{
    public class DataWithVersion
    {
        public DataWithVersion(long version, byte[] data)
        {
            Version = version;
            Data = data;
        }

        public long Version { get; private set; }
        public byte[] Data{ get; private set; }
    }
}