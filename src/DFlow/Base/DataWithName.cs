namespace DFlow.Base
{
    public sealed class DataWithName
    {
        public DataWithName(string name, byte[] data)
        {
            Name = name;
            Data = data;
        }

        public string Name { get; private set; }
        public byte[] Data{ get; private set; }
        
    }
}