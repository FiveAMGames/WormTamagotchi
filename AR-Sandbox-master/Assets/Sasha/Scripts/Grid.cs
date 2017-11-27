using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Grid : MonoBehaviour {

    public bool updateGrid = true;

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

        if (timeTimer>0.3f && updateGrid){
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

				layer = 1 << (mesh.GetPixelLayer((int)worldPoint.x, (int)worldPoint.z));
                grid [x, y] = new Node ((Node.TerrainLayer)layer, worldPoint, x, y);
			}
		}

        if(onGridCreated != null)
            onGridCreated.Invoke();
 	}


    public void SetNode(int _x, int _y, Node.TerrainLayer _layer){
        grid [_x, _y].layer = _layer;
    }

    public Node GetNode(int _x, int _y){
        return grid [_x, _y];
    }

    void UpdateGrid(){
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2;
        Vector3 worldPoint;
        int layer;

        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeZ; y++) {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius);

				layer = 1 << (mesh.GetPixelLayer((int)worldPoint.x, (int)worldPoint.z) ); //bit shift to left (multiply by two)
                grid[x, y].layer = (Node.TerrainLayer)layer;
                grid[x, y].worldPosition = worldPoint;
                grid[x, y].gridX = x;
                grid[x, y].gridY = y;
            }
        }

        if(onGridCreated != null)
            onGridCreated.Invoke();
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


    public IEnumerable<GridPoint> GetClusterOutline(GridPoint[] cluster)
    {
        List<GridPoint> points = new List<GridPoint>();

        GridPoint temp;

        // Find all outline points
        for(int i = 0; i < cluster.Length; i++)
        {
            if(cluster[i].X < gridSizeX)
            {
                temp = new GridPoint(cluster[i].X + 1, cluster[i].Y);

                if(!cluster.Contains(temp))
                    points.Add(temp);
            }

            if(cluster[i].Y < gridSizeZ)
            {
                temp = new GridPoint(cluster[i].X, cluster[i].Y + 1);

                if(!cluster.Contains(temp))
                    points.Add(temp);
            }

            if(cluster[i].X > 0)
            {
                temp = new GridPoint(cluster[i].X - 1, cluster[i].Y);

                if(!cluster.Contains(temp))
                    points.Add(temp);
            }

            if(cluster[i].Y > 0)
            {
                temp = new GridPoint(cluster[i].X, cluster[i].Y - 1);

                if(!cluster.Contains(temp))
                    points.Add(temp);
            }
        }

        return points.AsEnumerable();
    }

    // Get terrain clusters of a specific layer
    public IEnumerable<GridPoint[]> GetClusters(Node.TerrainLayer layer)
    {
        int[,] floodfill = new int[gridSizeX, gridSizeZ];
        List<GridPoint[]> clusters = new List<GridPoint[]>();
        IList<GridPoint> current = new List<GridPoint>();

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeZ; y++)
            {
                // Prefill array
                if((layer & grid[x, y].layer) == layer)
                    floodfill[x, y] = 1;
            }
        }

        for(int x = 0; x < gridSizeX; x++)
        {
            for(int y = 0; y < gridSizeZ; y++)
            {
                // Run floodfill for every node
                FloodFill(x, y, ref floodfill, ref current);

                // If this new cluster is bigger than any currently saved clusters
                //if(!clusters.Exists(points => points.Length > current.Count))
                if(current.Count > 0)
                {
                    clusters.Add(current.ToArray());
                    current.Clear();
                }
            }
        }

        return clusters.AsEnumerable();
    }

    // Floodfill used for cluster algorithm
    private void FloodFill(int x, int y, ref int[,] map, ref IList<GridPoint> cluster)
    {
        // Not correct terrain?
        if(map[x, y] == 0)
            return;

        if(map[x, y] == 1)
        {
            map[x, y] = 2;
            cluster.Add(new GridPoint(x, y));

            // Recursive floodfill
            if(x < gridSizeX)
                FloodFill(x + 1, y, ref map, ref cluster);

            if(y < gridSizeZ)
                FloodFill(x, y + 1, ref map, ref cluster);

            if(x > 0)
                FloodFill(x - 1, y, ref map, ref cluster);

            if(y > 0)
                FloodFill(x, y - 1, ref map, ref cluster);
        }
    }

	public List<Node> path = new List<Node>();
	public List<Node> pathForDodo = new List<Node>();
	public List<Node> pathForWandering = new List<Node>();


	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			//Node playerNode = PositionTarget(player.transform.position);
			foreach (Node n in grid) {

				switch(n.layer)
				{
					case Node.TerrainLayer.Mountain:
						Gizmos.color = Color.red;
						break;

					case Node.TerrainLayer.Grass:
						Gizmos.color = Color.green;
						break;

					case Node.TerrainLayer.Sand:
						Gizmos.color = Color.yellow;
						break;

					case Node.TerrainLayer.Water:
						Gizmos.color = Color.blue;
						break;

					default:
						break;
				}

				if (pathForDodo.Contains (n)) {
					Gizmos.color = Color.black;


				}

				Gizmos.DrawCube(n.worldPosition, Vector3.one*(nodeDiameter-0.1f));
			}
		}
	}
}


// Represents a coordinate on the grid
// Can be used as indexer.
public struct GridPoint : System.IEquatable<GridPoint>
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
