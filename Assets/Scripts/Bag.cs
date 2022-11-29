using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bag : MonoBehaviour
{
    [SerializeField, Tooltip("Maximum pieces of trash the bag can hold.")]
    private int capacity;

    /// <summary>
    /// Internally tracks the trash objects the player has collected.
    /// </summary>
    private List<TrashController> bag;

    /// <summary>
    /// Getter for maximum bag capacity.
    /// </summary>
    public int Capacity { get { return capacity; } }

    /// <summary>
    /// Getter for count of the underlying list.
    /// </summary>
    public int Count { get { return bag.Count; } }

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
            bag.Add(trash);
            return true;
        }
        else return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        bag = new List<TrashController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
