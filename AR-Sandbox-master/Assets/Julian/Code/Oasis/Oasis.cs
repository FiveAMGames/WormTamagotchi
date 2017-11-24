using System.Collections;
using System.Collections.Generic;
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

    // Register this method to the 'On Grid Created' event of the
    // grid component
    public void CreateOasis()
    {
        
    }
}
