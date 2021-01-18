using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaypointInitializer : MonoBehaviour
{
    public Tilemap floor;
    public GameObject wayPoint;
    public GameObject egg;
    public int scanStartX = -250, scanStartY = -250;
    public int scanFinishX = 250, scanFinishY = 250;

    public List<GameObject> waypointsList = new List<GameObject>();

    private int waypointIndex = 0;
    public List<Transform> wayPoints = new List<Transform>();

    [ContextMenu("Initialize Waypoints")]
    private void InitializeWaypoints() {
        waypointIndex = 0;

        if ( wayPoints.Count > 0 )
            wayPoints.Clear();

        for ( int x = scanStartX; x < scanFinishX; x++ ) {
            for ( int z = scanStartY; z < scanFinishY; z++ ) {
                GameObject go = floor.GetInstantiatedObject(new Vector3Int(x, z, 0));

                if ( go != null ) {
                    print("GO Name: " + go.name);
                    Vector3 vec = new Vector3(Mathf.CeilToInt(go.transform.position.x), Mathf.CeilToInt(go.transform.position.y), Mathf.CeilToInt(go.transform.position.z));
                    GameObject wp = Instantiate(wayPoint, vec, Quaternion.identity);
                    wp.name = string.Format("Waypoint ({0})", waypointIndex);
                    wp.tag = "Waypoint";
                    waypointIndex++;
                    wayPoints.Add(wp.transform);
                }

                //TileBase tb = floor.GetTile(new Vector3Int(x, z, 0));
                //if ( tb != null ) {

                //}
            }
        }

        // Check distance between all points starting from the start to the end point
        GameObject eggPoint = Instantiate(wayPoint, egg.transform.position, Quaternion.identity);
        waypointsList.Add(eggPoint);

        for ( int i = 0; i < waypointsList.Count; i++ ) {
            GameObject closestGO = GetClosestPosition(waypointsList[i].transform.position, wayPoints);
            //closestGO.SetActive(false);
            waypointsList.Add(closestGO);
            print("a");
        }
    }

    //[ContextMenu("Clear")]
    //private void ClearWaypoints() {
    //    if (wayPoints.Count > 0)
    //        for ( int i = 0; i < wayPoints.Count; i++ ) {
    //            Destroy(wayPoints[i].gameObject);
    //        }
    //    if (waypointsList.Count > 0)
    //        for ( int i = 0; i < waypointsList.Count; i++ ) {
    //            Destroy(waypointsList[i].gameObject);
    //        }
    //}

    private GameObject GetClosestPosition( Vector3 currentPos, List<Transform> waypoints ) {
        GameObject GOToReturn = null;
        float minDist = Mathf.Infinity;
        Transform ttwo = null;

        foreach ( Transform t in waypoints ) {
            float dist = Vector3.Distance(t.position, currentPos);

            if ( dist < minDist ) {
                GOToReturn = t.gameObject;
                minDist = dist;

                ttwo = t;
                Debug.DrawLine(currentPos, t.position, Color.blue);
            }
        }

        wayPoints.Remove(ttwo);

        return GOToReturn;
    }

    #region a* star (this is old)
    //public Transform seeker, target;
    //Grid grid;

    //void FindPath( Vector3 startPos, Vector3 targetPos ) {
    //    Node startNode = grid.NodeFromWorldPoint(startPos);
    //    Node targetNode = grid.NodeFromWorldPoint(targetPos);

    //    List<Node> openSet = new List<Node>();
    //    HashSet<Node> closedSet = new HashSet<Node>();
    //    openSet.Add(startNode);

    //    while ( openSet.Count > 0 ) {
    //        Node node = openSet[0];
    //        for ( int i = 1; i < openSet.Count; i++ ) {
    //            if ( openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost ) {
    //                if ( openSet[i].hCost < node.hCost )
    //                    node = openSet[i];
    //            }
    //        }

    //        openSet.Remove(node);
    //        closedSet.Add(node);

    //        if ( node == targetNode ) {
    //            RetracePath(startNode, targetNode);
    //            return;
    //        }

    //        foreach ( Node neighbour in grid.GetNeighbours(node) ) {
    //            if ( !neighbour.walkable || closedSet.Contains(neighbour) ) {
    //                continue;
    //            }

    //            int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
    //            if ( newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour) ) {
    //                neighbour.gCost = newCostToNeighbour;
    //                neighbour.hCost = GetDistance(neighbour, targetNode);
    //                neighbour.parent = node;

    //                if ( !openSet.Contains(neighbour) )
    //                    openSet.Add(neighbour);
    //            }
    //        }
    //    }
    //}

    //void RetracePath( Node startNode, Node endNode ) {
    //    List<Node> path = new List<Node>();
    //    Node currentNode = endNode;

    //    while ( currentNode != startNode ) {
    //        path.Add(currentNode);
    //        currentNode = currentNode.parent;
    //    }
    //    path.Reverse();

    //    grid.path = path;

    //}

    //int GetDistance( Node nodeA, Node nodeB ) {
    //    int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    //    int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

    //    if ( dstX > dstY )
    //        return 14 * dstY + 10 * ( dstX - dstY );
    //    return 14 * dstX + 10 * ( dstY - dstX );
    //}
    #endregion
}
