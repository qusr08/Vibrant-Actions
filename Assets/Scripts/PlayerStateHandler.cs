using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

// Editors:				Szun Kidd Choi
// Date Created:		11/24/22
// Date Last Edited:	11/24/22

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeField, Tooltip("The game manager for the recycling minigame.")]
    private RecyclingGameManager recyclingGameManager;

    [SerializeField]
    private UIManager uiManager;
    
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

    public void SwitchToCollecting()
    {
        State = GameStates.Collecting;
    }

    /// <summary>
    /// Listen for ENTER keypress to switch states to Recycling if player is
    /// close enough to the bins.
    /// </summary>
    private void OnEnableRecycling(InputValue enableRecyclingValue)
    {
        if (inRecyclingTrigger && !GetComponent<Bag>().Empty) 
            State = GameStates.Recycling;
    }

    /// <summary>
    /// Listen for ENTER keypress to switch states to Collecting if player is
    /// playing the recycling minigame.
    /// </summary>
    private void OnEnablePlayer(InputValue enablePlayerValue)
    {
        State = GameStates.Collecting;
    }

    private void OnLandfill(InputValue landfillValue)
    {
        // Yes, probably not the cleanest way to handle passing the return value
        // to the UI Manager, but it'll have to do for now.
        uiManager.ChoiceCorrectOrIncorrect = recyclingGameManager.Validate(false);
    }

    private void OnRecycle(InputValue landfillValue)
    {
        // Yes, probably not the cleanest way to handle passing the return value
        // to the UI Manager, but it'll have to do for now.
        uiManager.ChoiceCorrectOrIncorrect = recyclingGameManager.Validate(true);
    }
}
