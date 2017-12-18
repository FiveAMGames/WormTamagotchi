using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class PopupShapeAnimation : MonoBehaviour
{
    [SerializeField] protected float speed = 4f;
    [SerializeField] protected float delay = 0f;
    [SerializeField] protected bool playOnAwake = true;
    [SerializeField] protected bool invertBlending = false;
    [SerializeField] protected bool resetForm = false;

    protected SkinnedMeshRenderer renderer;
    protected bool playing = false;

    protected int startIndex, formIndex;

    private float percentage = 0f;
    private bool reset = false;
    private bool subtract = false;
    private float time = 0f;


    private void Start()
    {
        renderer = GetComponent<SkinnedMeshRenderer>();

        startIndex = renderer.sharedMesh.GetBlendShapeIndex("Start");
        formIndex = renderer.sharedMesh.GetBlendShapeIndex("Form");

        if(startIndex == -1)
            Debug.LogError("[PopupShapeAnimation] No 'Start' blend shape defined!");

        percentage = renderer.GetBlendShapeWeight(startIndex);
        subtract = percentage > 99f;
        
        if(playOnAwake)
            Play();
    }

    private void Update()
    {
        if(playing && !reset)
        {
            renderer.SetBlendShapeWeight(startIndex, percentage);

            if(formIndex != -1)
                renderer.SetBlendShapeWeight(formIndex, invertBlending ? 100f - percentage : percentage);

            if(!subtract)
            {
                percentage += Time.deltaTime * speed * 10f;

                if(percentage >= 100f)
                {
                    percentage = invertBlending ? 0f : 100f;

                    // Fix 'Start' blend shape
                    StartCoroutine(SetBlendShapeNextFrame(startIndex, invertBlending ? 100f - percentage : percentage));

                    if(resetForm)
                    {
                        reset = true;
                    }
                    else
                    {
                        if(formIndex != -1)
                            StartCoroutine(SetBlendShapeNextFrame(formIndex, invertBlending ? percentage : 100f - percentage));
                        
                        playing = false;
                    }
                }
            }
            else
            {
                percentage -= Time.deltaTime * speed * 10f;

                if(percentage <= 0f)
                {
                    percentage = invertBlending ? 100f : 0f;

                    // Fix 'Start' blend shape
                    StartCoroutine(SetBlendShapeNextFrame(startIndex, invertBlending ? 100f - percentage : percentage));

                    if(resetForm)
                    {
                        reset = true;
                    }
                    else
                    {
                        if(formIndex != -1)
                            StartCoroutine(SetBlendShapeNextFrame(formIndex, invertBlending ? percentage : 100f - percentage));

                        playing = false;
                    }
                }
            }
        }
        else if(playing && reset)
        {
            if(formIndex != -1)
                renderer.SetBlendShapeWeight(formIndex, percentage);

            if(!invertBlending)
            {
                percentage += Time.deltaTime * speed * 10f;

                if(percentage >= 100f)
                {
                    percentage = 100f;

                    if(formIndex != -1)
                        StartCoroutine(SetBlendShapeNextFrame(formIndex, percentage));
                    
                    playing = false;
                }
            }
            else
            {
                percentage -= Time.deltaTime * speed * 10f;

                if(percentage <= 0f)
                {
                    percentage = 0f;

                    if(formIndex != -1)
                        StartCoroutine(SetBlendShapeNextFrame(formIndex, percentage));
                    
                    playing = false;
                }
            }
        }
    }

    private IEnumerator SetBlendShapeNextFrame(int index, float value)
    {
        yield return new WaitForEndOfFrame();
        yield return null;
        renderer.SetBlendShapeWeight(index, value);
    }

    private IEnumerator DelayStart(float seconds = 0f)
    {
        if(seconds > 0f)
            yield return new WaitForSeconds(seconds);
        
        playing = true;
        reset = false;
        percentage = subtract ? 100f : 0f;
    }

    public void Play()
    {
        StartCoroutine(DelayStart(delay));
    }
}
