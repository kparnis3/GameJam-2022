using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState
{
    Available,
    Current,
    Completed,
    ChosenNode

}

public class MazeNode : MonoBehaviour
{

    /* 
     * MazeNode is used by "MazeGenerator" to perform several tasks.
     * The first task is being called to remove walls during the DFS (RemoveWall()).
     * The second task is used for demonstration and thats to show the DFS and backtracking (SetState()).
     * The final task is setting the pickups in the scene using the x and y axis of the floor and the player z axis (SetPickup()).
     */

    [SerializeField] GameObject[] walls;
    [SerializeField] MeshRenderer floor;
    [SerializeField] GameObject floorObject;
    [SerializeField] GameObject[] Pickups;
    [SerializeField] GameObject Player;

    public void RemoveWall(int wallToRemove)
    {
        walls[wallToRemove].gameObject.SetActive(false);
    }
 
    public void SetState(NodeState state)
    {
        switch (state)
        {
            case NodeState.Available:
                floor.material.color = Color.white;
                break;
            case NodeState.Current:
                floor.material.color = Color.yellow;
                break;
            case NodeState.Completed:
                floor.material.color = Color.black;
                break;
        }
    }

    public void SetPickup(NodeState state, int position)
    {
        switch (state)
        {
            case NodeState.ChosenNode:
                //floor.material.color = Color.blue;
                Vector3 SpawnPosition = new Vector3(floorObject.transform.position.x, floorObject.transform.position.y, Player.transform.position.z);
                Instantiate(Pickups[position], SpawnPosition, Quaternion.identity);
                break;
        }
    }

    // Helper function used within "MazeGenerator" to get the TotalPickup count.
    public int getPickupLength()
    {
        return Pickups.Length;
    }

}
