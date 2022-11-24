using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject recyclingPosition;
    [SerializeField] private GameObject playerCapsule;
    [SerializeField] private Camera recyclingCamera;
    [SerializeField] private Camera mainCamera;
    private PlayerInput playerInput;
    public bool inRecyclingTrigger;
    public GameStates State { get; private set; } = GameStates.Collecting;
    public Transform toReceptacles;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        inRecyclingTrigger = false;
        recyclingCamera.enabled = false;
    }

    public void LookAtReceptacles()
    {
        // TODO: move to centre of collider
        Debug.Log("lookatreceptacles");
        // Turn player to look at receptacles.
        playerCapsule.transform.LookAt(toReceptacles);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(State);

        // I could have just set the state here but I wanted to explicitly handle
        // all state-related stuff in the StateManager itself.
        if (other.gameObject.CompareTag("RecyclingTrigger"))
            inRecyclingTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("RecyclingTrigger"))
            inRecyclingTrigger = false;
    }

    private void OnRecycle(InputValue startRecyclingValue)
    {
        //Debug.Log("recycling");
        State = GameStates.Recycling;
    }

    private void OnEnableRecycling(InputValue enableRecyclingValue)
    {
        //controls.playerCount 
        if (inRecyclingTrigger)
        {
            Debug.Log("recycling enabled");

            // To manually set the position of a GameObject with a
            // CharacterController attached, disable the CharacterController,
            // move the object, then re-enable the CharacterController.
            GetComponent<CharacterController>().enabled = false; //gameObject.SetActive(false);
            playerCapsule.transform.position = new Vector3(
                recyclingPosition.transform.position.x,
                recyclingPosition.transform.position.y,
                playerCapsule.transform.position.z);
            //LookAtReceptacles();
            GetComponent<CharacterController>().enabled = true; //gameObject.SetActive(true);

            mainCamera.enabled = !mainCamera.enabled;
            recyclingCamera.enabled = !recyclingCamera.enabled;

            playerInput.SwitchCurrentActionMap("Recycling");
        }
    }

    private void OnEnablePlayer(InputValue enablePlayerValue)
    {
        playerInput.SwitchCurrentActionMap("Player");
        mainCamera.enabled = !mainCamera.enabled;
        recyclingCamera.enabled = !recyclingCamera.enabled;
        Debug.Log("player enabled");
    }
}
