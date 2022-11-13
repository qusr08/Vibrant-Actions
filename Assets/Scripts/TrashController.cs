using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Editors:				Frank Alfano, Szun Kidd Choi
// Date Created:		10/25/22
// Date Last Edited:	11/8/22

public class TrashController : MonoBehaviour
{
    [Tooltip("The radius of influence of the trash. Size of the grayscale around the trash.")]
    [SerializeField][Min(0)] public float Radius;

    [Space]

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

    [Tooltip("Time to move from the initial position to the player, in seconds.")]
    [SerializeField][Min(0)] private float journeyTime;

    /// <summary>
    /// The centre of the arc formed between this trash object and the player.
    /// </summary>
    private Vector3 centre;
    private float startTime;


    /// <summary>
    /// Used to identify when to start moving this trash object toward the player.
    /// </summary>
    private bool collected;

    private void Start()
    {
        collected = false;
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
        // Only animate movement once the player clicks on this trash object.
        if (collected)
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
