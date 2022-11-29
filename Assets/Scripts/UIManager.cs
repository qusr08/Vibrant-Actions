using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField, Tooltip("UI text displaying bag capacity.")]
    private TextMeshProUGUI bagCapacity;

    [SerializeField, Tooltip("UI text telling the player to go recycling.")]
    private GameObject goRecycling;

    [SerializeField, Tooltip("The player's bag.")]
    private Bag bag;

    [SerializeField, Tooltip("Arrow pointing to the nearest piece of trash.")]
    private GameObject arrow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bagCapacity.text = string.Format("Bag: {0}/{1}", bag.Count, bag.Capacity);

        if (bag.Count == bag.Capacity)
        {
            goRecycling.SetActive(true);
            arrow.SetActive(false);
        }
        else
        {
            goRecycling.SetActive(false);
            arrow.SetActive(true);
        }
    }
}
