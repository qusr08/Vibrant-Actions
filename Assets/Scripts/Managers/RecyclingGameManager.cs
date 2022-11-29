using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclingGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player's bag.")]
    private Bag bag;

    /// <summary>
    /// Determines whether the player's choice to place the trash object in the
    /// recycling or landfill bin is correct.
    /// </summary>
    /// <param name="isRecyclable">
    /// Player opinion on whether the current trash object is recyclable.
    /// </param>
    /// <returns>
    /// "Correct" if player chooses correctly, "Incorrect" otherwise.
    /// </returns>
    public string Validate(bool isRecyclable)
    {
        if (bag.Peek.Recyclable == isRecyclable)
        {
            bag.DiscardOne();
            return "Correct";
        }
        else return "Incorrect";
    }
}
