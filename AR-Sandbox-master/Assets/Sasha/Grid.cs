using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour {

	public Transform player;

	Node[,] grid;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public DepthMesh mesh;

    public UnityEvent onGridCreated;

	float nodeDiameter;
	int gridSizeX, gridSizeZ;
	float timeTimer = 0f;

	
	void Start(){
        nodeDiameter = nodeRadius * 2;
        gridSizeX = (int)(gridWorldSize.x / nodeDiameter);
        gridSizeZ = (int)(gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}


	void Update(){
		timeTimer += Time.deltaTime;

		if (timeTimer>0.3f){
            UpdateGrid ();
			timeTimer = 0;
		}
	}



	void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeZ];
        
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2;
        Vector3 worldPoint;
        int layer;

		for (int x = 0; x < gridSizeX; x++){
			for (int y = 0; y < gridSizeZ; y++) {
				worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius);

                layer = 1 << mesh.GetPixelLayer((int)worldPoint.x, (int)worldPoint.z);
                grid [x, y] = new Node ((Node.TerrainLayer)layer, worldPoint, x, y);
			}
		}

        if(onGridCreated == null)
            onGridCreated.Invoke();
 	}

    void UpdateGrid(){
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2;
        Vector3 worldPoint;
        int layer;

        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeZ; y++) {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius);

                layer = 1 << mesh.GetPixelLayer((int)worldPoint.x, (int)worldPoint.z);
                grid[x, y].layer = (Node.TerrainLayer)layer;
                grid[x, y].worldPosition = worldPoint;
                grid[x, y].gridX = x;
                grid[x, y].gridY = y;
            }
        }
    }

	public Node PositionTarget(Vector3 tr){
        if(grid.Length == 0)
            return null;
        
        return grid [(int)(tr.x/nodeDiameter), (int)(tr.z/nodeDiameter)];
	}


	public List<Node> GetNeighbours(Node node){
		List<Node> neighbours = new List<Node> ();

		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue;
				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeZ) {
					neighbours.Add (grid [checkX, checkY]);
				}

			}
		}
		return neighbours;
	}


    // Get biggest terrain cluster of a specific layer
    /*public IEnumerable<GridPoint> GetBiggestCluster(Node.TerrainLayer layer)
    {
        IList<GridPoint> cluster = new List<GridPoint>();
        IList<GridPoint> done = new List<GridPoint>();

        int size = 0;
        int oldSize = 0;

        bool counting = false;
        bool upper = false;
        bool previous = false;

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeZ; y++)
            {
                // Already done?
                if(done.Contains(new GridPoint(x, y)))
                    continue;

                // Perform floodfill
                FloodFill(x, y, ref cluster);
            }
        }
    }*/

    // Floodfill used for cluster algorithm
    private void FloodFill(int x, int y, Node.TerrainLayer layer, ref IList<GridPoint> cluster, ref IList<GridPoint> done)
    {
        // Already done?
        if(cluster.Contains(new GridPoint(x, y)))
            return;

        // Correct layer?
        if((layer & grid[x, y].layer) == layer)
        {
            cluster.Add(new GridPoint(x, y));
        }
    }

	public List<Node> path = new List<Node>();


	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			//Node playerNode = PositionTarget(player.transform.position);
			foreach (Node n in grid) {
                Gizmos.color = (n.layer == Node.TerrainLayer.Sand) ? Color.white : Color.red;

				if (path.Contains (n)) {
					Gizmos.color = Color.black;

				}



				//if (playerNode == n) {
				//	Gizmos.color = Color.blue;
				//}


				Gizmos.DrawCube(n.worldPosition, Vector3.one*(nodeDiameter-0.1f));
			}
		}
	}
}


// Represents a coordinate on the grid
// Can be used as indexer.
struct GridPoint : System.IEquatable<GridPoint>
{
    private readonly int x, y;

    public GridPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int X {
        get { return x; }
    }

    public int Y {
        get { return y; }
    }

    public bool Equals(GridPoint other)
    {
        return (other.x == x) && (other.y == y);
    }
}
