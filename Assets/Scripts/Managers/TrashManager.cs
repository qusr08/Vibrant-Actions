using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrashManager : MonoBehaviour {
	[Tooltip("Checking this box will force an update of the OnValidate method.")]
	[SerializeField] private bool CallOnValidate;

	public TrashController[ ] TrashControllers;
	private Vector4[ ] trashPositionData;
	private float[ ] trashRadiiData;

	/// <summary>
	/// The number of trash objects in the scene. All Trash objects should be a child to this trash manager object.
	/// </summary>
	public int TrashCount {
		get {
			return TrashControllers.Length;
		}
	}

	private void OnValidate ( ) {
		// Create arrays
		TrashControllers = GetComponentsInChildren<TrashController>( );
		trashPositionData = new Vector4[TrashCount];
		trashRadiiData = new float[TrashCount];

		// Update the trash count
		Shader.SetGlobalInt("Trash_Count", TrashCount);

		// Update the positions and radius of each trash object
		UpdateTrashPositions( );
		UpdateTrashRadii( );

		if (CallOnValidate) {
			CallOnValidate = false;
		}
	}

	private void Start ( ) {
		OnValidate( );  
	}

	private void Update ( ) {
		// As the trash objects move during the game, update their positions
		// Do not need to update the radius (for right now) because that is not changing during the game
		// Can be optimised by adding a "hasmoved" flag in the trash and only changing the position if that trash object has updated
		UpdateTrashPositions( );

		int dCount = 0;
		for (int i = 0; i < TrashCount; i++)
		{
            if (TrashControllers[i].discarded)
            {
				dCount++;
            }

			//Check that all trash is collected and recycled, and move onto the Game Over Scene
			if(dCount == TrashCount)
            {
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			}
		}
	}

	/// <summary>
	/// Update the trash positions in the grayscale shader
	/// </summary>
	public void UpdateTrashPositions () {
		for (int i = 0; i < TrashCount; i++) {
			// If the game object is not activated in the scene, do not try and update its position
			if (!TrashControllers[i].gameObject.activeSelf) {
				// TEMPORARY
				// This sets the "position" of the grayscale sphere to way below the map so it isn't visible
				// I can't figure out a better way to do this at this moment, so this works for now
				trashPositionData[i] = new Vector3(0, -1000, 0);
			} else {
				trashPositionData[i] = TrashControllers[i].transform.position;
			}
		}

		Shader.SetGlobalVectorArray("Trash_Positions", trashPositionData);
	}

	/// <summary>
	/// Update the trash radii in the grayscale shader
	/// </summary>
	public void UpdateTrashRadii ( ) {
		for (int i = 0; i < TrashCount; i++) {
			// If the game object is not activated in the scene, do not try and update its radius
			if (!TrashControllers[i].gameObject.activeSelf) {
				continue;
			}

			trashRadiiData[i] = TrashControllers[i].Radius;
		}

		Shader.SetGlobalFloatArray("Trash_Radii", trashRadiiData);
	}
}
