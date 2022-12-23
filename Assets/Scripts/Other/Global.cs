using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Global
{
    public static int GenerateID<T>()
    {
        return GenerateID(typeof(T));
    }

    public static int GenerateID(System.Type type)
    {
        return Animator.StringToHash(type.Name);
    }

    public static string PrepareNotification<T>()
    {
        return PrepareNotification(typeof(T));
    }

    public static string PrepareNotification(System.Type type)
    {
        return $"{type.Name}.PrepareNotification";
    }

    public static string PerformNotification<T>()
    {
        return PerformNotification(typeof(T));
    }

    public static string PerformNotification(System.Type type)
    {
        return $"{type.Name}.PerformNotification";
    }
}