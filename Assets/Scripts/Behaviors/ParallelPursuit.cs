using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelPursuit : Behavior
{
    private Vector2 m_xAxis = new Vector2(1.0f, 0.0f);
    private float m_speed = 1.0f;

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        Player closestEscaper = GetClosestEnemy(GameManager.PlayerRole.Escaper, currentPos, false);
        if (!closestEscaper)
        {
            return new Vector2(0.0f, 0.0f);
        }

        var enemySpeed = closestEscaper.GetSpeed();
        var coef = enemySpeed / m_speed;

        var toEscaperVec = (closestEscaper.GetPosition() - currentPos).normalized;
        var fi = Vector2.SignedAngle(Vector2.right, toEscaperVec);
        var alpha = Vector2.SignedAngle(Vector2.right, closestEscaper.GetDirection());
        var fiminusAlpha = fi - alpha;
        var sinfiminusalpha = Mathf.Sin(fiminusAlpha * Mathf.Deg2Rad);
        var sinfiminsalphapursuer = sinfiminusalpha * coef;
        var fiminusalphapursuer = Mathf.Asin(sinfiminsalphapursuer) * Mathf.Rad2Deg;
        var alphaPusuer = fi - fiminusalphapursuer;

        Vector2 newDir = MathUtil.Rotate(Vector2.right, alphaPusuer);

        Debug.Log(fi);
        //var toEscaperVec = (closestEscaper.GetPosition() - currentPos).normalized;

        //var fi = Vector2.Angle(m_xAxis, toEscaperVec);
        //var angleEscaper = Vector2.Angle(m_xAxis, closestEscaper.GetDirection());


        //var fiMunisAngleEscaper = fi - angleEscaper;

        //var newAngle = fi - coef * fiMunisAngleEscaper;







        //var dirToPursuer = (closestEscaper.GetPosition() - currentPos);
        //dirToPursuer = dirToPursuer.normalized;

        //var crossProduct = Vector3.Cross(MathUtil.Vec2ToVec3(dirToPursuer), MathUtil.Vec2ToVec3(closestEscaper.GetDirection())).normalized;
        //float sign = crossProduct.y > 0.0f ? -1.0f : 1.0f;
        //var angle3d = Vector3.SignedAngle(MathUtil.Vec2ToVec3(dirToPursuer), MathUtil.Vec2ToVec3(closestEscaper.GetDirection()), -crossProduct);







        //var angle = Vector2.Angle(dirToPursuer, closestEscaper.GetDirection()) ;
        //var angleSinus = Mathf.Sin(angle) * coef;
        //var newAngle = Mathf.Asin(angleSinus);
        //newAngle = Mathf.Rad2Deg * newAngle;

        //var newAngle2 = angle * coef * sign;

        //Debug.Log(newAngle);

        //Vector2 newDir = MathUtil.Rotate(dirToPursuer, newAngle2).normalized;
        return newDir;
    }
}
