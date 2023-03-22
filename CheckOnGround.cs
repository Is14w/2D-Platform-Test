using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnGround : MonoBehaviour
{
    [SerializeField] public static bool isOnGround;
    public LayerMask layerMask;

    private void Update()
    {
        var raycastAll = Physics2D.OverlapBoxAll(transform.position, new Vector2(0.5f, 0.1f), 0, layerMask);
        if (raycastAll.Length > 0)
        {
            isOnGround = true;
            PlayerController.canDoubleJump = true;
        }
        else
        {
            isOnGround = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(0.5f, 0.1f));
    }
}
