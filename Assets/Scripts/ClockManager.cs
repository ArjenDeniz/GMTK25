// ClockManager controls the core game loop

// Cycles through the 6 clock slots in order (clockwise or counter-clockwise),
// Activates each card, applying its damage and any special effects (like reversing direction or boosting others),
// Tracks loops remaining and boss health,
// Ends the game when the boss is defeated or loops run out.

//Pseudocode is pasted below if there is anything you want to understand about the logic

//TODO: Could not test the code yet. These should be done first.
//The first 6 cards layout logic (create card slots and update drag and drop logic to fill those slots)
//Loop starting button should be created, (startloop() function here should be added to that button)
//commented out new applyeffect() in card.cs should be uncommented


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockManager : MonoBehaviour
{
    [Header("Clock Setup")]
    public List<Card> clockSlots = new List<Card>(6); // Cards placed into the 6 clock slots
    public int currentIndex = 0;
    public bool isClockwise = true;

    [Header("Game State")]
    public int loopsRemaining = 3;
    public int bossHealth = 100;
    public bool loopInProgress = false;

    public delegate void OnGameEnd(bool victory);
    public event OnGameEnd GameEnded;

    public void StartLoop()
    {
        if (loopInProgress)
        {
            Debug.LogWarning("Loop already in progress!");
            return;
        }

        if (clockSlots.Count < 6 || clockSlots.Contains(null))
        {
            Debug.LogWarning("Not all clock slots are filled!");
            return;
        }

        StartCoroutine(RunClockLoop());
    }

    private IEnumerator RunClockLoop()
    {
        loopInProgress = true;
        Debug.Log($"🔁 Starting Loop | Loops Left: {loopsRemaining}, Boss HP: {bossHealth}");

        int loopStartIndex = currentIndex;

        do
        {
            Card currentCard = clockSlots[currentIndex];

            Debug.Log($"➡️ Activating '{currentCard.cardName}' at index {currentIndex}");

            // Deal damage
            int damage = currentCard.GetFinalDamage();
            bossHealth -= damage;
            Debug.Log($"💥 '{currentCard.cardName}' deals {damage} damage! Boss HP now: {bossHealth}");

            // Apply effect (can alter rotation or other card values)
            currentCard.ApplyEffect(ref isClockwise, clockSlots, currentIndex);

            // Optional: trigger animations, visual feedback, sound here

            yield return new WaitForSeconds(0.5f); // pacing between cards

            currentIndex = GetNextIndex();

        } while (currentIndex != loopStartIndex && bossHealth > 0);

        loopsRemaining--;

        if (bossHealth <= 0)
        {
            Debug.Log("🎉 Victory! Boss defeated.");
            GameEnded?.Invoke(true);
        }
        else if (loopsRemaining <= 0)
        {
            Debug.Log("💀 Game Over. You ran out of loops.");
            GameEnded?.Invoke(false);
        }
        else
        {
            Debug.Log($"🔁 Loop complete. {loopsRemaining} loop(s) left.");
        }

        loopInProgress = false;
    }

    private int GetNextIndex()
    {
        return isClockwise
            ? (currentIndex + 1) % clockSlots.Count
            : (currentIndex - 1 + clockSlots.Count) % clockSlots.Count;
    }
}



// function StartGameLoop():
//     while bossHP > 0 and loopsRemaining > 0:
//         RunOneFullLoop()

//     if bossHP <= 0:
//         print "Victory!"
//     else:
//         print "Defeat"

// function RunOneFullLoop():
//     loopStartIndex = currentIndex

//     do:
//         currentCard = clockSlots[currentIndex]

//         print "Activating", currentCard.name

//         // Damage phase
//         damage = currentCard.baseDamage * currentCard.multiplier
//         bossHP -= damage
//         print "Boss takes", damage, "damage"

//         // Apply card effect that may alter state
//         ApplyEffect(ref isClockwise, clockSlots, currentIndex)

//         currentIndex = GetNextIndex(currentIndex, isClockwise)
    
//     while currentIndex != loopStartIndex and bossHP > 0

//     loopsRemaining -= 1

// function ApplyEffect(ref isClockwise, clockSlots, index):
//     card = clockSlots[index]

//     if card.effectType == MultiplyDamage:
//         card.multiplier *= card.effectValue
//         print "Card damage multiplied by", card.effectValue

//     else if card.effectType == ReverseOrder:
//         isClockwise = not isClockwise
//         print "Rotation reversed!"

//     else if card.effectType == IncreaseClockwise:
//         target = (index + 1) mod clockSlots.length
//         clockSlots[target].multiplier += card.effectValue
//         print "Boosted clockwise card"

//     else if card.effectType == IncreaseCounterClockwise:
//         target = (index - 1 + clockSlots.length) mod clockSlots.length
//         clockSlots[target].multiplier += card.effectValue
//         print "Boosted counter-clockwise card"

// function GetNextIndex(index, isClockwise):
//     if isClockwise:
//         return (index + 1) mod clockSlots.length
//     else:
//         return (index - 1 + clockSlots.length) mod clockSlots.length

