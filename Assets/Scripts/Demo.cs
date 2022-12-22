using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Foo
{
    public int count;
}

[Serializable]
public class Bar : Foo
{
    public string name;
}

public class Demo : MonoBehaviour
{
    public Bar bar;
    public List<Foo> foos = new();

    private void Start()
    {
        foos.Add(new Foo());
        foos.Add(new Bar());
        bar = (Bar)foos[1];
    }
}