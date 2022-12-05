using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Editors:				Szun Kidd Choi
// Date Created:		10/29/22
// Date Last Edited:	11/1/22

public class ReticleRaycast : MonoBehaviour
{
    ///// <summary>
    ///// UI Manager Script.
    ///// </summary>
    //[SerializeField] private UIManager uIManager;

    /// <summary>
    /// Reticle image asset.
    /// </summary>
    [SerializeField] Image reticle;

    /// <summary>
    /// Maximum distance from player at which Raycast can check for collisions.
    /// </summary>
    [SerializeField] private float maxDistance;

    /// <summary>
    /// Camera attached to the FirstPersonCharacter GameObject.
    /// </summary>
    [SerializeField] private Camera cam;

    /// <summary>
    /// The player's bag.
    /// </summary>
    private Bag bag;

    /// <summary>
    /// Width of the reticle image asset.
    /// </summary>
    private float reticleWidth;

    /// <summary>
    /// Height of the reticle image asset.
    /// </summary>
    private float reticleHeight;

    /// <summary>
    /// x-coordinate of the centre of the screen.
    /// </summary>
    private float centreX;

    /// <summary>
    /// y-coordinate of the centre of the screen.
    /// </summary>
    private float centreY;

    /// <summary>
    /// out variable used to store information of objects the Raycast collides 
    /// with, if any.
    /// </summary>
    private RaycastHit hit;

    /// <summary>
    /// Index of layer at which to check for collisions with the Raycast.
    /// </summary>
    private int layerIndex;

    /// <summary>
    /// Layer mask to Raycast to.
    /// </summary>
    private int layerMask;

    [SerializeField, Tooltip("The ShowUI script.")]
    private ShowUI showUI;

    // Start is called before the first frame update
    void Start()
    {
        // The bag is attached to the same component as this script.
        bag = GetComponent<Bag>();
        
        // Raycast from the centre of the screen, accounting for the reticle's
        // dimensions.
        reticleWidth = reticle.rectTransform.rect.width;
        reticleHeight = reticle.rectTransform.rect.height;
        centreX = Screen.width / 2 - reticleWidth / 2;
        centreY = Screen.width / 2 - reticleHeight / 2;

        // Get the index of the Interactables layer.
        layerIndex = LayerMask.NameToLayer("Interactables");

        // If the index does not exist, NameToLayer() returns -1. Handle this
        // error case explicitly.
        if (layerIndex == -1) Debug.LogError("Layer does not exist");

        // Otherwise, the index exists. Calculate the layer mask to Raycast to,
        // by bitshifting 1 by the layer's index to obtain a bitmask.
        else layerMask = 1 << layerIndex;
    }

    private void Update()
    {

    }

    // Use FixedUpdate since a Raycast is a physics-related query.
    void FixedUpdate()
    {

    }

    private void OnInteract(InputValue clickValue)
    {
        Cast();
    }

    /// <summary>
    /// Cast a ray on an object to determine whether to collect it or not.
    /// </summary>
    public void Cast(/*bool isEnabled*/)
    {
        // Draw a ray for debugging purposes.
        //Debug.DrawRay(
        //    cam.ScreenToWorldPoint(new Vector3(centreX, centreY, 0)), 
        //    cam.transform.forward * maxDistance);

        // Using overload 15 of Physics.Raycast():
        // a) set the origin of the Raycast to be the centre of the screen;
        // b) the Raycast travels in the direction the camera is facing; and
        // c) check for collisions at the layer represented by the layer mask.
        bool raycastCollided = Physics.Raycast(
            cam.ScreenToWorldPoint(new Vector3(centreX, centreY, 0)),   // a
            cam.transform.forward,                                      // b
            out hit,
            maxDistance,
            layerMask);                                                 // c

        // Click on trash when close enough to it to collect.
        if (raycastCollided && hit.transform.CompareTag("Trash"))
        {
            TrashController tc = hit.transform.gameObject.GetComponent<TrashController>();

            // Only allow the player to collect the piece of trash if the bag
            // is not already full.
            if (bag.Collect(tc))
            {
                if (!showUI.collectedFirstPieceOfTrash
                    && showUI.finishedShowingInstructions0to3)
                {
                    showUI.collectedFirstPieceOfTrash = true;
                    StartCoroutine(showUI.ShowInstructions4to6());
                }

                tc.Collect();
                AudioManager.instance.PlayCollectSFX(tc.TrashType);
            }
        }
    }
}
