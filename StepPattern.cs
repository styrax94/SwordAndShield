using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class StepPattern : MonoBehaviour {

    public GameObject[] tiles;
    public Material[] materials;
    public int puzzleId;
    public float timer;
    public int tilesActivated = -1;
    public PuzzleOne firstPuzzle;
    bool puzzleSolved = false;
    public bool puzzleIsActive = false;
    MeshRenderer meshRend;
    

    public UnityAction CorrectPattern;

	void Start () {
       
        CreatePattern();
	}
	
	// Update is called once per frame
	void Update () {

       
	}

    void CreatePattern()
    {
        tilesActivated = -1;
        for (int i = 0; i < tiles.Length; i++)
        {
            GameObject tempTile = tiles[i];
            int randomInt = Random.Range(i, tiles.Length);
            tiles[i] = tiles[randomInt];
            tiles[i].GetComponent<ShieldRune>().SetId(i);
            tiles[randomInt] = tempTile;                 
        }
     
    }

    IEnumerator PlayPattern()
    {
        
        foreach (GameObject Tile in tiles)
        {
           yield return new WaitForSeconds(timer);
            meshRend = Tile.GetComponent<MeshRenderer>();
            meshRend.material = materials[1];
        }

        yield return new WaitForSeconds(timer);
        ResetMaterials();
    }

   public void ResetMaterials()
    {
        foreach (GameObject Tile in tiles)
        {
            meshRend = Tile.GetComponent<MeshRenderer>();
            meshRend.material = materials[0];
            Tile.GetComponent<ShieldRune>().UnActivate();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Sword")
        {
            StartCoroutine("PlayPattern");
            meshRend = GetComponent<MeshRenderer>();
            meshRend.material = materials[1];
            puzzleIsActive = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Sword")
        {
            if (!puzzleSolved)
            {
                ResetPuzzle();
                meshRend = GetComponent<MeshRenderer>();
                meshRend.material = materials[0];
                puzzleIsActive = false;
            }
          
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Sword")
        {
            StartCoroutine("PlayPattern");
            meshRend = GetComponent<MeshRenderer>();
            meshRend.material = materials[1];
            puzzleIsActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Sword")
        {
            if (!puzzleSolved)
            {
                ResetPuzzle();
                meshRend = GetComponent<MeshRenderer>();
                meshRend.material = materials[0];
                puzzleIsActive = false;
            }

        }
    }

    public void CheckPattern(int id)
    {
        tilesActivated++;

        if (tilesActivated != id)
        {
            ResetPuzzle();
        }   

        if(tilesActivated == tiles.Length - 1)
        {
            puzzleSolved = true;
            if(puzzleId == 4)
            {
                CorrectPattern.Invoke();
                
            }
            else if(puzzleId == 1)
            {
                firstPuzzle.DisactivateWalls();

            }
        }
    }

    private void ResetPuzzle()
    {
        StopCoroutine("PlayPattern");
        ResetMaterials();
        CreatePattern();
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material = materials[0];
        puzzleSolved = false;
        puzzleIsActive = false;
    }

    public void NextPuzzlePhase(float time)
    {
        ResetPuzzle();
        if(time != 0)
        timer = time;
    }
}
