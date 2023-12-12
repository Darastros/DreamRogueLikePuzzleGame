using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public interface IEventListener
    { }

    public class EventDispatcher
    {
	    private readonly Dictionary<Type, List<(IEventListener listener, Action<Event> callback)>> m_listeners = new();

        public void RegisterEvent<T>(IEventListener _listener, Action<T> _callback) where T : Event
		{ 
			var eventType = typeof(T);

			if (!m_listeners.ContainsKey(eventType))
				m_listeners[eventType] = new List<(IEventListener listener, Action<Event> callback)>();
			
			m_listeners[eventType].Add((_listener, _e => _callback.Invoke((T)_e)));
		}

		public void UnregisterEvent<T>(IEventListener _listenerToUnregister) where T : Event
		{
			var eventType = typeof(T);

			if (m_listeners.TryGetValue(eventType, out var existingList))
			{
				existingList.RemoveAll(_x => _x.listener == _listenerToUnregister);
				if (existingList.Count == 0)
				{
					m_listeners.Remove(eventType);
				}
			}
		}

		public void UnregisterAllEvents(IEventListener _eventListener)
		{
			foreach (var key in new List<Type>(m_listeners.Keys))
			{
				var existingList = m_listeners[key];
				existingList.RemoveAll(_x => _x.listener == _eventListener);
				if (existingList.Count == 0)
				{
					m_listeners.Remove(key);
				}
			}
		}

        public void SendEvent(Event _eventToSend)
        {
	        var eventType = _eventToSend.GetType();
	        if (m_listeners.TryGetValue(eventType, out var eventListeners))
	        {
		        foreach (var (listener, callback) in eventListeners)
		        {
			        callback.Invoke(_eventToSend);
		        }
	        }
        }

        public void SendEvent<T>(params object[] _args) where T : Event
        {
            
	            var eventType = typeof(T);
	            if (!(Activator.CreateInstance(eventType, _args) is T instance))
	            {
		            Debug.LogError($"Failed to create event instance and send event: {typeof(T)} with arguments {_args}");
		            return;
	            }
	            SendEvent(instance);
        }
    }

    public class Event
    {
        public Event()
        { }
    }
}