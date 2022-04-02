using System;

namespace DFlow.Configuration.Startup
{
    public sealed class EntryPoint
    {
        private static readonly Lazy<EntryPoint> lazy = new Lazy<EntryPoint>(() => new EntryPoint());

        private EntryPoint()
        {
        }

        public static EntryPoint Instance => lazy.Value;
    }
}