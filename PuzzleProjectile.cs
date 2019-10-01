using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleProjectile : MonoBehaviour
{


    public float maxDistance;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 direction;
    public float force;
    
    public Transform inActivePool;
    

    public void Spawn()
    {
        inActivePool = GameObject.Find("InactivePool").transform;
        startPosition = transform.position;
        currentPosition = startPosition;
       
        GetComponentInChildren<ParticleSystem>().Play();
       
       
        StartCoroutine("CheckTravelDistance");
      
    }
    private void Update()
    {
       
    }

    IEnumerator CheckTravelDistance()
    {
        while (Vector3.Distance(startPosition, currentPosition) < maxDistance)
        {
            yield return null;
            transform.position += direction * force * Time.deltaTime;
            currentPosition = transform.position;
        }
       
        Kill();
    }

    private void Kill()
    {
        StopCoroutine("CheckTravelDistance");
        GetComponentInChildren<ParticleSystem>().Stop();
        transform.SetParent(inActivePool);
        gameObject.SetActive(false);
        
    }

    public void SetDirection(Vector3 mDirection)
    {
        direction = mDirection;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!GetComponent<FinalBlast>().enabled)
        {
            if (other.tag == "Sword" || other.tag == "Shield") return;

            if (other.tag == "FinalBlast")
            {
                Kill();
            }
        }
           
    }
}
