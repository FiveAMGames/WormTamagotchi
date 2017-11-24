using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Oasis : MonoBehaviour
{
    [SerializeField][Range(0f, 100f)] protected float chance = 20f;
    [SerializeField] protected int minArea = 8;

    protected bool exists = false;
    protected Grid gridRef;

    public bool IsExisting
    {
        get { return exists; }
    }

    private void Start()
    {
        gridRef = GetComponent<Grid>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            int lastSize = 0;
            List<GridPoint> cluster = new List<GridPoint>();

            // Find biggest cluster
            foreach(var c in gridRef.GetClusters(Node.TerrainLayer.Grass))
            {
                if(c.Length > lastSize)
                {
                    lastSize = c.Length;
                    cluster = c.ToList();
                }
            }

            if(cluster != null)
            {
                foreach(var c in gridRef.GetClusterOutline(cluster.ToArray()))
                {
                    Debug.LogFormat("{0}/{1}", c.X, c.Y);
                }
            }
        }
    }

    // Register this method to the 'On Grid Created' event of the
    // grid component
    public void CreateOasis()
    {
        
    }
}
