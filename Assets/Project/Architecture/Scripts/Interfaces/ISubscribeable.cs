using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubscribeable<T> where T:System.Enum{
    public void ReceiveEvent(T ev);
}
