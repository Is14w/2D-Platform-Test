using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] public float time;
    [SerializeField] public float startTime;
    [SerializeField] public int damage;
    private Animator anim;
    private PolygonCollider2D coll;
    public static bool isAttacking;
    private Coroutine attackCoroutine;

    private void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        coll = GetComponent<PolygonCollider2D>();
    }

    public void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            anim.SetBool("CancelAttack", false);
            isAttacking = true;
            anim.SetTrigger("Attack");
            attackCoroutine = StartCoroutine("StartAttack");
        }
    }

    private void Update()
    {
        Attack();
        if (Input.GetKeyDown(KeyCode.Space) || PlayerController.isDash && isAttacking)
        {
            if (attackCoroutine != null)
            {
                StopCoroutine(attackCoroutine);
            }
            coll.enabled = false;
            isAttacking = false;
            PlayerController.moveBias = 1.0f;
            anim.SetBool("CancelAttack", true);
        }
    }

    IEnumerator StartAttack()
    {
        PlayerController.moveBias = 0.65f;
        yield return new WaitForSeconds(startTime);
        coll.enabled = true;
        StartCoroutine(disableHitBox());
    }


    IEnumerator disableHitBox()
    {
        anim.SetBool("CancelAttack", true);
        yield return new WaitForSeconds(time);
        PlayerController.moveBias = 1.0f;
        coll.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyBase>().TakeDamage(damage);
        }
    }
}
