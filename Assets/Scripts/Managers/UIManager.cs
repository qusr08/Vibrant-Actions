using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField, Tooltip("Displays bag capacity.")]
    private TextMeshProUGUI bagCapacity;

    [SerializeField, Tooltip("Tells the player to go recycling.")]
    private GameObject goRecycling;

    [SerializeField, Tooltip("Shows the topmost item in the bag.")]
    private GameObject currentItem;

    [SerializeField, Tooltip("Tells the player whether their waste management " +
        "decision was correct or incorrect.")]
    private GameObject correctOrIncorrect;

    [SerializeField, Tooltip("The player's bag.")]
    private Bag bag;

    [SerializeField, Tooltip("Arrow pointing to the nearest piece of trash.")]
    private GameObject arrow;

    public string ChoiceCorrectOrIncorrect { get; set; }


    /// <summary>
    /// Displays UI related to the collection phase.
    /// </summary>
    public void ShowCollectionUI()
    {
        goRecycling.SetActive(true);
        arrow.SetActive(true);
    }


    /// <summary>
    /// Hides UI related to the collection phase.
    /// </summary>
    public void HideCollectionUI()
    {
        goRecycling.SetActive(false);
        arrow.SetActive(false);
    }

    /// <summary>
    /// Updates UI related to the collection phase.
    /// </summary>
    public void UpdateCollectionUI()
    {
        if (bag.Full)
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

    /// <summary>
    /// Displays UI related to the recycling minigame.
    /// </summary>
    public void ShowRecyclingUI()
    {
        currentItem.SetActive(true);
        correctOrIncorrect.SetActive(true);

        // Show the topmost item in the bag.
        currentItem.GetComponent<TextMeshProUGUI>().text = string.Format(
            "Current Item: {0}",
            bag.Peek.TrashType.ToString());

        // This method gets called when the minigame starts. Default to an
        // empty string.
        correctOrIncorrect.GetComponent<TextMeshProUGUI>().text = string.Empty;
    }

    /// <summary>
    /// Hides UI related to the recycling minigame.
    /// </summary>
    public void HideRecyclingUI()
    {
        currentItem.SetActive(false);
        correctOrIncorrect.SetActive(false);
        ChoiceCorrectOrIncorrect = string.Empty;
    }

    /// <summary>
    /// Updates UI related to the recycling minigame.
    /// </summary>
    public void UpdateRecyclingUI()
    {
        if (!bag.Empty)
        {
            // Show the topmost item in the bag.
            currentItem.GetComponent<TextMeshProUGUI>().text = string.Format(
                "Current Item: {0}",
                bag.Peek.TrashType.ToString());
        }

        // Show whether the player's most recent choice was correct or not.
        correctOrIncorrect.GetComponent<TextMeshProUGUI>().text = 
            ChoiceCorrectOrIncorrect;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Bag capacity is always displayed regardless of state.
        bagCapacity.text = string.Format("Bag: {0}/{1}", bag.Count, bag.Capacity);
    }
}
