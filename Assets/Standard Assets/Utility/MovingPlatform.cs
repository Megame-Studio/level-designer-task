using System;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [z_TagSelector]
    public List<string> tags;

    private void OnTriggerEnter(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            if (other.attachedRigidbody == null)
            {
                other.transform.SetParent(transform);
            }
            else
            {
                other.attachedRigidbody.transform.SetParent(transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (tags.Contains(other.tag))
        {
            if (other.attachedRigidbody == null)
            {
                other.transform.SetParent(null);
            }
            else
            {
                other.attachedRigidbody.transform.SetParent(null);
            }
        }
    }
}