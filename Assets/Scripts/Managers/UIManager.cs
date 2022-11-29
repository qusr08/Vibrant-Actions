using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    [SerializeField, Tooltip("The ui for the recycling minigame.")]
    private GameObject recyclingMinigameUI;

    [SerializeField, Tooltip("Tells the player to go recycling.")]
    private GameObject goRecycling;

    [SerializeField, Tooltip("Shows the topmost item in the bag.")]
    private GameObject currentItem;

    [SerializeField, Tooltip("Tells the player whether their waste management " +
        "decision was correct or incorrect.")]
    private GameObject correctOrIncorrect;

    [SerializeField, Tooltip("The sprite of the current item for the recycling minigame.")]
    private Image trashThumbnailImage;
    [SerializeField] private Sprite bottleSprite;
    [SerializeField] private Sprite coffeeCupSprite;

    [SerializeField, Tooltip("The player's bag.")]
    private Bag bag;

    [SerializeField, Tooltip("Arrow pointing to the nearest piece of trash.")]
    private GameObject arrow;


    /// <summary>
    /// Captures the return value of RecyclingGameManager.Validate().
    /// </summary>
    private string _choiceCorrectOrIncorrect = "";
    public string ChoiceCorrectOrIncorrect {
        get {
            return _choiceCorrectOrIncorrect;
        }

        set {
            _choiceCorrectOrIncorrect = value;

            StopAllCoroutines( );
            StartCoroutine(UpdateCorrectOrIncorrect( ));
        } 
    }

    ///// <summary>
    ///// Displays UI related to the collection phase.
    ///// </summary>
    //public void ShowCollectionUI()
    //{
    //    goRecycling.SetActive(true);
    //    arrow.SetActive(true);
    //}

    /// <summary>
    /// Hides UI related to the collection phase.
    /// </summary>
    public void HideCollectionUI()
    {
        goRecycling.SetActive(false);
        arrow.SetActive(false);
    }

    /// <summary>
    /// Displays and updates UI related to the collection phase.
    /// </summary>
    public void UpdateCollectionUI()
    {
        // Bag is full. Tell player to go recycling and hide arrow.
        if (bag.Full)
        {
            goRecycling.SetActive(true);
            arrow.SetActive(false);
        }
        // Bag is not full. Hide text telling player to go recycling and
        // display the arrow.
        else
        {
            goRecycling.SetActive(false);
            arrow.SetActive(true);
        }
    }

    /// <summary>
    /// Displays UI related to the recycling minigame.
    /// </summary>
    public void ShowRecyclingUI() {
        recyclingMinigameUI.SetActive(true);
        ChoiceCorrectOrIncorrect = "";
        UpdateRecyclingUI( );
    }

    /// <summary>
    /// Hides UI related to the recycling minigame.
    /// </summary>
    public void HideRecyclingUI() {
        recyclingMinigameUI.SetActive(false);
        ChoiceCorrectOrIncorrect = "";
    }

    /// <summary>
    /// Updates UI related to the recycling minigame.
    /// </summary>
    public void UpdateRecyclingUI()
    {
        if (!bag.Empty)
        {
            // Show the topmost item in the bag.
            currentItem.GetComponent<TextMeshProUGUI>( ).text = bag.Peek.TrashType.ToString( );

            // Set the sprite of the recycling minigame based on the trash type
            // For more trash types, add to this switch statement with more references to each trash's thumbnail sprite
            switch (bag.Peek.TrashType) {
				case TrashTypes.Bottle:
                    trashThumbnailImage.sprite = bottleSprite;

                    break;
				case TrashTypes.Cup:
                    trashThumbnailImage.sprite = coffeeCupSprite;

                    break;
				default:
                    trashThumbnailImage.sprite = null;

                    break;
			}
		}
    }

    /// <summary>
    /// Update the correct/incorrect text and have it disappear after a certain amount of seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateCorrectOrIncorrect () {
        correctOrIncorrect.SetActive(true);

        // Show whether the player's most recent choice was correct or not.
        correctOrIncorrect.GetComponent<TextMeshProUGUI>( ).text = ChoiceCorrectOrIncorrect;

        // Wait 0.75 second before disabling the text
        yield return new WaitForSeconds(0.75f);

        correctOrIncorrect.SetActive(false);
    }
}
