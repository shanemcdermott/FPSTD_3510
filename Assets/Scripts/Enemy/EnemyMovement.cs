using UnityEngine;
using System.Collections;
using System;

public class EnemyMovement : MonoBehaviour, IRespondsToDeath
{
    public GameObject target;
    public float speed = 4f;

	private Vector3[] pathToTarget;

    void Awake ()
    {
        GetComponent<HealthComponent>().RegisterDeathResponder(this);
    }

    public void AssignTarget(GameObject targetObject)
    {
        target = targetObject;
    }


    //Move Towards Target
    void Update ()
    {
        if (target != null)
        {
            GetComponent<Animator>().SetBool("isWalking", true);
            float zDiff = target.transform.position.z - this.transform.position.z;
            float xDiff = target.transform.position.x - this.transform.position.x;
            this.transform.localEulerAngles = new Vector3(0, (Mathf.Atan2(xDiff, zDiff) / Mathf.PI * 180), 0);
            this.transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));

        }
    }

    public void OnDeath(DamageContext context)
    {
        enabled = false;
    }
}
