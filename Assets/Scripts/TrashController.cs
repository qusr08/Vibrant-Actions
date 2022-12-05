using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Editors:				Frank Alfano, Szun Kidd Choi
// Date Created:		10/25/22
// Date Last Edited:	11/12/22

public enum TrashTypes
{
    Bottle,
    Cup
}

public class TrashController : MonoBehaviour
{
    [Tooltip("The radius of influence of the trash. Size of the grayscale around the trash.")]
    [SerializeField][Min(0)] public float Radius;

    [Space]

    [Tooltip("The camera active during the recycling minigame.")]
    [SerializeField] private Camera recyclingCamera;

    [Tooltip("The trash area object. The sphere that surrounds a trash object.")]
    [SerializeField] private GameObject trashArea;

    [Tooltip("The targets that this trash object will effect when it is collected.")]
    [SerializeField] private List<GameObject> targets;

    [Tooltip("A reference to the player object.")]
    [SerializeField] private GameObject player;

    [Tooltip("The speed by which this trash object moves towards the player " +
        "after it is collected.")]
    [SerializeField] private float speed;

    [Tooltip("The minimum distance this trash object must be from the player " +
        "before it is deactivated in the Scene.")]
    [SerializeField][Min(0)] private float minDist;

    [Tooltip("The minimum distance this trash object must be from the receptacle " +
        "before it is deactivated in the Scene.")]
    [SerializeField][Min(0)] private float receptacleMinDist;

    [Tooltip("Time to move from the initial position to the player, in seconds.")]
    [SerializeField][Min(0)] private float journeyTime;

    [Tooltip("The type of trash item this object is.")]
    [SerializeField] private TrashTypes trashType;

    [Tooltip("Transform of the landfill receptacle.")]
    [SerializeField] private Transform[ ] landfillReceptacles;

    [Tooltip("Transform of the recycling receptacle.")]
    [SerializeField] private Transform[] recyclingReceptacles;

    [Tooltip("Whether this trash object is recyclable.")]
    [SerializeField] private bool recyclable;

    /// <summary>
    /// Transform of the "correct" bin this trash object must be thrown into.
    /// </summary>
    private Transform closestCorrectReceptacle;

    /// <summary>
    /// Getter for whether this trash object is recyclable.
    /// </summary>
    public bool Recyclable { get { return recyclable; } }

    /// <summary>
    /// The centre of the arc formed between this trash object and the player.
    /// </summary>
    private Vector3 centre;

    /// <summary>
    /// Time at which the animation begins.
    /// </summary>
    private float startTime;

    /// <summary>
    /// Used to identify when to start moving this trash object toward the 
    /// player.
    /// </summary>
    private bool collected;

    /// <summary>
    /// Used to identify when to start moving this trash object toward the 
    /// receptacle.
    /// </summary>
    public bool discarded;

    /// <summary>
    /// The type of this trash object.
    /// </summary>
    public TrashTypes TrashType { get { return trashType; } }

    private void Start()
    {
        collected = false;

        // Assign the destination that this trash object will "fly" towards
        // during the recycling minigame. Assume that the trash object will
        // never fly into an incorrect receptacle.
        // if (recyclable) correctReceptacle = recyclingReceptacles;
        // else correctReceptacle = landfillReceptacles;
    }

    /// <summary>
    /// Called when this trash object is thrown into the correct receptacle.
    /// </summary>
    public void Discard()
    {
        gameObject.SetActive(true);

        // Throw from the recycling camera's position
        transform.position = recyclingCamera.transform.position;

        // Find the closest recepticle
        float minDistance = float.MaxValue;
        if (recyclable) {
            closestCorrectReceptacle = recyclingReceptacles[0];
            for (int i = 1; i < recyclingReceptacles.Length;i++) {
                if (Vector3.Distance(recyclingReceptacles[i].position, player.transform.position) < minDistance) {
                    minDistance = Vector3.Distance(recyclingReceptacles[i].position, player.transform.position);
                    closestCorrectReceptacle = recyclingReceptacles[i];
                }
			}
        } else {
            closestCorrectReceptacle = landfillReceptacles[0];
            for (int i = 1; i < landfillReceptacles.Length; i++) {
                if (Vector3.Distance(landfillReceptacles[i].position, player.transform.position) < minDistance) {
                    minDistance = Vector3.Distance(landfillReceptacles[i].position, player.transform.position);
                    closestCorrectReceptacle = landfillReceptacles[i];
                }
            }
        }

        // Begin the animation and get the time at which it starts.
        discarded = true;
        startTime = Time.time;
    }

