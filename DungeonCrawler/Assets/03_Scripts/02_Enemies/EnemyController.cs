using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyBehaviour[] states;

    EnemyBehaviour _currentState;
    public EnemyBehaviour currentState
    {
        get => _currentState;
        set
        {
            if(_currentState != value)
            {
                if (_currentState != null)
                    _currentState.Release();

                _currentState = value;

                _currentState.Init();
            }
        }
    }

    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        RuntimeAnimatorController controllerInstance = Instantiate(animator.runtimeAnimatorController);
        animator.runtimeAnimatorController = controllerInstance;
    }

    private void Update()
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].Validate())
            {
                currentState = states[i];
                break;
            }
        }

        currentState.UpdateBehaviour();
    }
}
