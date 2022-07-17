using UnityEngine;

public interface IStateBase
{
    void EnterState();
    void UpdateState(float deltaTime);
    void ExitState();
    void ProcessInput(Vector2 movement, Vector3 look);
}
