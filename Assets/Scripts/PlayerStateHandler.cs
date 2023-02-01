using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
//using static UnityEditor.Experimental.GraphView.Port;

// Editors:				Szun Kidd Choi
// Date Created:		11/24/22
// Date Last Edited:	11/24/22

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeField, Tooltip("The game manager for the recycling minigame.")]
    private RecyclingGameManager recyclingGameManager;

    [SerializeField, Tooltip("The UI Manager.")]
    private UIManager uiManager;

    [SerializeField, Tooltip("The audio manager.")]
    private AudioManager audioManager;

    [SerializeField, Tooltip("The arrow pointing to the nearest piece of trash.")]
    private Arrow arrow;

    [SerializeField, Tooltip("The ShowUI script.")]
    private ShowUI showUI;
    
    /// <summary>
    /// Flags whether the player is in proximity to be able to play the 
    /// recycling minigame.
    /// </summary>
    private bool inRecyclingTrigger;

    /// <summary>
    /// Internal state, used to update the StateManager based on player actions.
    /// </summary>
    public GameStates State { get; private set; } = GameStates.Collecting;

    private void Start()
    {
        // Modify volume based on slider in settings menu.
        GetComponent<AudioSource>().volume = 0.15f * PersistentData.Instance.musicVolume;
        
        inRecyclingTrigger = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // I could have just set the state here but I wanted to explicitly handle
        // all state-related stuff in the StateManager itself.

        // Player is near the bins, and can choose to play the recycling
        // minigame.
        if (other.gameObject.CompareTag("RecyclingTrigger"))
            inRecyclingTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Player is too far away from the bins, and cannot play the recycling
        // minigame.
        if (other.gameObject.CompareTag("RecyclingTrigger"))
            inRecyclingTrigger = false;
    }

    /// <summary>
    /// Switches the player state to Collecting.
    /// </summary>
    public void SwitchToCollecting()
    {
        State = GameStates.Collecting;

        if (!showUI.finishedFirstRecyclingRun)
        {
            showUI.finishedFirstRecyclingRun = true;
            StartCoroutine(showUI.ShowInstructions8());
        }
    }

    /// <summary>
    /// Listen for ENTER keypress to switch states to Recycling if player is
    /// close enough to the bins.
    /// </summary>
    private void OnEnableRecycling(InputValue enableRecyclingValue)
    {
        if (inRecyclingTrigger && !GetComponent<Bag>().Empty)
        {
            State = GameStates.Recycling;

            if (!showUI.goneRecycling)
            {
                showUI.goneRecycling = true;
                showUI.ShowInstructions7();
            }
        }
    }

    /// <summary>
    /// Listen for ENTER keypress to switch states to Collecting if player is
    /// playing the recycling minigame.
    /// </summary>
    private void OnEnablePlayer(InputValue enablePlayerValue)
    {
        State = GameStates.Collecting;
    }

    /// <summary>
    /// Listen for 'A' keypress to throw trash in the landfill bin.
    /// </summary>
    private void OnLandfill(InputValue landfillValue)
    {
        // Yes, probably not the cleanest way to handle passing the return value
        // to the UI Manager, but it'll have to do for now.
        uiManager.ChoiceCorrectOrIncorrect = recyclingGameManager.Validate(false);
        // If the player has chosen correctly, play the collect sound effect
        // This needs to be changed if we want to add more trash objects
        // Also play the clip that tells the player they chose the correct bin.
        if (uiManager.ChoiceCorrectOrIncorrect == "Correct") {
            audioManager.PlayCorrectnessSFX(true);
            audioManager.PlayCollectSFX(TrashTypes.Cup);
        }
        // Play the clip that tells the player they chose the wrong bin.
        else
        {
            audioManager.PlayCorrectnessSFX(false);
        }
    }

    /// <summary>
    /// Listen for 'D' keypress to throw trash in the recycling bin.
    /// </summary>
    private void OnRecycle(InputValue landfillValue)
    {
        // Yes, probably not the cleanest way to handle passing the return value
        // to the UI Manager, but it'll have to do for now.
        uiManager.ChoiceCorrectOrIncorrect = recyclingGameManager.Validate(true);
        // If the player has chosen correctly, play the collect sound effect
        // This needs to be changed if we want to add more recyclable objects
        // Also play the clip that tells the player they chose the correct bin.
        if (uiManager.ChoiceCorrectOrIncorrect == "Correct") {
            audioManager.PlayCorrectnessSFX(true);
            audioManager.PlayCollectSFX(TrashTypes.Bottle);
        }
        // Play the clip that tells the player they chose the wrong bin.
        else
        {
            audioManager.PlayCorrectnessSFX(false);
        }
    }

    // I know this is not the best place to put this but for the sake of time
    // it will have to do.
    /// <summary>
    /// Toggles the arrow UI on and off.
    /// </summary>
    /// <param name="toggleUIValue"></param>
    private void OnToggle(InputValue toggleUIValue)
    {
        if (arrow.spriteRenderer.color.a != 0)
            arrow.spriteRenderer.color = new Color(1, 1, 1, 0);
        else
            arrow.spriteRenderer.color = new Color(1, 1, 1, Arrow.Opacity);
    }

    private void OnMove(InputValue moveValue)
    {
        //showUI.ShowInstructions();
        if (!showUI.showedInstructions0to3)
        {
            showUI.showedInstructions0to3 = true;
            StartCoroutine(showUI.ShowInstructions0to3());
        }
    }
}
