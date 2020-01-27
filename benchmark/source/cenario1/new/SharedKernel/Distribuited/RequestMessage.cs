namespace SharedKernel.Distribuited
{
    internal class RequestMessage
    {
        public string ReturnType { get; set; }
        
        public string FilterType { get; set; }
        
        public string Payload { get; set; }

        public RequestMessage()
        {
            
        }
        
        
        public RequestMessage(string returnType, string filterType, string payload)
        {
            this.ReturnType = returnType;
            this.FilterType = filterType;
            this.Payload = payload;
        }
    }
}