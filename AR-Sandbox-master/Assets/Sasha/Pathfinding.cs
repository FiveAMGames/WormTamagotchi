using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinding: MonoBehaviour {

	Grid grid;

	public Transform seeker, target;


	void Awake(){
		
		grid = GetComponent<Grid> ();
	}

	void Update(){
		//FillBox (10, 10, 20, 20);
			FindPath (seeker.position, target.position);
		
	}

	void FillBox(int x1, int y1, int x2, int y2) {
		for (int i = x1; i <= x2; ++i)
			for (int j = y1; j <= y2; ++j)
				grid.SetNode (i, j, false);
	}





	void FindPath(Vector3 startPos, Vector3 targetPos){
		Node startNode = grid.PositionTarget (startPos);
		Node targetNode = grid.PositionTarget (targetPos);

        // Failsave - Julian 21/11/2017
        if((startNode == null) || (targetNode == null))
            return;

		HashSet<Node> openSet = new HashSet<Node> ();
		HashSet<Node> closedSet = new HashSet<Node> ();


		openSet.Add (startNode);


		int count1 = 0;
		int count2 = 0;


		while (openSet.Count > 0) {
			count1++;
			Node currentNode = null;
			foreach (Node cur in openSet ) {
				count2++;
				if (currentNode == null || cur.fCost < currentNode.fCost){// || cur.fCost == currentNode.fCost && cur.hCost < currentNode.hCost) {
					currentNode = cur;
				}
			}

			openSet.Remove (currentNode);
			closedSet.Add (currentNode);


			if (currentNode == targetNode) {  //found
				RetracePath(startNode, targetNode);
				Debug.Log (count1 + " " + count2);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains (neighbour)) {
					continue;
				}
				int newMovementCostToNeighbour = currentNode.gCost + GetDistance (currentNode, neighbour);

				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {
					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour, targetNode);

					neighbour.parent = currentNode;



					openSet.Add (neighbour);

				}

			}

		}
	}


	void RetracePath(Node startNode, Node endNode){

		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode){
			path.Add (currentNode);
			currentNode = currentNode.parent;
		}

		path.Reverse ();

		grid.path = path;

		seeker.position = Vector3.MoveTowards (seeker.position, new Vector3(path [1].worldPosition.x, seeker.position.y, path [1].worldPosition.z), 10 * Time.deltaTime); 




		seeker.LookAt(target);
		seeker.rotation = Quaternion.FromToRotation(transform.right, Vector3.right) * seeker.rotation;


	}



	int GetDistance(Node nodeA, Node nodeB){

		int distX = Mathf.Abs (nodeA.gridX - nodeB.gridX);
		int distY = Mathf.Abs (nodeA.gridY - nodeB.gridY);

		if (distX > distY) 
			return 14 * distY + 10 * (distX - distY);   //1.4 is a square root of 2
		return 14 * distX + 10 * (distY - distX);
		



	}

}
