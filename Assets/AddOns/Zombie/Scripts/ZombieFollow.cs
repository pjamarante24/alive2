using UnityEngine;
using UnityEngine.AI;

public class ZombieFollow : MonoBehaviour
{
    [SerializeField] public UnityEngine.AI.NavMeshAgent agent;
    [SerializeField] public GameObject target;
    [SerializeField] private Animator animator = null;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] public bool isDead = false;
    [SerializeField] public bool shouldCrawl = false;

    void Awake()
    {
        if (!animator) { animator = gameObject.GetComponent<Animator>(); }
        if (!agent) { agent = gameObject.GetComponent<NavMeshAgent>(); }
        if (!target) target = GameObject.FindGameObjectWithTag("PlayerCapsule");
    }

    private void Start()
    {
        animator.SetBool("Crawl", shouldCrawl);
    }

    void Update()
    {
        if (!target || isDead) return;

        agent.SetDestination(target.transform.position);

        float velocity = agent.velocity.magnitude;

        if (!TargetIsClose(500f))
        {
            velocity *= 2f;
        }

        animator.SetFloat("Velocity", velocity);

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

    public bool TargetIsClose(float distance = 6f)
    {
        return (transform.position - target.transform.position).sqrMagnitude < distance;
    }
}
