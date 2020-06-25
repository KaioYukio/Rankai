using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetIndicator : MonoBehaviour
{
    public Transform targetUI;
    public SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = targetUI.GetComponent<SpriteRenderer>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (targetUI == null)
        {
            targetUI = GameObject.FindGameObjectWithTag("TargetGraph").transform;
            return;
        }

        if (sprite == null)
        {
            sprite = targetUI.GetComponent<SpriteRenderer>();
            return;
        }


        if (GameManager.instance.IsPlayable())
        {
            if (targetUI.transform.parent == null)
            {
                return;
            }

            if (targetUI.transform.parent.tag == "TargetIndicator")
            {
                sprite.enabled = true;
            }
            else
            {
                sprite.enabled = false;
            }
        }


    }

    public bool PlayerIsLocked()
    {
        if (sprite.enabled)
        {
            return true;

        }
        else
        {
            return false;
        }
    }

    public void RemoveParent()
    {
        if (sprite == null  || targetUI == null)
        {
            return;
        }
        sprite.enabled = false;
        targetUI.transform.SetParent(null);
    }

    public void PlaceTargetIndicator(Transform enTarget)
    {
        if (enTarget != null)
        {
            targetUI.position = enTarget.position;
            targetUI.SetParent(enTarget.transform);
        }
        else
        {

        }

    }
}
