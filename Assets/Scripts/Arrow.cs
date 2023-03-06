using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Editors:				Szun Kidd Choi, Frank Alfano
// Date Created:		11/6/22
// Date Last Edited:	11/8/22

public class Arrow : MonoBehaviour
{
    [Tooltip("Reference to the TrashManager. Used to obtain the List of trash objects.")]
    [SerializeField] private TrashManager trashManager;

    /// <summary>
    /// The trash object nearest to the player.
    /// </summary>
    private TrashController nearest;

    /// <summary>
    /// Distance vector between nearest and the player.
    /// </summary>
    private Vector3 nearestVec;

    /// <summary>
    /// Distance vector between the trash object currently under consideration 
    /// and the player (see Update).
    /// </summary>
    private Vector3 currentVec;

    /// <summary>
    /// Sprite Renderer for the arrow sprite.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// The value at which to set the arrow sprite's alpha.
    /// </summary>
    public const float Opacity = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

	private void OnValidate ( ) {
        trashManager = FindObjectOfType<TrashManager>( );
	}

	// Update is called once per frame
	void Update()
    {
        nearest = null;

        if (trashManager.TrashCount > 0)
        {
            // Grab the first active object in the enumerable and use that for
            // comparison for the first loop iteration.
            for (int i = 0; i < trashManager.TrashCount; i++)
            {
                if (trashManager.TrashControllers[i].gameObject.activeSelf)
                {
                    nearest = trashManager.TrashControllers[i];
                    break;
                }
            }

            //nearest = trashManager.TrashControllers[0];

            if (nearest)
            {
                nearestVec = nearest.transform.position - transform.position;

                // Loop through the trash objects to find the closest object
                for (int i = 0; i < trashManager.TrashCount; i++) {
                    // If the piece of trash is not active (as in it has already been collected), do not try and point the arrow towards it
                    if (!trashManager.TrashControllers[i].gameObject.activeSelf) {
                        continue;
                    }

                    // Get the distance between the current trash and the arrow position
                    // Then check to see if that distance is less than the minimum distance found so far
                    currentVec = trashManager.TrashControllers[i].transform.position - transform.position;
                    if (currentVec.magnitude < nearestVec.magnitude) {
                        nearestVec = currentVec;
                    }
                }

                // Compute the angle at which to rotate the arrow to produce a
                // compass-like effect.
                float angle = Mathf.Atan2(nearestVec.x, nearestVec.z) * Mathf.Rad2Deg + 180;
            
                // Rotate the arrow.
                transform.eulerAngles = new Vector3(-90, 0, angle);
            }
            //else
            //{
            //    Debug.Log("all objects collected");
            //}
        }
    }

    
}
