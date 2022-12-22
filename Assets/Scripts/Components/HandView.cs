using System.Collections.Generic;
using UnityEngine;

public class HandView : MonoBehaviour
{
    public Transform activeHandle;
    public List<Transform> cards = new();
    public Transform inactiveHandle;
}