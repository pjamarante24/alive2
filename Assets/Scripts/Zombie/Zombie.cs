using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private GameObject target;
    [SerializeField] private HealthPlayer targetHealth;
    [SerializeField] private Animator animator = null;

    [SerializeField] private float initialHealth = 100f;
    [SerializeField] private float currentHealth;
    [SerializeField] private float damagePerAttack = 20f;
    [SerializeField] private bool isDead = false;

    private void Awake()
    {
        if (!animator) { animator = gameObject.GetComponent<Animator>(); }
        if (!agent) { agent = gameObject.GetComponent<NavMeshAgent>(); }
        if (!target)
        {
            target = GameObject.FindGameObjectsWithTag("Player")[0];
            targetHealth = target.GetComponent<HealthPlayer>();
        }
    }

    private void Start()
    {
        currentHealth = initialHealth;
    }

    private void Update()
    {
        if (!target) return;

        if (isDead)
        {
            agent.enabled = false;
            return;
        }

        agent.SetDestination(target.transform.position);

        float velocity = agent.velocity.magnitude;

        animator.SetFloat("MoveSpeed", velocity * 2);

        if (velocity < 0.5f && TargetIsClose())
        {
            animator.SetTrigger("Attack");
        }
    }

    private void Attack()
    {
        if (!targetHealth) return;
        if (TargetIsClose()) targetHealth.TakeDamage(damagePerAttack);

    }

    private bool TargetIsClose()
    {
        return (transform.position - target.transform.position).sqrMagnitude < 6f;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Dead");
            isDead = true;
            Destroy(gameObject, 2f);
        }
    }
}
