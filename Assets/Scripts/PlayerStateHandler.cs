using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateHandler : MonoBehaviour
{
    //public bool
    public GameStates State { get; private set; } = GameStates.Collecting;
    public Transform toReceptacles;

    public void LookAtReceptacles()
    {
        // TODO: move to centre of collider
        Debug.Log("lookatreceptacles");
        // Turn player to look at receptacles.
        transform.LookAt(toReceptacles);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(State);
        // I could have just set the state here but I wanted to keep all state-
        // related stuff in the StateManager itself.
        if (other.gameObject.CompareTag("RecyclingTrigger"))
            State = GameStates.Recycling;
    }
}
