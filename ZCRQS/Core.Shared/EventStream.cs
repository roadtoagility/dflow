using System;
using System.Collections.Generic;

namespace Core.Shared
{
    public sealed class EventStream
    {
        private List<IObserverStream> _observers = new List<IObserverStream>();
        private Dictionary<string, List<IObserverStream>> _observersDictionary = new Dictionary<string, List<IObserverStream>>();
        
        private static readonly Lazy<EventStream>
            lazy =
                new Lazy<EventStream>
                    (() => new EventStream());

        public static EventStream Instance { get { return lazy.Value; } }

        private EventStream()
        {
        }
        
        public void Subscribe(IObserverStream observer)
        {
            foreach (var subscriber in observer.GetEventListers())
            {
                if (!_observersDictionary.ContainsKey(subscriber))
                {
                    _observersDictionary.Add(subscriber, new List<IObserverStream>());
                }
                
                _observersDictionary[subscriber].Add(observer);
            }
            _observers.Add(observer);
        }
        
        public void Raise(params Event[] events)
        {
            foreach (var eventUnit in events)
            {
                if (_observersDictionary.ContainsKey(eventUnit.Name))
                {
                    foreach (var observer in _observersDictionary[eventUnit.Name])
                    {
                        observer.Raise(eventUnit);
                    }
                }
            }
        }
    }
}