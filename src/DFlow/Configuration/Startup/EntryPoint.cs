using System;

namespace DFlow.Configuration.Startup
{
    public sealed class EntryPoint
    {
        private EntryPoint()
        {
            
        }
        
        private static  readonly Lazy<EntryPoint> lazy = new Lazy<EntryPoint>(() => new EntryPoint());
        
        public static EntryPoint Instance
        {
            get { return lazy.Value; }
        }
    }
}