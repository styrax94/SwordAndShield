using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldRune : MonoBehaviour {

    bool activated;
    int id;
	void Start () {
        activated = false;
       
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Shield" && !activated && GetComponentInParent<StepPattern>().puzzleIsActive)
        {
            MeshRenderer meshRend = GetComponent<MeshRenderer>();
            meshRend.material = GetComponentInParent<StepPattern>().materials[1];
            GetComponentInParent<StepPattern>().CheckPattern(id);
            activated = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield" && !activated && GetComponentInParent<StepPattern>().puzzleIsActive)
        {
            MeshRenderer meshRend = GetComponent<MeshRenderer>();
            meshRend.material = GetComponentInParent<StepPattern>().materials[1];
            GetComponentInParent<StepPattern>().CheckPattern(id);
            activated = true;
        }
    }

    public void UnActivate()
    {
        activated = false;
    }
    public void SetId(int num)
    {
        id = num;
    }
}
