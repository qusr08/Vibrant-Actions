using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShowUI : MonoBehaviour
{
    public List<GameObject> ui = new List <GameObject>();
    public bool showedInstructions0to3;
    public bool finishedShowingInstructions0to3;
    public bool collectedFirstPieceOfTrash;
    public bool goneRecycling;
    public bool finishedFirstRecyclingRun;
    //public bool 

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Wait());
        //GameObject[] instructs = GameObject.FindGameObjectsWithTag("Instruct");
        ui = GameObject.FindGameObjectsWithTag("Instruct").ToList();

        for(int i = 0; i < ui.Count; i++){
            ui[i].SetActive(false);
        }
        ui[0].SetActive(true);

        //StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
    }

    public IEnumerator ShowInstructions0to3()
    {
        ////Check for movement key pressed
        //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))

        yield return new WaitForSeconds(3.0f);

        // WASD to move --> Move mouse to look around
        ui[0].SetActive(false);
        ui[1].SetActive(true);
        //StartCoroutine(Wait());
        yield return new WaitForSeconds(3.0f);

        // Move mouse to look around --> Move close to a piece of trash
        ui[1].SetActive(false);
        ui[2].SetActive(true);
        yield return new WaitForSeconds(3.0f);
        //StartCoroutine(Wait());

        // Move close to a piece of trash
        // --> Position the reticle on the trash, then CLICK to pick it up
        ui[2].SetActive(false);
        ui[3].SetActive(true);

        finishedShowingInstructions0to3 = true;
    }

    public IEnumerator ShowInstructions4to6()
    {
        // Position the reticle on the trash, then CLICK to pick it up
        // --> You can only carry 10 pieces of trash at a time
        ui[3].SetActive(false);
        ui[4].SetActive(true);
        yield return new WaitForSeconds(3.0f);

        // You can only carry around 10 pieces of trash at a time
        // --> When your bag is full, empty it out at the bins
        ui[4].SetActive(false);
        ui[5].SetActive(true);
        yield return new WaitForSeconds(3.0f);

        // When your bag is full, empty it out at the bins
        // --> Walk up to the bins in front of the store and press ENTER
        ui[5].SetActive(false);
        ui[6].SetActive(true);
    }



    public void ShowInstructions7()
    {
        // Walk up to the bins in front of the store and press ENTER
        // --> Press A to dispose the current piece of trash in the landfill bin.
        //     Press D to throw it in the recycling bin.
        ui[6].SetActive(false);
        ui[7].SetActive(true);

    }

    public IEnumerator ShowInstructions8()
    {
        // Press A to dispose the current piece of trash in the landfill bin.
        // Press D to throw it in the recycling bin.
        // --> That's it! Happy Collecting!
        ui[7].SetActive(false);
        ui[8].SetActive(true);

        yield return new WaitForSeconds(3.0f);
        ui[8].SetActive(false);
    }

    //StartCoroutine(Wait());

    // Move closer to the trash to pick it up --> You can only carry around 10 pieces of trash at a time
    //Trash pickup

    //ui[4].SetActive(false);
    //    ui[5].SetActive(true);
    //    yield return new WaitForSeconds(3.0f);

    //StartCoroutine(Wait());

    ////Check that a piece of trash is picked up
    //if (collect)
    //{
    //    //Prompt for trash disposal
    //    ui[4].SetActive(false);
    //    ui[5].SetActive(false);
    //    ui[6].SetActive(true);
    //    StartCoroutine(Wait());

    //    //Cheers
    //    ui[6].SetActive(false);
    //    ui[7].SetActive(true);
    //    StartCoroutine(Wait());
    //}
    //}
    //}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
    }
}
