using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor.Animations;
using UnityEngine;

public class DieRoller2D : MonoBehaviour
{
    public event Action<int> OnRoll;
    public int Result { get; private set; }

    [SerializeField] bool _usePhysics = true;                       //Not Used Anymore.
    [SerializeField] private bool _isSelected = false;

    public bool IsRolling()
    {
        return _isRolling;
    }

    private SpriteRenderer _spriteRend;
    private Color _defaultColor, _selectedColor;
    [Tooltip("Roll time only applies when not using physics")]
    [SerializeField] float _rollTime = 2f;

    public IEnumerator RollDieWithDelay(float delay, int value = 0)             
    {
        yield return new WaitForSeconds(delay); 
        RollDie(value); 
    }

    public void RollDie(int value = 0)
    {
        Result = value == 0 ? UnityEngine.Random.Range(1, 6) : value;
        RollWithoutPhysics();       
    }

    Rigidbody2D _rigidbody2D;
    Animator _animator;
    static readonly int RollingAnimation = Animator.StringToHash("Roll");
    static readonly int ThrowingAnimation = Animator.StringToHash("Throw");  
    static readonly int[] ResultAnimations = new[]
    {
        Animator.StringToHash("Roll1"),
        Animator.StringToHash("Roll2"),
        Animator.StringToHash("Roll3"),
        Animator.StringToHash("Roll4"),
        Animator.StringToHash("Roll5"),
        Animator.StringToHash("Roll6")
    };

    bool _isRolling;
    float _timeRemaining;
    RandomAudioClipPlayer _audioClipPlayer;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>(); // Check that this component exists on the GameObject
        _spriteRend = GetComponent<SpriteRenderer>(); // Ensure this is not null
        if (_spriteRend == null)
        {
            Debug.LogError("SpriteRenderer component is missing on " + gameObject.name);
        }
        _defaultColor = _spriteRend.color;
        _selectedColor = Color.yellow; // Highlights in yellow
        _animator = GetComponent<Animator>(); // Ensure this is assigned correctly
        _audioClipPlayer = GetComponent<RandomAudioClipPlayer>(); // Check if this is used and assigned
    }


    void OnMouseDown() // When dice are clicked
    {
        ToggleSelected();
    }


    void ToggleSelected()
    {
        _isSelected = !_isSelected;
        _spriteRend.color = _isSelected ? _selectedColor : _defaultColor;
    }

    public bool IsSelected() => _isSelected;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isRolling)
        {
            StartCoroutine(RollDieWithDelay(UnityEngine.Random.Range(0f, 0.5f)));       
        }
        if (!_isRolling) return;
        _timeRemaining -= Time.deltaTime;
        if (_usePhysics || _timeRemaining > 0f) return;     //Not Used Anymore.
        FinishRolling();
    }

    void RollWithoutPhysics()
    {
        _animator.SetTrigger(RollingAnimation);
        _animator.SetTrigger(ThrowingAnimation);    
        _isRolling = true;
        _timeRemaining = _rollTime;
    }

    void FinishRolling()
    {
        _isRolling = false;
        _audioClipPlayer?.PlayRandomClip();
        _animator.SetTrigger(ResultAnimations[Result - 1]);
        OnRoll?.Invoke(Result);

        // Log to confirm the result of the roll
        Debug.Log($"Die rolled and landed on: {Result}");
    }

    public void ResetSelection()
    {
        _isSelected = false;
        if (_spriteRend != null)
        {
            _spriteRend.color = _defaultColor;
        }
        else
        {
            Debug.LogWarning("ResetSelection called, but SpriteRenderer is not assigned on " + gameObject.name);
        }
    }

    public void ToggleActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
}
