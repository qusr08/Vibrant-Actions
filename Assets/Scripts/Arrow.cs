using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

// Editors:				Szun Kidd Choi
// Date Created:		11/6/22
// Date Last Edited:	11/6/22

public class Arrow : MonoBehaviour
{
    [Tooltip("Reference to the GameManager. Used to obtain the List of trash objects.")]
    [SerializeField] private GameManager gameManager;
    [Tooltip("Reference to the Arrow UI GameObject.")]
    [SerializeField] private GameObject arrow;

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
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// The value at which to set the arrow sprite's alpha.
    /// </summary>
    private const float Opacity = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = arrow.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.Trash.Count() > 0)
        {
            // Grab the first object in the enumerable and use that for
            // comparison for the first loop iteration.
            nearest = gameManager.Trash.First();
            nearestVec = nearest.transform.position - arrow.transform.position;

            // Find the trash object closest to the player's position.
            foreach (TrashController tc in gameManager.Trash)
            {
                currentVec = tc.transform.position - arrow.transform.position;
                if (currentVec.magnitude < nearestVec.magnitude)
                {
                    nearestVec = currentVec;
                }
            }

            // Compute the angle at which to rotate the arrow to produce a
            // compass-like effect.
            float angle = Mathf.Atan2(nearestVec.x, nearestVec.z) * Mathf.Rad2Deg + 180;
            
            // Rotate the arrow.
            arrow.transform.eulerAngles = new Vector3(-90, 0, angle);

        }
    }

    /// <summary>
    /// Toggles the arrow UI on and off.
    /// </summary>
    /// <param name="toggleUIValue"></param>
    private void OnToggle(InputValue toggleUIValue)
    {
        if (spriteRenderer.color.a != 0)
            spriteRenderer.color = new Color(1, 1, 1, 0);
        else
            spriteRenderer.color = new Color(1, 1, 1, Opacity);
    }
}
