using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

    public bool isOccupied
    {
        get
        {
            return occupier != null;
        }
    }

    //Variable to know whether some Player is already at this spot. Useful for future possible multiplayer
    protected PlayerPawn occupier;

    /// <summary>
    /// Function to Spawn Pawn at SpawnPoint position and have him shoot from there
    /// </summary>
    /// <param name="pawnPrefab"></param>
    /// <returns></returns>
    public PlayerPawn SpawnPawn(GameObject pawnPrefab)
    {
        GameObject playerPawnObject = Instantiate(pawnPrefab, transform.position, transform.rotation);
        PlayerPawn playerPawn = playerPawnObject.GetComponent<PlayerPawn>();
        occupier = playerPawn;

        return playerPawn;
    }
    

#if UNITY_EDITOR
    private float ballRadius = 0.1100606f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        DrawGizmos();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        DrawGizmos();
    }

    void DrawGizmos()
    {
        Vector3 ballOrigin = transform.position + Vector3.up * ballRadius;
        Gizmos.DrawWireSphere(ballOrigin, ballRadius);
        Gizmos.DrawRay(ballOrigin, transform.forward * 0.2f);
    }
#endif
}
