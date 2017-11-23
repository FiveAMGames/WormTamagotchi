using UnityEngine;
using System.Collections;

public class Node  {

    // Node properties
	public TerrainLayer layer;
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
    /// <param name="_gridX">Grid coordinate x.</param>
    /// <param name="_gridY">Grid coordinate y.</param>
    public Node(TerrainLayer _layer, Vector3 _worldPos, int _gridX, int _gridY){
        layer = _layer;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	
	}

    [System.Flags]
    public enum TerrainLayer
    {
        Water = 0x01,
        Sand = 0x02,
        Grass = 0x04,
        Mountain = 0x08
    }

    // Following is used for astar algorithm
	public int fCost{
		get{ return gCost + hCost; }

	}
}
