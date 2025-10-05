using UnityEngine;

public class GameSequence : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;


    private void StartSequence()
    {
        _playerMovement.ResetPosition();



    }

    private void EndSequence()
    {

    }
}
