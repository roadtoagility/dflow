using System;
using System.Collections.Generic;

namespace SharedKernel
{
    public sealed class QueryHandlersActivation
    {
        private Dictionary<string, bool> Activated;
        
        private QueryHandlersActivation()  
        { 
            Activated = new Dictionary<string, bool>();
        }  
        
        private static readonly Lazy<QueryHandlersActivation> lazy = new Lazy<QueryHandlersActivation>(() => new QueryHandlersActivation());  
        public static QueryHandlersActivation Instance  
        {  
            get  
            {  
                return lazy.Value;  
            }  
        }

        public bool IsActivate(string name)
        {
            return Activated.ContainsKey(name);
        }

        public void Activate(string name)
        {
            Activated.Add(name, true);
        }
    }  
}