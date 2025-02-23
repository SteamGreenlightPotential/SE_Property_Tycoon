using System.Collections;
using UnityEngine;

public class Grid_Movement : MonoBehaviour
{
    private bool isMoving = false;
    private Vector3 origPos, targetPos;
    private float TimeToMove = 0.2f;
    private int TileCount = 0;

    public void Move(int steps)  // Called from Turn_Script
    {
        StartCoroutine(ProcessMovements(steps));
    }

    private IEnumerator ProcessMovements(int steps)
    {
        for (int i = 0; i < steps; i++)
        {
            Vector3 direction = NextDir();
            yield return StartCoroutine(MovePlayer(direction));
        }
    }

    private Vector3 NextDir()
    {
        Vector3 direction = Vector3.zero;
        if (TileCount >= 0 && TileCount < 10)
        {
            direction = Vector3.right;
            transform.eulerAngles = new Vector3(180, 0, 270);
        }
        else if (TileCount >= 10 && TileCount < 20)
        {
            direction = Vector3.down;
            transform.eulerAngles = new Vector3(180, 0, 0);
        }
        else if (TileCount >= 20 && TileCount < 30)
        {
            direction = Vector3.left;
            transform.eulerAngles = new Vector3(180, 0, 90);
        }
        else if (TileCount >= 30 && TileCount < 40)
        {
            direction = Vector3.up;
            transform.eulerAngles = new Vector3(180, 0, 180);
        }
        else
        {
            TileCount = 0;
            direction = Vector3.right;
        }

        TileCount += 1;
        return direction;
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < TimeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / TimeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isMoving = false;
    }
}