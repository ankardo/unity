using System.Collections.Generic;
using UnityEngine;

public delegate void MultiplicativeModifiers(object args);
public class Stats : MonoBehaviour
{
    public List<Stat> overall;
    public MultiplicativeModifiers multiplicativeModifiers;
    public Stat this[StatEnum statEnum]
    {
        get { return overall[(int)statEnum]; }
    }
}
