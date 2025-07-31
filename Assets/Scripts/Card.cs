using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum CardType
{
    Damage,
    Modifier,
    Utility
}

[System.Serializable]
public enum EffectType
{
    None,
    MultiplyDamage,
    ReverseOrder,
    IncreaseClockwise,
    IncreaseCounterClockwise

}

[CreateAssetMenu(fileName = "New Card", menuName = "Clock Game/Card")]
public class Card : ScriptableObject
{
    [Header("Basic Card Info")]
    public string cardName = "Basic Card";
    public CardType cardType = CardType.Damage;

    [Header("Card Stats")]
    public int baseDamage = 10;
    public float multiplier = 1.0f;

    [Header("Special Effects")]
    public EffectType effectType = EffectType.None;
    public int effectValue = 0;
    public string effectDescription = "";

    [Header("Visual")]
    public Sprite cardArt;
    public Color cardColor = Color.white;

    [Header("Debug Info")]
    [TextArea(2, 3)]
    public string debugNotes = "placeholder card for testing";


    public int GetFinalDamage()
    {
        return Mathf.RoundToInt(baseDamage * multiplier);
    }

    public string GetCardDescription()
    {
        string desc = $"Damage: {GetFinalDamage()}";

        if (effectType != EffectType.None)
        {
            desc += $"\nEffect: {effectDescription}";
        }
        return desc;
    }

    public void ApplyEffect()
    {
        switch (effectType)
        {
            case EffectType.MultiplyDamage:
                // Will be implemented when ClockManager is ready
                Debug.Log($"Applying multiply damage effect: x{effectValue}");
                break;
            case EffectType.ReverseOrder:
                Debug.Log($"Applying reverse order effect");
                break;
            case EffectType.IncreaseClockwise:
                Debug.Log($"Applying clockwise boost: +{effectValue}");
                break;
            case EffectType.IncreaseCounterClockwise:
                Debug.Log($"Applying counter-clockwise boost: +{effectValue}");
                break;
        }
    }
}

