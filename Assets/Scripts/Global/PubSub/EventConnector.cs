using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Comma.Global.PubSub
{
    public class EventConnector : MonoBehaviour
    {
        private Dictionary<string, UnityEvent<object>> _eventDictionary;
        public static EventConnector EventInstance;
        private static EventConnector _eventConnector;
        private static EventConnector Instance
        {
            get
            {
                _ = !_eventConnector ? _eventConnector = FindObjectOfType<EventConnector>() : null;
                _eventConnector?.Init();

                return _eventConnector;
            }
        }

        private void Awake()
        {
            if (EventInstance != null && EventInstance != this) Destroy(gameObject);
            else EventInstance = this;
            
            // Make this object persistent once it's instantiated
            DontDestroyOnLoad(gameObject);
        }

        private void Init()
        {
            _eventDictionary ??= new();
        }

        /// <summary>
        /// Subscribe to a specific event and get event update
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="listener">Listener to be Invoked when a certain event occurs</param>
        public static void Subscribe(string eventName, UnityAction<object> listener)
        {
            // Nullify any subscribe request with no listener
            if (listener == null) return;

            if (Instance._eventDictionary.TryGetValue(
                eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }
        /// <summary>
        /// Unsubscribe from a specific event and stop receiving update
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="listener">Listener to be Invoked when a certain event occurs</param>
        public static void Unsubscribe(string eventName, UnityAction<object> listener)
        {
            // Any unsubscribe without listener is invalid request
            if (listener == null) return;

            // Check if this class is alive
            if (Instance == null) return;

            if (Instance._eventDictionary.TryGetValue(
                eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }
        /// <summary>
        /// Publish/Send an update to the subscriber wrapped in message object
        /// </summary>
        /// <param name="eventName">Name of event</param>
        /// <param name="message">Event message to be broadcasted to subscriber</param>
        public static void Publish(string eventName, object message)
        {
            // Null message is prohibited
            if (message == null) return;

            if (Instance._eventDictionary.TryGetValue(
                eventName, out UnityEvent<object> thisEvent))
            {
                thisEvent.Invoke(message);
            }
        }
    }
}