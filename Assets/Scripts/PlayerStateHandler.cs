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

    /// <summary>
    /// Listen for ENTER keypress to switch states to Recycling if player is
    /// close enough to the bins.
    /// </summary>
    private void OnEnableRecycling(InputValue enableRecyclingValue)
    {
        if (inRecyclingTrigger) State = GameStates.Recycling;
    }

    /// <summary>
    /// Listen for ENTER keypress to switch states to Collecting if player is
    /// playing the recycling minigame.
    /// </summary>
    private void OnEnablePlayer(InputValue enablePlayerValue)
    {
        State = GameStates.Collecting;
    }
}
