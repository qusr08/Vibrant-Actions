using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Editors:				Szun Kidd Choi
// Date Created:		11/24/22
// Date Last Edited:	11/24/22

public enum GameStates
{
    Menu,
    Collecting,
    Recycling
}

public class StateManager : MonoBehaviour
{
    /// <summary>
    /// Static singleton instance of this class.
    /// </summary>
    private static StateManager instance;

    [SerializeField, Tooltip("The camera attached to the first person controller.")] 
    private Camera mainCamera;

    [SerializeField, Tooltip("The camera active during the recycling minigame.")] 
    private Camera recyclingCamera;

    [SerializeField, Tooltip("The capsule representing the player character.")] 
    private GameObject playerCapsule;

    [SerializeField, Tooltip("The script handling changes of state within the " +
        "player object.")] 
    private PlayerStateHandler playerStateHandler;
    
    [SerializeField, Tooltip("The position the player resets to after exiting " +
        "the recycling minigame")] 
    private GameObject recyclingPosition;
    
    [SerializeField, Tooltip("Reference to the Player Input component attached " +
        "to the player capsule")] 
    private PlayerInput playerInput;

    [SerializeField, Tooltip("Arrow GameObject showing the nearest piece of " +
        "trash to the player in the scene.")] 
    private GameObject arrow;

    public GameStates State { get; set; }

    /// <summary>
    /// Getter for the static singleton instance.
    /// </summary>
    public static StateManager Instance
    {
        get
        {
            // Search for a component of type StateManager in the Scene.
            //
            // If FindObjectOfType() returns null, then the component does
            // not exist in the scene yet. Create an empty GameObject and
            // attach the component to it, since all executable code must
            // be attached to an active GameObject within the Hierarchy.
            //
            // This way, we "lazily" instantiate the singleton (i.e. only
            // create it when it is needed. Therefore, the component, and
            // therefore the GameObject it needs to be attached to, do not
            // need to exist in the Scene beforehand.
            if (!instance)
            {
                instance = FindObjectOfType<StateManager>();

                GameObject gameObject = new GameObject();
                gameObject.name = typeof(StateManager).Name;
                instance = gameObject.AddComponent<StateManager>();
            }

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the State Manager persists between gameplay and menu screens.
        DontDestroyOnLoad(this);

        // We don't have a menu screen yet, so just begin with the collection
        // phase for now.
        State = GameStates.Collecting;

        recyclingCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            // TODO: Implement this when Menu UI is added.
            case GameStates.Menu:
                Debug.LogError("This shouldn't happen.");
                break;

            // Collecting phase
            case GameStates.Collecting:
                // Trigger is player state handler changing state.
                // Switch to recycling state.
                if (playerStateHandler.State == GameStates.Recycling)
                {
                    //Debug.Log("switching state");
                    State = GameStates.Recycling;

                    // To manually set the position of a GameObject with a
                    // CharacterController attached, disable the CharacterController,
                    // move the object, then re-enable the CharacterController.
                    playerCapsule.GetComponent<CharacterController>().enabled = false;
                    playerCapsule.transform.position = new Vector3(
                        recyclingPosition.transform.position.x,
                        recyclingPosition.transform.position.y,
                        playerCapsule.transform.position.z);
                    playerCapsule.GetComponent<CharacterController>().enabled = true;

                    // Don't show the arrow during the recycling minigame
                    arrow.SetActive(false);

                    // Switch cameras.
                    mainCamera.enabled = !mainCamera.enabled;
                    recyclingCamera.enabled = !recyclingCamera.enabled;

                    // Switch input action maps to listen for inputs relevant
                    // to the recycling minigame.
                    playerInput.SwitchCurrentActionMap("Recycling");
                }

                break;

            // Recycling minigame
            case GameStates.Recycling:
                // Trigger is player state handler changing state.
                // Switch to collecting state.
                if (playerStateHandler.State == GameStates.Collecting)
                {
                    //Debug.Log("switching state");
                    State = GameStates.Collecting;

                    // Switch input action maps to listen for inputs relevant
                    // to the collecting phase.
                    playerInput.SwitchCurrentActionMap("Player");

                    // Re-display the arrow.
                    arrow.SetActive(true);

                    // Switch cameras.
                    mainCamera.enabled = !mainCamera.enabled;
                    recyclingCamera.enabled = !recyclingCamera.enabled;
                    //Debug.Log("player enabled");
                }

                break;
        }
    }
}
