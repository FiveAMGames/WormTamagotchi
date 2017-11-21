using UnityEngine;
using System.Collections;

public class Node  {

    // Node properties
	public bool walkable;
	public Vector3 worldPosition;

    // Grid coordinates
	public int gridX;
	public int gridY;

    // Following is used for astar algorithm
    public int gCost;
    public int hCost;
	public Node parent;

    /// <summary>
    /// Initializes a new instance of the <see cref="Node"/> class.
    /// </summary>
    /// <param name="_walkable">If set to <c>true</c>, this node tile is walkable.</param>
    /// <param name="_worldPos">World position.</param>
    /// <param name="_gridX">Grid x.</param>
    /// <param name="_gridY">Grid y.</param>
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY){
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	
	}

    // Following is used for astar algorithm
	public int fCost{
		get{ return gCost + hCost; }

	}
}
