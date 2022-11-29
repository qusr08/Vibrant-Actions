using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecyclingGameManager : MonoBehaviour
{
    [SerializeField, Tooltip("The player's bag.")]
    private Bag bag;

    public string Validate(bool isRecyclable)
    {
        if (bag.Peek.Recyclable == isRecyclable)
        {
            bag.DiscardOne();
            return "Correct";
        }
        else return "Incorrect";
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
