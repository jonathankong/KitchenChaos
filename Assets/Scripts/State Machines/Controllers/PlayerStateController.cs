using UnityEngine;

public class PlayerStateController : StateController
{ 
    [SerializeField]
    private InputReaderSO _inputReader;

    [Header("Player-Specific Components")]
    [SerializeField]
    private FloatReference _minMoveMagThreshold;

    public Animator PlayerAnimator { get; private set; }
    public FloatReference MinMoveMagThreshold => _minMoveMagThreshold;
    public Vector2 MoveDirection { get; private set; }

    #region UnityMethods
    private void Awake()
    {
        PlayerAnimator = GetComponentInChildren<Animator>();    
    }

    public override void Start()
    {
        base.Start();
        GameRestart();
    }
    private void OnEnable()
    {
        _inputReader.MoveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.MoveEvent -= OnMoveInput;
    }
    #endregion
    // this should be added to the GameRestart EventListener as callback
    public void GameRestart()
    {
        // set the start state
        TransitionToState(startState);
    }

    private void OnMoveInput(Vector2 vector) => MoveDirection = vector;
}