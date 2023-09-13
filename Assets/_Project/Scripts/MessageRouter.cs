using System.Collections.Generic;
using UnityEngine;

using static MessageRouter;

public class MessageRouter : object
{
    private Dictionary<string, MessageDelegate> listeners = new Dictionary<string, MessageDelegate>();
    public delegate void MessageDelegate(string channel, object data);

    public ListenerHandle RegisterListener(string channel, MessageDelegate inDelegate)
    {
        if (!listeners.ContainsKey(channel))
        {
            listeners.Add(channel, inDelegate);
            return new ListenerHandle(channel, inDelegate);
        }

        listeners[channel] += inDelegate;
        return new ListenerHandle(channel, inDelegate);
    }

    public void UnregisterListener(ListenerHandle handle)
    {
        if (!listeners.ContainsKey(handle.Channel)) return;
        listeners[handle.Channel] -= handle.Delegate;
    }

    public void BroadcastMessage(string channel, object data)
    {
        if (!listeners.ContainsKey(channel)) return;
        if (listeners[channel] == null) return;

        List<MessageDelegate> invalidListenerHandles = null;
        foreach (MessageDelegate messageDelegate in listeners[channel].GetInvocationList())
        {
            if (messageDelegate.Target is MonoBehaviour behaviour)
            {
                if (behaviour == null)
                {
                    invalidListenerHandles ??= new List<MessageDelegate>();
                    invalidListenerHandles.Add(messageDelegate);
                    continue;
                }
            }

            messageDelegate.Invoke(channel, data);
        }

        if (invalidListenerHandles == null) return;
        foreach (MessageDelegate invalidListenerHandle in invalidListenerHandles)
        {
            listeners[channel] -= invalidListenerHandle;
        }
    }
}

public class ListenerHandle
{
    public ListenerHandle(string channel, MessageDelegate inDelegate)
    {
        Channel = channel;
        Delegate = inDelegate;
    }

    public readonly string Channel;
    public readonly MessageDelegate Delegate;
}