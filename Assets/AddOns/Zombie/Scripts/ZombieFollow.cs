using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    [SerializeField] private UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] public GameObject target;
    [SerializeField] private Animator animator = null;
    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isDead = false;

    void Awake()
    {
        if (!animator) { animator = gameObject.GetComponent<Animator>(); }
        if (!agent) { agent = gameObject.GetComponent<NavMeshAgent>(); }
        if (!target) target = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        if (!target || isDead) return;

        agent.SetDestination(target.transform.position);

        float velocity = agent.velocity.magnitude;

        animator.SetBool("Walking", velocity > 0.1);

        if (TargetIsClose() && !isAttacking)
        {
            isAttacking = true;
            animator.SetBool("Attacking", true);
        }
        else if (!TargetIsClose() && isAttacking)
        {
            isAttacking = false;
            animator.SetBool("Attacking", false);
        };
    }

    public void Kill(bool isHeadshot = false)
    {
        isDead = true;
        agent.enabled = false;
        if (isHeadshot) animator.SetBool("Dying 2", true);
        else animator.SetBool("Dying", true);
    }

    public bool TargetIsClose()
    {
        return (transform.position - target.transform.position).sqrMagnitude < 6f;
    }
}
