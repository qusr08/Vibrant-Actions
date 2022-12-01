using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField, Tooltip("Maximum pieces of trash the bag can hold.")]
    private int capacity;
    [SerializeField] private TextMeshProUGUI bagCapacityText;

    /// <summary>
    /// Internally tracks the trash objects the player has collected.
    /// </summary>
    private Stack<TrashController> bag;

    /// <summary>
    /// Getter for maximum bag capacity.
    /// </summary>
    public int Capacity { get { return capacity; } }

    /// <summary>
    /// Getter for count of the underlying stack.
    /// </summary>
    public int Count { get { return bag.Count; } }

    /// <summary>
    /// Getter for whether count of underlying stack is zero.
    /// </summary>
    public bool Empty { get { return bag.Count == 0; } }

    /// <summary>
    /// Getter for whether count of underlying stack equals capacity.
    /// </summary>
    public bool Full { get { return bag.Count == capacity; } }

    /// <summary>
    /// Returns most recently collected trash object.
    /// </summary>
    public TrashController Peek
    {
        get 
        { 
            // Explicitly check stack length to avoid exceptions being thrown.
            if (bag.Count > 0) return bag.Peek();
            else return null;
        }
    }

    /// <summary>
    /// Removes items from the bag, one at a time. Brings player back to
    /// collection phase if there is nothing left to recycle.
    /// </summary>
    public void DiscardOne()
    {
        // Explicitly check stack length to avoid exceptions being thrown.
        if (bag.Count > 0)
        {
            // Begin the animation for the trash object flying into the bin.
            bag.Pop().Discard();

            UpdateBagUI();

            // There is nothing left to recycle. Transition player back to
            // collection phase.
            if (bag.Count == 0)
            {
                Debug.Log("Emptied bag! Returning to collection phase now.");
                GetComponent<PlayerStateHandler>().SwitchToCollecting();
            }
        }
    }

    /// <summary>
    /// Only allows the collection of trash if the bag is not at capacity.
    /// </summary>
    /// <param name="trash">
    /// TrashController script of the trash object to be collected.
    /// </param>
    /// <returns>
    /// True if bag was not full at time of collection, false otherwise.
    /// </returns>
    public bool Collect(TrashController trash)
    {
        if (bag.Count < capacity)
        {
            bag.Push(trash);
            UpdateBagUI( );

            return true;
        }
        else return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        bag = new Stack<TrashController>();
        
        // Make sure the bag shows the right capacity at the start of the game
        UpdateBagUI( );
    }

    /// <summary>
    /// Update the bag ui numbers.
    /// </summary>
    private void UpdateBagUI () {
        // Set the UI text to reflect how full the player's bag is and the capacity of the bag
        bagCapacityText.text = $"{Count} / {Capacity}";
    }
}
