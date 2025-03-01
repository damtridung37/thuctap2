using D;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Animator anim;

    //Constants
    private const string IDLE = "Idle";
    private const string IDLE_LEFT = "IdleLeft";
    private const string IDLE_RIGHT = "IdleRight";
    private const string IDLE_TOP = "IdleTop";
    private const string WALK = "Walk";
    private const string WALK_LEFT = "WalkLeft";
    private const string WALK_RIGHT = "WalkRight";
    private const string WALK_TOP = "WalkUP";

    //Flags
    private Vector2 currentDirection;

    private void Input()
    {
        currentDirection = joystick.Direction;
    }

    private void AnimationControl()
    {

        if (currentDirection == Vector2.zero)
        {
            anim.Play(IDLE);
            return;
        }

        if (Mathf.Abs(currentDirection.x) > Mathf.Abs(currentDirection.y))
        {
            if (currentDirection.x > 0)
            {
                anim.Play(WALK_RIGHT);
            }
            else
            {
                anim.Play(WALK_LEFT);
            }
        }
        else
        {
            if (currentDirection.y > 0)
            {
                anim.Play(WALK_TOP);
            }
            else
            {
                anim.Play(WALK);
            }
        }
    }

    private void Update()
    {

        Input();

        AnimationControl();

    }
}
