using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ColorToPrefab {
	public Color32 color;
	public GameObject prefab;
}

public class LevelLoader : MonoBehaviour {

    int patrolNumber = 0;
    GameObject player;
    List<System.IO.FileInfo> fileInfo;
    public List<string> levelNames;
    Vector2 WorldSize;
    List<GameObject> enemies;
    List<GameObject> patrolPoints;

    public ColorToPrefab[] colorToPrefab;


	// Use this for initialization
	void Start () {
        enemies = new List<GameObject>();
        patrolPoints = new List<GameObject>();
        LoadAllLevelNames();
        if (levelNames.Count > 0) LoadMap(levelNames[6]);
    }

	void EmptyMap() {
		// Find all of our children and...eliminate them.

		while(transform.childCount > 0) {
			Transform c = transform.GetChild(0);
			c.SetParent(null); // become Batman
			Destroy(c.gameObject); // become The Joker
		}
	}

	void LoadAllLevelNames() {
        // Read the list of files from StreamingAssets/Levels/*.png
        // The player will progess through the levels alphabetically

        var info = new System.IO.DirectoryInfo(Application.streamingAssetsPath);

        fileInfo = info.GetFiles().ToList<System.IO.FileInfo>();

        fileInfo.RemoveAll(u => u.Name.EndsWith("meta"));
        
        foreach (System.IO.FileInfo fno in fileInfo)
        {
            levelNames.Add(fno.Name);
        }

    }

	void LoadMap(string levelName) {
		EmptyMap();

        WorldSize = GetComponent<Grid>().gridWorldSize;

        // Read the image data from the file in StreamingAssets
        string filePath = Application.dataPath + "/StreamingAssets/" + levelName;
		byte[] bytes = System.IO.File.ReadAllBytes(filePath);
		Texture2D levelMap = new Texture2D(2, 2);
		levelMap.LoadImage(bytes);


		// Get the raw pixels from the level imagemap
		Color32[] allPixels = levelMap.GetPixels32();
		int width = levelMap.width;
		int height = levelMap.height;

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				SpawnTileAt( allPixels[(y * width) + x], x, y );

			}
		}
        managePatrolPoints();
        GetComponent<Grid>().CreateGrid();
	}

    void managePatrolPoints()
    {
        //decides which patrol points link to eachother 
        patrolPoints[0].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[1], patrolPoints[0]);
        patrolPoints[1].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[7], patrolPoints[1]);
        patrolPoints[2].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[3], patrolPoints[2]);
        patrolPoints[3].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[4], patrolPoints[3]);
        patrolPoints[4].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[2], patrolPoints[4]);
        patrolPoints[5].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[6], patrolPoints[5]);
        patrolPoints[6].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[0], patrolPoints[6]);
        patrolPoints[7].GetComponent<PatrolPoint>().point = new Patrol(patrolPoints[5], patrolPoints[7]);

        //need to turn off all other patrol points except for the currently active one

        
    }

    public List<GameObject> getEnemiesList()
    {
        return enemies;
    }

    public GameObject getPlayerObject()
    {
        return player;
    }

	void SpawnTileAt( Color32 c, int x, int y ) {
        
		// If this is a transparent pixel, then it's meant to just be empty.
		if(c.a <= 0) {
			return;
		}




        // Find the right color in our map

        // NOTE: This isn't optimized. You should have a dictionary lookup for max speed

        Color32 playerColor = new Color32(194, 145, 195, 255);
        Color32 enemyColor = new Color32(255, 0, 0, 255);
        Color32 patrolPointColor = new Color32(33, 227, 0, 255);

		foreach(ColorToPrefab ctp in colorToPrefab) {

           


            if ( c.Equals(ctp.color) ) {


                // Spawn the prefab at the right location

                GameObject go;

                if (c.Equals(playerColor))
                {
                    Debug.Log("i found the player and spawned");
                     go = (GameObject)Instantiate(ctp.prefab, new Vector3(-WorldSize.x / 2 + x, 0, -WorldSize.y / 2 + y), Quaternion.identity);
                    player = go;
                   

                    //player.SetActive(false);
                    return;
                }
                else if (c.Equals(enemyColor))
                {
                    
                    //enemies.Add(go);
                    return;
                }
                else if (c.Equals(patrolPointColor))
                {
                   
                    y += 1;
                    go = (GameObject)Instantiate(ctp.prefab, new Vector3(-WorldSize.x / 2 + x, 0, -WorldSize.y / 2 + y), Quaternion.identity);
                   
                    go.name = "number:" + patrolNumber;
                    patrolNumber++;
                    patrolPoints.Add(go);
                }
                else
                {
                    
                    y = y+ 1;
                     go = (GameObject)Instantiate(ctp.prefab, new Vector3(-WorldSize.x / 2 + x, 0, -WorldSize.y / 2 + y), Quaternion.identity);
                }

                /*if(go.GetComponent<BoxCollider>() != null)
                {
                    float currentX = go.transform.position.x;
                    float currentY = go.transform.position.y;

                    go.transform.position = new Vector3(currentX + .5f, currentY + .5f, 0);

                   
                   
                }*/
                go.transform.SetParent(this.transform);

                if(go.GetComponent<Renderer>() != null)
                go.GetComponent<Renderer>().material.color = c;
              
				// maybe do more stuff to the gameobject here?
				return;
			}
		}

		// If we got to this point, it means we did not find a matching color in our array.

		Debug.LogError("No color to prefab found for: " + c.ToString() );

	}
	
}
