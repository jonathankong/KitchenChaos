using UnityEngine;

public class PlayerStateController : StateController, IHasAnimator
{ 
    [SerializeField]
    private InputReader _inputReader;

    [Header("Player-Specific Components")]
    [SerializeField]
    private FloatReference _minMoveMagThreshold;

    #region Getters
    public Animator @Animator { get; private set; }
    public FloatReference MinMoveMagThreshold => _minMoveMagThreshold;
    public Vector2 MoveDirection { get; private set; }
    #endregion

    #region UnityMethods
    private void Awake()
    {
        @Animator = GetComponentInChildren<Animator>();    
    }
    public override void Start()
    {
        base.Start();
        GameRestart();
    }
    private void OnEnable()
    {
        _inputReader.MovePerformed += OnMoveInput;
    }

    private void OnDisable()
    {
        _inputReader.MovePerformed -= OnMoveInput;
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