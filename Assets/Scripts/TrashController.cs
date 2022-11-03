using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashController : MonoBehaviour {
	[Tooltip("The trash area object.")]
	[SerializeField] private GameObject trashArea;
	[Space]
	[Tooltip("The radius of influence of the trash. Size of the grayscale around the trash.")]
	[SerializeField] [Min(0)] private float trashAreaRadius;

	/// <summary>
	/// Called each time Unity editor is updated.
	/// </summary>
	private void OnValidate ( ) {
		// Update the trash area object based on values set for the trash
		if (trashArea == null) {
			Debug.LogWarning("Trash Area [Transform] is not set!");
		} else {
			// Set the shader radius
			Shader.SetGlobalFloat("SphericalMask_Radius", trashAreaRadius);

			// Set the size of the trash area
			trashArea.transform.localScale = trashAreaRadius * 2 * Vector3.one;
		}

		Shader.SetGlobalVector("SphericalMask_Position", transform.position);
	}

	/// <summary>
	/// Calls as fast as possible while game is running.
	/// </summary>
	private void Update ( ) {
		// As the game runs, update the position of the grayscale shader to the position of this trash
		Shader.SetGlobalVector("SphericalMask_Position", transform.position);
	}
}
