using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [Header("Actions")]
    public InputAction _moveAction;
    public InputAction _jump;
    public InputAction _sneak;
    public InputAction _run;

    public states _currentState = states.IDLE;

    public bool _isGrounded;
    public bool _isMoving;
    public bool _isRunning = false;
    public bool _isSneaking = false;
    public bool _isJumping = false;
    public enum states
    {
        IDLE, JOGGING, RUNNING, FALLING, SNEAKING, JUMPING
    }

    public Rigidbody _rb;

    private void OnEnable()
    {
        _moveAction.Enable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckGround();
        Move();
        OnStateUpdate();
    }

    #region StateMachine
     public void OnStateEnter()
    {
        switch (_currentState)
        {
            case states.IDLE:
                break;
            case states.RUNNING:
                break;
            case states.JOGGING:
                break;
            case states.FALLING:
                break;
            case states.JUMPING:
                break;
            case states.SNEAKING:
                break;
        }        
    }

    public void OnStateUpdate()
    {
        switch (_currentState)
        {
            case states.IDLE:

                if (_isGrounded && _isMoving)
                {
                    TransitionToState(states.JOGGING);
                }

                if (!_isGrounded)
                {
                    TransitionToState(states.FALLING);
                }


                if (_isJumping)
                {
                    TransitionToState(states.JUMPING);
                }

                if (_isSneaking)
                {
                    TransitionToState(states.SNEAKING);
                }
                break;

            case states.JOGGING:
                if (!_isGrounded)
                {
                    TransitionToState(states.FALLING);
                }
                if (_isMoving && _isRunning)
                {
                    TransitionToState(states.RUNNING);
                }
                if (!_isMoving)
                {
                    TransitionToState(states.IDLE);
                }
                if (_isJumping)
                {
                    TransitionToState(states.JUMPING);
                }
                if (_isSneaking)
                {
                    TransitionToState(states.SNEAKING);
                }
                break;

            case states.RUNNING:
                if (_isRunning && !_isMoving)
                {
                    TransitionToState(states.IDLE);
                }
                if (!_isRunning && _isMoving)
                {
                    TransitionToState(states.JOGGING);
                }

                if (_isJumping && !_isGrounded)
                {
                    TransitionToState(states.JUMPING);
                }
                if (!_isGrounded)
                {
                    TransitionToState(states.FALLING);
                }
                break;

            case states.FALLING:
                if (_isGrounded)
                {
                    TransitionToState(states.IDLE);
                }
                break;

            case states.JUMPING:
                if (!_isGrounded)
                {
                    TransitionToState(states.FALLING);
                }
                break;

            case states.SNEAKING:
                if (!_isSneaking)
                {
                    TransitionToState(states.IDLE);
                }
                break;
        }
    }

    public void OnStateExit()
    {
        switch (_currentState)
        {
            case states.IDLE:
                break;
            case states.RUNNING:
                break;
            case states.JOGGING:
                break;
            case states.FALLING:
                break;
            case states.JUMPING:
                break;
            case states.SNEAKING:
                break;
        }
    }

    public void TransitionToState(states newState)
    {
        OnStateExit();
        _currentState = newState;
        OnStateEnter();
    }

    #endregion

    #region Methods

    public void Move()
    {
        Vector2 leftStick = _moveAction.ReadValue<Vector2>();

        if (leftStick.magnitude >0f)
        {
            // on met l'axe X en X parce qu'il nous deplace de gauche a droite
            // on ne touche pas a l'axe Y pour ne pas ecraser la vitesse de chute
            // On met l'axe Y pour se deplacer d'avant en arriere
            _rb.velocity = new Vector3(leftStick.x * 5f, _rb.velocity.y, leftStick.y* 5f) ;
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isJumping = true;
                break;
            case InputActionPhase.Canceled:
                _isJumping = false; 
                break;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position - new Vector3(0f, 1.1f, 0f), new Vector3(1f, 0.5f, 1f));
    }

    public void CheckGround()
    {
        Collider[] collider = Physics.OverlapBox(transform.position - new Vector3(0f, 1.1f, 0f), new Vector3(1f, 0.5f, 1f), Quaternion.identity, LayerMask.GetMask("Ground"));
        if (collider.Length > 0)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    #endregion
}