    /// <summary>
    /// Called when this trash object is considered "collected".
    /// </summary>
    public void Collect()
    {
        // For each of the targets that are connected to the trash object ...
        foreach (GameObject target in targets)
        {
            // Get the target's material so we can set the color of it
            // Creating a copy of the material is important to make this work,
            //		otherwise all of the object will turn the same color at once since they
            //		all use the same material
            Material targetMaterial = target.GetComponent<MeshRenderer>().material;

            // Generate a color that is colorful and not monotone
            /// TO DO: Right now, if all three color channels generate 0.3f, the color will
            ///		be monotone. Not too sure how to fix this at the moment.
            float r = Random.Range(0f, 1f);
            float g = Random.Range(0f, 1f - r);
            float b = Random.Range(0f, 1f - r - g);
            targetMaterial.color = new Color(r, g, b);

            // Set the material of the target to the new color
            target.GetComponent<MeshRenderer>().material = targetMaterial;
        }

        //// Destroy this trash object
        // Destroy(gameObject);

        // Begin the animation and get the time at which it starts.
        collected = true;
        startTime = Time.time;
    }

    private void Update()
    {
        if (collected)
        {
            // The player has clicked on this trash object during the collection phase.
            if (!discarded)
            {
                // Move this trash object toward the player until it is close enough.
                if ((transform.position - player.transform.position).magnitude > minDist)
                {
                    // The centre of the arc is the midpoint between this trash
                    // object and the player.
                    centre = (transform.position + player.transform.position) * 0.5f;

                    // Move the centre downwards slightly to make the arc vertical.
                    centre -= new Vector3(0, 1, 0);

                    // Interpolate over the arc relative to the centre.
                    Vector3 relCenter = transform.position - centre;
                    Vector3 playerRelCenter = player.transform.position - centre;

                    // The fraction of the animation that has happened so far is
                    // equal to the elapsed time divided by the desired time for
                    // the total journey.
                    float fracComplete = (Time.time - startTime) / journeyTime;

                    // Slerp treats initial and target vectors as directions rather
                    // than points in space.
                    transform.position = Vector3.Slerp(
                        relCenter,
                        playerRelCenter,
                        fracComplete);
                    transform.position += centre;
                }
                // Once it is close enough, deactivate it in the Scene.
                else gameObject.SetActive(false);
            }
            // The player has thrown this trash object into the correct receptacle.
            else
            {
                // Move this trash object toward the receptacle until it is close enough.
                if ((transform.position - closestCorrectReceptacle.position).magnitude > receptacleMinDist)
                {
                   // Debug.Log((transform.position - closestCorrectReceptacle.position).magnitude);
                    // The centre of the arc is the midpoint between this trash
                    // object and the correct receptacle.
                    centre = (transform.position + closestCorrectReceptacle.position) * 0.5f;

                    // Move the centre downwards slightly to make the arc vertical.
                    centre -= new Vector3(0, 1, 0);

                    // Interpolate over the arc relative to the centre.
                    Vector3 relCenter = transform.position - centre;
                    Vector3 receptacleRelCentre = closestCorrectReceptacle.position - centre;

                    // The fraction of the animation that has happened so far is
                    // equal to the elapsed time divided by the desired time for
                    // the total journey.
                    float fracComplete = (Time.time - startTime) / journeyTime;

                    // Slerp treats initial and target vectors as directions rather
                    // than points in space.
                    transform.position = Vector3.Slerp(
                        relCenter,
                        receptacleRelCentre,
                        fracComplete);
                    transform.position += centre;
                }
                // Once it is close enough, deactivate it in the Scene.
                else gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called each time Unity editor is updated.
    /// </summary>
    private void OnValidate()
    {
        // Update the trash area object based on values set for the trash
        if (trashArea == null)
        {
            Debug.LogWarning("Trash Area [Transform] is not set!");
        }
        else
        {
            // Set the size of the trash area
            trashArea.transform.localScale = Radius * 2 * Vector3.one;
        }
    }
}
