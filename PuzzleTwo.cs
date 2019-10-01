using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleTwo : MonoBehaviour {

    public Material[] materials;
    MeshRenderer meshRend;
    public GameObject[] walls;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.material = materials[1];
            
            foreach(GameObject t in walls)
            {
                t.SetActive(false);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sword")
        {
            meshRend = GetComponent<MeshRenderer>();
            meshRend.material = materials[0];

            foreach (GameObject t in walls)
            {
                t.SetActive(true);

            }
        }
    }
}
