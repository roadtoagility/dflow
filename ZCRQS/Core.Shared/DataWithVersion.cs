namespace Core.Shared
{
    public class DataWithVersion
    {
        public DataWithVersion(int version, byte[] data)
        {
            Version = version;
            Data = data;
        }

        public int Version { get; private set; }
        public byte[] Data{ get; private set; }
    }
}