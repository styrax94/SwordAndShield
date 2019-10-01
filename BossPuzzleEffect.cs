using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossPuzzleEffect : MonoBehaviour {

    public Transform [] spawnPositions;
    
    public Transform finalBlast;
    public GameObject prefab;
   
    public GameObject prefabFX;
    public Transform inActivePool;
    public Transform activePool;
    public StepPattern patternEvent;

    public Transform bossT;
    void Start () {

		foreach(Transform positions in spawnPositions)
        {
            GameObject proj = (GameObject)Object.Instantiate(prefab, Vector3.zero, Quaternion.identity, inActivePool);
            GameObject particles = (GameObject)Object.Instantiate(prefabFX, Vector3.zero, Quaternion.identity, proj.transform);
            particles.GetComponent<ParticleSystem>().Stop();
            if(positions.position == finalBlast.position)
            {
                proj.GetComponent<PuzzleProjectile>().enabled = false;
                proj.GetComponent<FinalBlast>().enabled = true;

            }
            else
            {
                proj.GetComponent<PuzzleProjectile>().enabled = true;
                proj.GetComponent<FinalBlast>().enabled = false;
            }
            proj.SetActive(false);
        }
   
    }

    private void OnEnable()
    {
        patternEvent.CorrectPattern += ShootForFinalBlast;
    }
    private void OnDisable()
    {
        patternEvent.CorrectPattern -= ShootForFinalBlast;
    }
    // Update is called once per frame
    void Update () {

        /*if (Input.GetKeyDown(KeyCode.R))
        {
            ShootForFinalBlast();
        }
        */
	}

    void ShootForFinalBlast()
    {
        foreach(Transform positions in spawnPositions)
        {
            GameObject proj = inActivePool.GetChild(0).gameObject;


            proj.transform.position = positions.position;
            Vector3 direction;
            if (proj.GetComponent<FinalBlast>().enabled)
            {
                direction = bossT.position - positions.position;
                direction = direction.normalized;
                proj.GetComponent<FinalBlast>().SetDirection(direction);
                proj.GetComponentInChildren<ParticleSystem>().Play();
                proj.transform.SetParent(activePool);
                proj.SetActive(true);
                proj.GetComponent<FinalBlast>().Spawn();
               

            }
            else
            {
                direction = finalBlast.position - positions.position;
               
                direction = direction.normalized;
               
                proj.GetComponent<PuzzleProjectile>().SetDirection(direction);
                proj.GetComponentInChildren<ParticleSystem>().Play();
                proj.transform.SetParent(activePool);
                proj.SetActive(true);
                proj.GetComponent<PuzzleProjectile>().Spawn();
            }
          
           


        }





    }
}
