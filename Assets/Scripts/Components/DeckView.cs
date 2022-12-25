using UnityEngine;

public class DeckView : MonoBehaviour
{
    [SerializeField] private Transform squisher;
    public Transform topCard;

    public void ShowDeckSize(float size)
    {
        squisher.localScale = Mathf.Approximately(size, 0) ? Vector3.zero : new Vector3(1, size, 1);
    }
}