using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour : StateMachineBehaviour
{
    private GameObject[] patrolPoints;

    int randomPoint;

    public float speed;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        patrolPoints = GameObject.FindGameObjectsWithTag("PatrolPoints");
        randomPoint = Random.Range(0, patrolPoints.Length);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.transform.position = Vector2.MoveTowards(animator.transform.position, patrolPoints[randomPoint].transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(animator.transform.position, patrolPoints[randomPoint].transform.position) < .1f)
        {
            randomPoint = Random.Range(0, patrolPoints.Length);
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    
}
