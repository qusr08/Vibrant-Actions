using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour {
	[Tooltip("Checking this box will force an update of the OnValidate method.")]
	[SerializeField] private bool CallOnValidate;

	private Vector4[ ] trashPositionData;
	private float[ ] trashRadiiData;
	private TrashController[ ] trashControllers;

	/// <summary>
	/// The number of trash objects in the scene. All Trash objects should be a child to this trash manager object.
	/// </summary>
	public int TrashCount {
		get {
			return trashControllers.Length;
		}
	}

	private void OnValidate ( ) {
		// Create arrays
		trashControllers = GetComponentsInChildren<TrashController>( );
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
	}

	/// <summary>
	/// Update the trash positions in the grayscale shader
	/// </summary>
	private void UpdateTrashPositions () {
		for (int i = 0; i < TrashCount; i++) {
			// If the game object is not activated in the scene, do not try and update its position
			if (trashControllers[i] == null) {
				continue;
			}

			trashPositionData[i] = trashControllers[i].transform.position;
		}

		Shader.SetGlobalVectorArray("Trash_Positions", trashPositionData);
	}

	/// <summary>
	/// Update the trash radii in the grayscale shader
	/// </summary>
	private void UpdateTrashRadii ( ) {
		for (int i = 0; i < TrashCount; i++) {
			// If the game object is not activated in the scene, do not try and update its radius
			if (trashControllers[i] == null) {
				continue;
			}

			trashRadiiData[i] = trashControllers[i].Radius;
		}

		Shader.SetGlobalFloatArray("Trash_Radii", trashRadiiData);
	}
}
