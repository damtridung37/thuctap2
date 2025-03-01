using System.Collections.Generic;
using System;

public static class GlobalEvent<T>
{
    private static Dictionary<string, Action<T>> eventDictionary = new Dictionary<string, Action<T>>();

    public static void Subscribe(string eventName, Action<T> action, bool overwrite = false)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            if (overwrite)
                eventDictionary[eventName] = action;
            else
                eventDictionary[eventName] += action;
        }
        else
        {
            eventDictionary.Add(eventName, action);
        }
    }

    public static void Unsubscribe(string eventName, Action<T> action)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName] -= action;
        }
    }

    public static void Trigger(string eventName, T parameter)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary[eventName]?.Invoke(parameter);
        }
    }

    public static void ClearEvent(string eventName)
    {
        if (eventDictionary.ContainsKey(eventName))
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void ClearAllEvents()
    {
        eventDictionary.Clear();
    }
}
