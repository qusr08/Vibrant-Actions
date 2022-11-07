using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Editors:				Szun Kidd Choi
// Date Created:		11/6/22
// Date Last Edited:	11/6/22

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Dynamically manages the trash objects in the Scene.
    /// </summary>
    private List<TrashController> trash;
    
    /// <summary>
    /// Returns the contents of the trash List as an IEnumerable to prevent
    /// external classes from modifying the List's contents.
    /// </summary>
    public IEnumerable<TrashController> Trash
    {
        get { return trash; }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Dynamically populate the list based on Scene contents.
        trash = FindObjectsOfType<TrashController>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        // Iterate through and destroy trash objects marked for removal.
        for (int i = trash.Count - 1; i >= 0; i--)
        {
            if (trash[i].Collected)
            {
                GameObject removed = trash[i].gameObject;
                trash.RemoveAt(i);
                Destroy(removed);
            }
        }
    }
}
