
using UnityEngine;

public class Slippery : MonoBehaviour
{
    [SerializeField] private float grip = 0.04f;

    void OnCollisionEnter(Collision hit)
    {
        PlayerMove move = hit.collider.GetComponent<PlayerMove>();

        if (move != null) move.SetGrip(grip);
    }

    void OnCollisionExit(Collision hit)
    {
        PlayerMove move = hit.collider.GetComponent<PlayerMove>();

        if (move != null) move.SetGrip(1f);
    }
}
