using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public interface IAwake
{
    void Awake();
}

public static class AwakeExtensions
{
    public static void Awake(this IContainer container)
    {
        foreach (var aspect in container.Aspects())
        {
            var item = aspect as IAwake;
            item?.Awake();
        }
    }
}