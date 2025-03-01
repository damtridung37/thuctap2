using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public sealed class CharacterController : MonoBehaviour
{
  #region Constants

  private static readonly int ANIM_WALKING = Animator.StringToHash("Walk");

  private static readonly int ANIM_DIRECTION =
    Animator.StringToHash("Direction");

  #endregion


  #region Inspector

  [Min(0.1f)]
  [SerializeField]
  private float speed = 1;

  #endregion


  #region Fields

  private Animator _animator;

  private Rigidbody2D _rigidbody;

  #endregion


  #region MonoBehaviour

  private void Awake ()
  {
    _animator = GetComponent<Animator>();
    _rigidbody = GetComponent<Rigidbody2D>();
  }

  private void FixedUpdate ()
  {
    int? direction = null;
    Vector2 velocity = Vector2.zero;

    if ( Input.GetKey(KeyCode.UpArrow) )
    {
      direction = 2;
      velocity = new Vector2(0, 1);
    }
    else if ( Input.GetKey(KeyCode.DownArrow) )
    {
      direction = 0;
      velocity = new Vector2(0, -1);
    }
    else if ( Input.GetKey(KeyCode.RightArrow) )
    {
      direction = 1;
      velocity = new Vector2(1, 0);
    }
    else if ( Input.GetKey(KeyCode.LeftArrow) )
    {
      direction = 3;
      velocity = new Vector2(-1, 0);
    }

    _rigidbody.velocity = velocity * speed;
    _animator.SetBool(ANIM_WALKING, direction.HasValue);
    if ( direction.HasValue )
      _animator.SetInteger(ANIM_DIRECTION, direction.Value);
  }

  #endregion
}