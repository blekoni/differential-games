using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeInStaticDirection : Behavior
{
    [SerializeField] Vector2 direction;

    public EscapeInStaticDirection(Vector2 staticDir)
    {
        direction = staticDir;
    }

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        return direction.normalized;
    }
}
