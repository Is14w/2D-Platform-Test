using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryEcho : MonoBehaviour
{
    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
