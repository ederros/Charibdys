using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer<T> where T:System.Enum
{
    Dictionary<T,List<ISubscribeable<T>>> eventsSubs = new Dictionary<T, List<ISubscribeable<T>>>();

    public void Subscribe(T eventCode,ISubscribeable<T> newSub){
        if(!eventsSubs.ContainsKey(eventCode)) eventsSubs.Add(eventCode, new List<ISubscribeable<T>>());
        if(eventsSubs[eventCode].Contains(newSub)) return;
        eventsSubs[eventCode].Add(newSub);
    }

    public void UnSubscribe(T eventCode,ISubscribeable<T> oldSub){
        if(!eventsSubs.ContainsKey(eventCode)) return;
        if(!eventsSubs[eventCode].Contains(oldSub)) return;
        eventsSubs[eventCode].Remove(oldSub);
    }
    public void Notify(T eventCode){
        if(!eventsSubs.ContainsKey(eventCode)) return;
        foreach (ISubscribeable<T> sub in eventsSubs[eventCode])
        {
            sub.ReceiveEvent(eventCode);
        }
    }
}
