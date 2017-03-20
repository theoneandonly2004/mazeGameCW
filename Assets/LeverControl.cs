using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverControl : MonoBehaviour {

    bool isActive = false;
    List<GameObject> doors;
	// Use this for initialization
	void Start () {
        GameObject [] lockedDoors = GameObject.FindGameObjectsWithTag("UnlockableDoor");
        doors = new List<GameObject>();

        for (int count = 0; count < lockedDoors.Length; count++)
        {
            doors.Add(lockedDoors[count]);
            doors[count].GetComponent<DoorwayControl>().currentDoor.ManageDoorState();
        }

        //InvokeRepeating("doDoorStuff", 2.0f, 2.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            doDoorStuff();
        }
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            doDoorStuff();
        }
        else if (collider.tag == "Enemy")
        {
            doDoorStuff();
        }
    }

    void doDoorStuff()
    {
        for (int count = 0; count < doors.Count; count++)
        {

            Door currentDoor = doors[count].GetComponent<DoorwayControl>().currentDoor;
            currentDoor.reverseOpenNess();
            Node pointNode = GameObject.Find("Map").GetComponent<Grid>().NodeFromWorldPoint(currentDoor.getDoorObject().transform.position);
            pointNode.walkable = currentDoor.getIsOpen();
        }

        GameObject.Find("Map").GetComponent<Grid>().CreateGrid();
    }
}
