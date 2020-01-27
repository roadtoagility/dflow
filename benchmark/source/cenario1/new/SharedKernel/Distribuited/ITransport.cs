using System;
using NetMQ;

namespace SharedKernel.Distribuited
{
    public interface ITransport<TInputSocket, TOutputSocket>: IDisposable 
        where TInputSocket: NetMQSocket 
        where TOutputSocket: NetMQSocket
    {
        void Start();
    }
}