using System.Collections;
using System.Collections.Generic;
using TheLiquidFire.AspectContainer;
using UnityEngine;

public interface IDestroy
{
    void Destroy();
}

public static class DestroyExtensions
{
    public static void Destroy(this IContainer container)
    {
        foreach (var aspect in container.Aspects())
        {
            var item = aspect as IDestroy;
            item?.Destroy();
        }
    }
}