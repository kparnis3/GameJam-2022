using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MazeGenerator : MonoBehaviour
{

    /*
     *  MazeGenerator is the core script thats used to generate the Maze using DFS and Backtracking, 
     *  compute the A* obstacle scan and generate the designated pickups.
     * 
     */

    [SerializeField] MazeNode[] nodePrefabs;
    [SerializeField] MazeNode nodePrefab;
    [SerializeField] MazeNode startPrefab;
    [SerializeField] Vector2Int mazeSize;
    [SerializeField] float nodeSize;

   // public GameObject myGameManager;
    public Score other;

    public AIPath aiPath;
    int index;

    public int TotalPickups;
    List<int> list = new List<int>();

    private void Start()
    {

        GameObject gd = GameObject.FindGameObjectWithTag("Data");
        other = gd.GetComponent<Score>();

        
        TotalPickups = nodePrefab.getPickupLength(); //Fetch the pickup length from "MazeNode" script
        other.setScorePlayer(); //Set the Initial Score.

        /*
         * IMPORTANT: either StartCoroutine() or GenerateMazeInstant() are to be commented, not both set as active.
         * 
         */

          //Coroutine used to slowly generate the map and highlight DFS and backtrack.
        //StartCoroutine(GenerateMaze(mazeSize)); 

          //Genrate the maze how its intended to be played.
        GenerateMazeInstant(mazeSize);
    
    }
    void GenerateMazeInstant(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();
        MazeNode newNode;
        bool Start = true;
        
        // Create the initial grid depending on the maze's specified gridsize.
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (size.x / 2f), y - (size.y / 2f), 0) * nodeSize;
                if(Start == true) //Make sure the first node always spawns the player.
                {
                    newNode = Instantiate(startPrefab, nodePos, Quaternion.identity, transform);
                    Start = false;
                }
                else //Randomly spawn a node from specified node prefabs.
                {
                    index = Random.Range(0, nodePrefabs.Length);
                    newNode = Instantiate(nodePrefabs[index], nodePos, Quaternion.identity, transform);
                }           
                
                nodes.Add(newNode);
            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();

        //Choose starting Node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);

        // The Depth first serach algorithm with backtracking.
        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];

                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                }
                currentPath.Add(chosenNode);
               // chosenNode.SetState(NodeState.Current);
            }

            else //BackTrack section of the DFS.
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            }

        }

        /* 
         * Once the Maze is generated, astar scans the maze for obstacles using the "Obstacle" tag.
         * After the pickups are generated randomly using the GeneratePickup() function.
         */

        // Scan here
        AstarPath.active.Scan();

        // Generate Pickups 
        //GeneratePickup(completedNodes);
        GeneratePickup(nodes);

        SetInvisible();

    }

    void SetInvisible()
    {
        GameObject[] gd = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int x = 0; x < gd.Length; x++)
        {
            gd[x].GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        } 
        
    }

    //Used to generate a specified pickup amount at random nodes (excluding staring node) 
    void GeneratePickup(List<MazeNode> AllNodes)
    {
        list = new List<int>(new int[TotalPickups]);

        for (int x = 0; x <= TotalPickups-1; x++)
        {
            int chosenNode = Random.Range(1, AllNodes.Count); //Take not including starting

            while (list.Contains(chosenNode)) //If the pickup is already instantiated, choose a new one.
            {
                chosenNode = Random.Range(1, AllNodes.Count);
                
            }

            AllNodes[chosenNode].SetPickup(NodeState.ChosenNode, x); //Instatiate at the node position with the chosen pickup.
            list.Add(chosenNode);
        }
            
    }

    //Similar to GenerateMazeInstant but slowed down to show how the DPS works.
IEnumerator GenerateMaze(Vector2Int size)
    {
        List<MazeNode> nodes = new List<MazeNode>();

        // Create nodes
        for (int x=0; x<size.x; x++)
        {
            for (int y=0; y<size.y; y++)
            {
                Vector3 nodePos = new Vector3(x - (size.x / 2f), y - (size.y / 2f), 0) * nodeSize;
                MazeNode newNode = Instantiate(nodePrefab, nodePos, Quaternion.identity, transform);
                nodes.Add(newNode);

                yield return null;

            }
        }

        List<MazeNode> currentPath = new List<MazeNode>();
        List<MazeNode> completedNodes = new List<MazeNode>();


        //Choose starting Node
        currentPath.Add(nodes[Random.Range(0, nodes.Count)]);
        currentPath[0].SetState(NodeState.Current);

        while (completedNodes.Count < nodes.Count)
        {
            // Check nodes next to current node
            List<int> possibleNextNodes = new List<int>();
            List<int> possibleDirections = new List<int>();

            int currentNodeIndex = nodes.IndexOf(currentPath[currentPath.Count - 1]);
            int currentNodeX = currentNodeIndex / size.y;
            int currentNodeY = currentNodeIndex % size.y;

            if (currentNodeX < size.x - 1)
            {
                // Check node to the right of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + size.y]))
                {
                    possibleDirections.Add(1);
                    possibleNextNodes.Add(currentNodeIndex + size.y);
                }
            }
            if (currentNodeX > 0)
            {
                // Check node to the left of the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - size.y]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - size.y]))
                {
                    possibleDirections.Add(2);
                    possibleNextNodes.Add(currentNodeIndex - size.y);
                }
            }
            if (currentNodeY < size.y - 1)
            {
                // Check node above the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex + 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex + 1]))
                {
                    possibleDirections.Add(3);
                    possibleNextNodes.Add(currentNodeIndex + 1);
                }
            }
            if (currentNodeY > 0)
            {
                // Check node below the current node
                if (!completedNodes.Contains(nodes[currentNodeIndex - 1]) &&
                    !currentPath.Contains(nodes[currentNodeIndex - 1]))
                {
                    possibleDirections.Add(4);
                    possibleNextNodes.Add(currentNodeIndex - 1);
                }
            }

            // Choose next node
            if (possibleDirections.Count > 0)
            {
                int chosenDirection = Random.Range(0, possibleDirections.Count);
                MazeNode chosenNode = nodes[possibleNextNodes[chosenDirection]];
                
                switch (possibleDirections[chosenDirection])
                {
                    case 1:
                        chosenNode.RemoveWall(1);
                        currentPath[currentPath.Count - 1].RemoveWall(0);
                        break;
                    case 2:
                        chosenNode.RemoveWall(0);
                        currentPath[currentPath.Count - 1].RemoveWall(1);
                        break;
                    case 3:
                        chosenNode.RemoveWall(3);
                        currentPath[currentPath.Count - 1].RemoveWall(2);
                        break;
                    case 4:
                        chosenNode.RemoveWall(2);
                        currentPath[currentPath.Count - 1].RemoveWall(3);
                        break;
                } 
                currentPath.Add(chosenNode);
                chosenNode.SetState(NodeState.Current);
            }
            
            else //BackTrack
            {
                completedNodes.Add(currentPath[currentPath.Count - 1]);

                currentPath[currentPath.Count - 1].SetState(NodeState.Completed);
                currentPath.RemoveAt(currentPath.Count - 1);
            } 
            
            yield return new WaitForSeconds(0.05f);
        }
        
    }
    
}
