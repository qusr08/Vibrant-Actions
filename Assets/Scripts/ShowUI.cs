using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShowUI : MonoBehaviour
{
    // public GameObject ui;
    // public GameObject[] ui = GameObject.FindGameObjectsWithTag("instruct");
    public List<GameObject> ui = new List <GameObject>();
    public bool collect; 

    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.FindGameObjectsWithTag("Instruct").ToList();

        ui[0].SetActive(true);
        for(int i = 1; i<6; i++){
            ui[i].SetActive(false);
        }
        
        StartCoroutine(Wait());
    }

    // Update is called once per frame
    void Update()
    {
        //Check for movement key pressed
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            ui[0].SetActive(false);
            ui[1].SetActive(true);
            StartCoroutine(Wait());
            
            //Mouse Movement
            ui[1].SetActive(false);
            ui[2].SetActive(true);
            StartCoroutine(Wait());

            //Trash pickup
            ui[2].SetActive(false);
            ui[3].SetActive(true);
            ui[4].SetActive(true);
            StartCoroutine(Wait());

            //Check that a piece of trash is picked up
            if(collect)
            {
                //Prompt for trash disposal
                ui[3].SetActive(false);
                ui[4].SetActive(false);
                ui[5].SetActive(true);
                StartCoroutine(Wait());

                //Cheers
                ui[5].SetActive(false);
                ui[6].SetActive(true);
                StartCoroutine(Wait());
            }

        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3.0f);
        //Destroy(ui);
        //Destroy script to prevent showing the text again
        //Destroy();
    }

}
