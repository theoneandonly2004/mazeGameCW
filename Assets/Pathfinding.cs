using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class Pathfinding : MonoBehaviour {
 
    Rigidbody rb2D;
    bool canRun = false;
    public Transform seeker;
    public Transform target;

    // string chaseObject = "RigidBodyFPSController(Clone)";
    string chaseObject = "PatrolPoint";


    [Range(0,0.2f)] public float speed = 0.1f;
               
    Grid grid;
 
    void Awake() {
       
    }

    void Start()
    {
        GameObject go = GameObject.Find("Map");
        grid = go.GetComponent<Grid>();
        rb2D = GetComponent<Rigidbody>();

        //GameObject plr = GameObject.Find(chaseObject);
        GameObject plr = GameObject.FindGameObjectsWithTag(chaseObject)[0];
        if(plr != null)
        target = plr.transform;

        if(target == null)
        {
            //plr = GameObject.FindGameObjectsWithTag("Respawn")[0];
            //target = plr.transform;
        }
        else
        {
            canRun = true;
        }

        if(target == null)
        {
            Debug.Log("player not found will check again in 1 second");
            Invoke("FinalCheck", 1.0f);
        }



    }

    void FinalCheck()
    {
       // GameObject plr = GameObject.Find(chaseObject);
        GameObject plr = GameObject.FindGameObjectsWithTag(chaseObject)[0];
        target = plr.transform;

        if (target == null)
        {
           /* plr = GameObject.FindGameObjectsWithTag("Respawn")[0];
            target = plr.transform;*/
        }
        else
        {
            canRun = true;
        }

        if (target == null)
        {
            Invoke("FinalCheck", 1.0f);
            Debug.Log("player not found will check again in 1 second");
        }
    }

    void Update() {

        if (canRun)
        {
            seeker = this.transform;
            if (grid.NodeFromWorldPoint(target.position).walkable)
            {

                FindPath(seeker.position, target.position);

                if (grid.path.Count > 0)
                    if (grid.path[0] != grid.NodeFromWorldPoint(seeker.position))
                    {
                        Vector3 flatDirection = new Vector3(grid.path[0].worldPosition.x,0, grid.path[0].worldPosition.z);
                        Vector3 moveDirection = (flatDirection - seeker.position);
                        Vector3 moveDirection2D = new Vector3(moveDirection.x, 0, moveDirection.z);
                        moveDirection2D.Normalize();
                         //seeker.position += moveDirection * speed;
                        rb2D.MovePosition(rb2D.position + moveDirection2D * speed);
                    }
            }
        }
              
    }
 
    bool FindPath(Vector3 startPos, Vector3 targetPos) {
            Node startNode = grid.NodeFromWorldPoint(startPos);
            Node targetNode = grid.NodeFromWorldPoint(targetPos);
 
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);
 
            while (openSet.Count > 0) {
                    Node currentNode = openSet[0];
                    for (int i = 1; i < openSet.Count; i ++) {
                            if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost) {
                                    currentNode = openSet[i];
                            }
                    }
 
                    openSet.Remove(currentNode);
                    closedSet.Add(currentNode);
 
                    if (currentNode == targetNode) {
                            RetracePath(startNode,targetNode);
                            return true;
                    }
 
                    foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
                            if (!neighbour.walkable || closedSet.Contains(neighbour)) {
                                    continue;
                            }
 
                            int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                            if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
                                    neighbour.gCost = newMovementCostToNeighbour;
                                    neighbour.hCost = GetDistance(neighbour, targetNode);
                                    neighbour.parent = currentNode;
 
                                    if (!openSet.Contains(neighbour))
                                            openSet.Add(neighbour);
                            }
                    }
            }

            return false;
    }
 
    void RetracePath(Node startNode, Node endNode) {
            List<Node> path = new List<Node>();
            Node currentNode = endNode;
 
            while (currentNode != startNode) {
                    path.Add(currentNode);
                    currentNode = currentNode.parent;
            }
                
            // List<Node> waypoints = SimplifyPath(path);
            // path = waypoints;

            path.Reverse();
 
            grid.path = path;
 
    }
 
    List<Node> SimplifyPath(List<Node> path) {
        List<Node> waypoints = new List<Node>();
        Vector3 directionOld = Vector3.zero;

        for (int i = 1; i < path.Count; i ++) {
            Vector3 directionNew = new Vector3(path[i-1].gridX - path[i].gridX,0, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld) {
                waypoints.Add(path[i]);
            }
            directionOld = directionNew;
        }
        return waypoints;
    }

    int GetDistance(Node nodeA, Node nodeB) {
            int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
 
            if (dstX > dstY)
                    return 14*dstY + 10* (dstX-dstY);
            return 14*dstX + 10 * (dstY-dstX);
    }

    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("i the enemy have collided *eats an apple to look like more of an asshole*" + collision.name);
    }
}
 