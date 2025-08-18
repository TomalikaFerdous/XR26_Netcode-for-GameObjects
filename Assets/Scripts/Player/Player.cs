using UnityEngine;
using Unity.Netcode;

[RequireComponent(typeof(Rigidbody))]
public class Player : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        Vector3 input = new Vector3(
            Input.GetAxis("Horizontal"),
            0f,
            Input.GetAxis("Vertical")
        );

        // Normalize input so diagonal isn't faster
        if (input.magnitude > 1f)
            input.Normalize();

        // Send input direction to the server
        MoveServerRpc(input);
    }

    [ServerRpc]
    private void MoveServerRpc(Vector3 input, ServerRpcParams rpcParams = default)
    {
        // Server-side movement using Rigidbody
        Vector3 move = input * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
}
