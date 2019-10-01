using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBlast : MonoBehaviour {

    public float maxDistance;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 direction;
    public float force;
    public Transform inActivePool;
    public int countHit;


   public void Spawn () {
        inActivePool = GameObject.Find("InactivePool").transform;
        startPosition = transform.position;
        currentPosition = startPosition;
        GetComponentInChildren<ParticleSystem>().Play();
        countHit = 0;
       
    }

    private void Update()
    {
       if(countHit == 4)
        {
            EnableTravel();
            countHit++;
        }
    }


    private void OnTriggerEnter(Collider other)
    {

     if(other.tag == "FinalBlast")
        {
            if (!GetComponent<PuzzleProjectile>().enabled)
            {
                transform.localScale += new Vector3(1f, 1f, 1f);
                countHit++;
            }
            
           
       }
    if(other.tag == "Enemy")
        {
            Kill();
           
            other.GetComponent<BossController>().GetHit();

        }
          
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
        transform.localScale = new Vector3(2f, 2f, 2f);
        transform.SetParent(inActivePool);
        gameObject.SetActive(false);

    }

    public void SetDirection(Vector3 mDirection)
    {
        direction = mDirection;
    }

    public void EnableTravel()
    {
        StartCoroutine("CheckTravelDistance");
    }

}
