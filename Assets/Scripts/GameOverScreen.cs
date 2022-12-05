using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour {
	public void Quit ( ) {
		Application.Quit( );
	}

	public void Restart () {
		SceneManager.LoadScene(SceneManager.GetActiveScene( ).buildIndex - 1);
	}
}
