using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door
{
    bool isOpen;
    GameObject doorObject;

    public Door(bool isOpen , GameObject doorObject)
    {
        this.isOpen = isOpen;
        this.doorObject = doorObject; 
    }

    public bool getIsOpen()
    {
        return isOpen;
    }

    public GameObject getDoorObject()
    {
        return doorObject;
    }

    public void reverseOpenNess()
    {
        isOpen = !isOpen;
        ManageDoorState(isOpen);
    }

    void ManageDoorState(bool isOpen)
    {
        if (isOpen)
        {
            doorObject.SetActive(false);
        }
        else
        {
            doorObject.SetActive(true);
        }
    }

    public void ManageDoorState() //just uses the default
    {
        if (isOpen)
        {
            doorObject.SetActive(false);
        }
        else
        {
            doorObject.SetActive(true);
        }
    }




}

public class DoorwayControl : MonoBehaviour {

    [HideInInspector]
    public Door currentDoor;

    void Awake()
    {
        currentDoor = new Door(this.name == "Doorway(Clone)", this.gameObject);

        Debug.Log("this door is open:" + currentDoor.getIsOpen());
    }

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
