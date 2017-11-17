using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	public Transform player;

	Node[,] grid;
	public Vector2 gridWorldSize;
	public float nodeRadius;
	public LayerMask unwalkableMask;
	public DepthMesh mesh;

	float nodeDiameter;
	int gridSizeX, gridSizeZ;
	float timeTimer = 0f;


	void Awake(){
		CreateGrid ();
	}
	void Start(){
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt( gridWorldSize.x / nodeDiameter);
		gridSizeZ = Mathf.RoundToInt( gridWorldSize.y / nodeDiameter);
		//CreateGrid ();
	}


	void Update(){
		timeTimer += Time.deltaTime;

		if (timeTimer>0.3f){
			CreateGrid ();
			timeTimer = 0;
		}

			

	}

	public void SetNode(int x, int y, bool walk){
		grid [x, y].walkable = walk;

	}



	void CreateGrid(){
		grid = new Node[gridSizeX, gridSizeZ];
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward*gridWorldSize.y/2;
		for (int x = 0; x< gridSizeX; x++){
			for (int y = 0; y < gridSizeZ; y++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter+nodeRadius);
				bool walkable = true;
				if (x < gridSizeX-1 && y < gridSizeZ-1) {
				walkable = (mesh.GetPixelLayer (Mathf.RoundToInt (worldPoint.x), Mathf.RoundToInt (worldPoint.z)) == 1);
				
				
				}


				grid [x, y] = new Node (walkable, worldPoint, x, y);
			}
		}
 	}


	public Node NodeFromWorldPoint(Vector3 worldPosition){
		float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
		float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
		percentX = Mathf.Clamp01 (percentX);
		percentY = Mathf.Clamp01 (percentY);

		int x =Mathf.RoundToInt ((gridSizeX - 1) * percentX);
		int y =Mathf.RoundToInt ((gridSizeZ - 1) * percentY);
		return grid [x, y];
	}

	public Node PositionTarget(Vector3 tr){
		return grid [Mathf.RoundToInt (tr.x/nodeDiameter), Mathf.RoundToInt (tr.z/nodeDiameter)];
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

	public List<Node> path = new List<Node>();


	void OnDrawGizmos(){
		Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			//Node playerNode = PositionTarget(player.transform.position);
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable) ? Color.white : Color.red;

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
