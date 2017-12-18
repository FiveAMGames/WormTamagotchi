using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Grid))]
public class Oasis : MonoBehaviour
{
    [SerializeField][Range(0f, 1f)] protected float chance = 0.2f;
    [SerializeField] protected int minArea = 8;
    [SerializeField] protected GameObject[] assets;
    [SerializeField][Range(0f, 1f)] protected float assetsAmount = 0.2f;
    [SerializeField][Range(1f, 60f)] protected int updateInterval = 10;
    [SerializeField] protected DepthMesh meshReference;
    [SerializeField] private bool debugSwitch;

    protected bool exists = false;
    protected float amount = 0f;
    protected List<GameObject> oasisAssets;

    protected Grid gridRef;
    protected List<GridPoint> cluster;
    protected GridPoint[] waterOutline;

    private bool running = false;
    private int updateCount = 0;

    public float WaterAmount
    {
        get { return amount; }
    }

    public bool IsExisting
    {
        get { return exists; }
    }

    private void Awake()
    {
        gridRef = GetComponent<Grid>();

        if(meshReference == null)
            Debug.LogError("[Oasis] No reference of the DepthMesh component assigned.");
    }

    private void Start()
    {
        cluster = new List<GridPoint>();
        oasisAssets = new List<GameObject>(20);
        running = true;
    }

    private void Update()
    {
        // Debug...
        if(Input.GetKeyDown(KeyCode.T))
        {
            waterOutline = null;
        }
    }

    // Register this method to the 'On Grid Created' event of the
    // grid component
    public void CreateOasis()
    {
        if(!running)
            return;

        if(updateCount >= updateInterval)
        {
            StartCoroutine(OasisAlgorithm());
            updateCount = 0;
        }
        else
        {
            updateCount++;
        }


    }

    IEnumerator OasisAlgorithm()
    {
        GridPoint[] cluster = null;
        IEnumerable<GridPoint> outline;
        int lastSize = 0;
        int waterCount = 0;

        // Create lake for debugging?
        if(debugSwitch)
            DebugOasis();

        // Find biggest cluster
        foreach(var c in gridRef.GetClusters(Node.TerrainLayer.Water))
        {
            if(c.Length > lastSize)
            {
                lastSize = c.Length;
                cluster = c;
            }

            waterCount += c.Length;

            // Opt out handle
            yield return null;
        }

        // Calculate the amount of water in the box
        amount = Mathf.Clamp01((float)waterCount / 500f);

        // No cluster found or too small
        if((cluster == null) || (cluster.Length < minArea))
        {
            if(exists)
                BuildOasis();

            yield break;
        }

        outline = gridRef.GetClusterOutline(cluster);

        if(waterOutline == null)
        {
            waterOutline = outline.ToArray();
            BuildOasis();
            yield break;
        }

        int i = 0;
        int numberOfEqual = 0;

        foreach(var gp in outline)
        {
            if((i < waterOutline.Length) && gp.Equals(waterOutline[i]))
                numberOfEqual++;

            i++;

            // Opt out handle
            //yield return null;
        }
        print(numberOfEqual);
        // Not the same oasis! Let old die or create new...
        if(numberOfEqual < (int)(waterOutline.Length * 0.6f))
        {
            waterOutline = outline.ToArray();
            BuildOasis();
        }
    }

    // Let current oasis die and maybe build a new one...
    // Instantiate vegetation, objects and stuff
    protected void BuildOasis()
    {
        if((meshReference != null) && meshReference.outOfBounds)
            return;

        // Oasis exists?
        if(exists)
        {
            // Let it die...
            for(int i = 0; i < oasisAssets.Count; i++)
            {
                GameObject.Destroy(oasisAssets[i], Random.Range(0f, 2f));
            }

            oasisAssets.Clear();
            waterOutline = null;
            exists = false;
        }
        else if(assets != null)
        {
            // Consider chance to build oasis
            if(Random.Range(0f, 1f) > chance)
                return;

            // Chance to build a new one...
            for(int i = 0; i < waterOutline.Length; i++)
            {
                if(Random.Range(0f, 1f) < (assetsAmount * assetsAmount))
                {
                    oasisAssets.Add(
                        GameObject.Instantiate(
                            assets[Random.Range(0, assets.Length)],
                            gridRef.GetNode(waterOutline[i].X, waterOutline[i].Y).worldPosition + new Vector3(
                                Random.Range(-1.5f, 1.5f),
                                0f,
                                Random.Range(-1.5f, 1.5f)
                            ),
                            Quaternion.Euler(0f, Random.Range(0f, 360f), 0f)
                        )
                    );
                }
            }

            exists = true;
        }
    }

    // Create lake for debugging
    private void DebugOasis()
    {
        // Create lake
        for(int x = 26; x < 45; x++)
        {
            for(int y = 15; y < 36; y++)
            {
                // Set some nodes to water
                gridRef.SetNode(x, y, Node.TerrainLayer.Water);
            }
        }
    }
}
