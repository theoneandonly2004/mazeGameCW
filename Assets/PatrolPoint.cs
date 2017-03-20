using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol
{
    Transform thisPosition;
    Transform linkLocation;
    GameObject linkObject , thisObject;
    Patrol linkedPoint;
    bool isActive = false;

    public Patrol(GameObject linkedObject , GameObject thisObject)
    {
        this.thisObject = thisObject;
        thisPosition = thisObject.transform;
        linkedPoint = linkedObject.GetComponent<PatrolPoint>().point;
        linkLocation = linkedObject.transform;
        linkObject = linkedObject;
    }

    public Patrol()
    {

    }

    public bool getIsActive()
    {
        return isActive;
    }

    public Transform getLinkedLocation()
    {
        return linkLocation;
    }

    public Patrol getLinkedPoint()
    {
        if(linkedPoint != null)
        {
            return linkedPoint;
        }

        return null;
    }

    public Transform getLocation()
    {
        return thisPosition;
    }



}

public class PatrolPoint : MonoBehaviour {
    [HideInInspector]
    public Patrol point;

	// Use this for initialization
	void Start () {
        //point = new Patrol();	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider collision)
    {
        Patrol nextPoint = point.getLinkedPoint();
        Debug.Log("hi" + point.getLinkedLocation());
        
            if (collision.tag == "Enemy")
            {
                Debug.Log("now moving to object at position " + point.getLinkedLocation());
                collision.GetComponent<Pathfinding>().target = point.getLinkedLocation();
            }
        
    }
}
