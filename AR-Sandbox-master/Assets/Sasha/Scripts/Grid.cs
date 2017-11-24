using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Transform player;

	Node[,] grid;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public DepthMesh mesh;

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
 	}


	public void SetNode(int _x, int _y, Node.TerrainLayer _layer){
		grid [_x, _y].layer = _layer;
	}

    void UpdateGrid(){
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2;
        Vector3 worldPoint;
        int layer;

        for (int x = 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeZ; y++) {
                worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius);

				layer = 1 << mesh.GetPixelLayer((int)worldPoint.x, (int)worldPoint.z); //bit shift to left (multiply by two)
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

	public List<Node> pathForDodo = new List<Node>();
	public List<Node> pathForWandering = new List<Node>();


	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			//Node playerNode = PositionTarget(player.transform.position);
			foreach (Node n in grid) {
				Gizmos.color = (n.layer == Node.TerrainLayer.Mountain) ? Color.white : Color.red;

				if (pathForDodo.Contains (n)) {
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
