using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    public TextMeshProUGUI attackText;
    public Image cardBack;
    public Image cardFront;
    public TextMeshProUGUI cardText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI titleText;

    public bool isFaceUp { get; private set; }
    public Card card;
    private GameObject[] faceUpElements;
    private GameObject[] faceDownElements;

    private void Awake()
    {
        faceUpElements = new[]
        {
            cardFront.gameObject,
            healthText.gameObject,
            attackText.gameObject,
            manaText.gameObject,
            titleText.gameObject,
            cardText.gameObject
        };
        faceDownElements = new[]
        {
            cardBack.gameObject
        };
        Flip(isFaceUp);
    }

    public void Flip(bool shouldShow)
    {
        isFaceUp = shouldShow;
        var show = shouldShow ? faceUpElements : faceDownElements;
        var hide = shouldShow ? faceDownElements : faceUpElements;
        Toggle(show, true);
        Toggle(hide, false);
        Refresh();
    }

    private void Toggle(GameObject[] elements, bool isActive)
    {
        foreach (var element in elements)
            element.SetActive(isActive);
    }

    private void Refresh()
    {
        if (isFaceUp == false)
            return;

        manaText.text = card.cost.ToString();
        titleText.text = card.name;
        cardText.text = card.text;

        if (card is Minion minion)
        {
            attackText.text = minion.attack.ToString();
            healthText.text = minion.maxHitPoints.ToString();
        }
        else
        {
            attackText.text = string.Empty;
            healthText.text = string.Empty;
        }
    }
}