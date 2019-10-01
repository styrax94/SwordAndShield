using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour {
    public static BossHealth instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    public float maxHealth;
    float currentHealth;

    bool testingDamage;
	void Start () {
        currentHealth = maxHealth;
        testingDamage = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F) && testingDamage)
        {
            GetDamaged(16);
            testingDamage = false;
            StartCoroutine("test");
            GetComponent<BossController>().CheckBoss();
        }
	}

    public void GetDamaged(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);
    }
    
    public float GetHealthPercentage()
    {
        return (currentHealth / maxHealth) * 100;
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(1);
        testingDamage = true;
        
    }
}
