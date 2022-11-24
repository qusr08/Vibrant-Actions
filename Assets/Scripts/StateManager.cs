using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    Menu,
    Collecting,
    Recycling
}

public class StateManager : MonoBehaviour
{
    private static StateManager instance;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerStateHandler playerStateHandler;

    public GameStates State { get; set; }

    public static StateManager Instance
    {
        get
        {
            //// Search for a component of type T in the Scene.
            ////
            //// If FindObjectOfType() returns null, then the component does
            //// not exist in the scene yet. Create an empty GameObject and
            //// attach the component to it, since all executable code must
            //// be attached to an active GameObject within the Hierarchy.
            ////
            //// This way, we "lazily" instantiate the singleton (i.e. only
            //// create it when it is needed. Therefore, the component, and
            //// therefore the GameObject it needs to be attached to, do not
            //// need to exist in the Scene beforehand.
            //if (!instance)
            //{
            //    instance = FindObjectOfType<StateManager>();

            //    GameObject gameObject = new GameObject();
            //    gameObject.name = typeof(StateManager).Name;
            //    instance = gameObject.AddComponent<StateManager>();
            //}

            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        // We don't have a menu screen yet, so just begin with the collection
        // phase for now.
        State = GameStates.Collecting;
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case GameStates.Menu:
                Debug.LogError("This shouldn't happen.");
                break;
            case GameStates.Collecting:
                if (playerStateHandler.State == GameStates.Recycling)
                {
                    Debug.Log("turning");
                    // Turn player to look at receptacles.
                    playerStateHandler.LookAtReceptacles();

                    Debug.Log("switching state");
                    State = GameStates.Recycling;
                }

                break;
            case GameStates.Recycling:
                // trigger to switch to collecting
                break;
        }
    }
}
