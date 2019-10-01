using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossController : MonoBehaviour {

    public Animator bossAnimator;
    public Transform fireBallSpawnPos;
    public GameObject fireBall;
    public GameObject fireEffect;

    public Transform player;
    public Transform inActivePool;
    public Transform activePool;

    public UnityAction IncreaseDifficulty;

    public bool canGetDamaged;
    public BossHealth bossCurrentHealth;
    public StepPattern bossPuzzle;

    bool phaseOne;
    bool phaseTwo;
    bool phaseThree;
    bool gettingUp;
    bool bossFightStarted;

    //shootCD
    public float fireCooldown;
    bool fired = false;
    float phaseTwoCounter;
    float phaseThreeCounter;
    private void OnEnable()
    {
      //  IncreaseDifficulty += 
    }

    void Start () {

        for(int i = 0; i < 5; i++)
        {
           GameObject proj = (GameObject)Object.Instantiate(fireBall, Vector3.zero, Quaternion.identity, inActivePool);
            GameObject particles = (GameObject)Object.Instantiate(fireEffect, Vector3.zero, Quaternion.identity, proj.transform);
            particles.GetComponent<ParticleSystem>().Stop();
            proj.SetActive(false);
        }
        canGetDamaged = false;
        phaseOne = true;
        phaseTwo = false;
        phaseThree = false;
        phaseTwoCounter = 0;
        phaseThreeCounter = 0;
        gettingUp = false;
        bossFightStarted = false;
    }
	
	// Update is called once per frame
	void Update () {


        if (!canGetDamaged && Vector3.Distance(player.position,transform.position)< 40 && !gettingUp)
        {
            if (!bossFightStarted)
            {
                if (!Camera.main.GetComponent<AudioSource>().isPlaying)
                    Camera.main.GetComponent<AudioSource>().Play();

                GameUIManager.instance.ActivateBossHealth();
                bossFightStarted = true;


            }

            if (phaseOne)
            {
                if (!fired)
                {
                    bossAnimator.Play("FireBallAttack");
                    fired = true;
                }
            }
            else if (phaseTwo)
            {
                if (!fired)
                {
                    bossAnimator.Play("FireBallAttack");
                    //phaseTwoCounter++;

                   
                    fired = true;
                }
                

            }
            else if (phaseThree)
            {
                if (!fired)
                {
                    bossAnimator.Play("FireBallAttack");
                    fired = true;
                }
            }
        }
        else
        {
            if (!bossFightStarted)
            {
                if (Camera.main.GetComponent<AudioSource>().isPlaying)
                    Camera.main.GetComponent<AudioSource>().Pause();

                GameUIManager.instance.DeactivateBossHealth();
            }       
        }
    }

    void Fire()
    {
        if (inActivePool.childCount > 0)
        {
          
            GameObject proj = inActivePool.GetChild(0).gameObject;
           
           
            proj.transform.position = fireBallSpawnPos.position;
            Vector3 direction = player.position - fireBallSpawnPos.position;
           
           
            direction = direction.normalized;
            proj.GetComponent<Projectile>().SetDirection(direction);
            proj.GetComponentInChildren<ParticleSystem>().Play();
            proj.transform.SetParent(activePool);
            proj.SetActive(true);
            proj.GetComponent<Projectile>().Spawn();

        }
        /*
        if (phaseTwoCounter > 1 && phaseTwoCounter <3)
        {
            fireCooldown = 1f;
            
        }
        if (phaseTwoCounter > 3 && phaseTwoCounter < 5)
        {
            fireCooldown = 5f;
            phaseTwoCounter = 0;
        }  
        */
        StartCoroutine("WaitForCoolDown");
       // fired = true;
    }

    public void GetHit()
    {
        fired = false;
        bossAnimator.Play("Fall");
        canGetDamaged = true;
        StartCoroutine("CheckIfPlayerHits");
    }

    IEnumerator CheckIfPlayerHits()
    {
        float cHealth = bossCurrentHealth.GetHealthPercentage();
        yield return new WaitForSeconds(10);

        if(cHealth == bossCurrentHealth.GetHealthPercentage())
        {
            canGetDamaged = false;
            bossAnimator.Play("GetUp");
            bossPuzzle.NextPuzzlePhase(0);
            gettingUp = true;
        }

    }


    public void CheckBoss()
    {
        if (phaseOne)
        {
            if (bossCurrentHealth.GetHealthPercentage() < 70)
            {
                Debug.Log("PhaseOne");
                canGetDamaged = false;
                bossAnimator.Play("GetUp");
                bossPuzzle.NextPuzzlePhase(0.8f);
                phaseOne = false;
                phaseTwo = true;
                gettingUp = true;
            }
        }
        else if (phaseTwo)
        {
            if (bossCurrentHealth.GetHealthPercentage() < 35)
            {

                Debug.Log("PhaseTwo");
                bossAnimator.Play("GetUp");
                bossPuzzle.NextPuzzlePhase(0.6f);
                phaseTwo = false;
                phaseThree = true;
                canGetDamaged = false;
                gettingUp = true;
            }

        }
        else if (phaseThree)
        {
            if (bossCurrentHealth.GetHealthPercentage() <= 0)
            {
                GameUIManager.instance.GameWon();
            }
        }

    }

    IEnumerator WaitForCoolDown()
    {
        
        yield return new WaitForSeconds(fireCooldown);
        fired = false;
        
    }

    public void SetFireCoolDown(int cd)
    {
        fireCooldown = cd;
    }

    public void SetCanGetDamaged()
    {
        gettingUp = false;
        Debug.Log("Got Up");
    }
}
