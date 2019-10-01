﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public GameObject fireEffect;
    public float maxDistance;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 direction;
    public float force;
    GameObject particles;
    public Transform inActivePool;
    

    public void Spawn()
    {
        inActivePool = GameObject.Find("InActivePool").transform;
        startPosition = transform.position;
        currentPosition = startPosition;
        //transform.GetChild(0).SetParent(transform);
        GetComponentInChildren<ParticleSystem>().Play();
       
        
        StartCoroutine("CheckTravelDistance");
        GetComponent<AudioSource>().Play();
    }

    IEnumerator CheckTravelDistance()
    {
        while (Vector3.Distance(startPosition, currentPosition) < maxDistance)
        {
            yield return null;
            transform.position += direction * force * Time.deltaTime;
            float y= Mathf.Clamp(transform.position.y, startPosition.y, -9f);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
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
        GetComponent<AudioSource>().Stop();
    }

    public void SetDirection(Vector3 mDirection)
    {
        direction = mDirection;
    }


    private void OnTriggerEnter(Collider other)
    {
       
        if (other.tag == "Sword")
        {
            GameUIManager.instance.Damaged();
        }
      if(other.tag != "Room3")
        Kill();

    }
}
