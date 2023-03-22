using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallelPursuit2 : Behavior
{
    private Vector2 m_xAxis = new Vector2(1.0f, 0.0f);
    private float m_speed = 1.0f;

    public override Vector2 GetNextStepDirection(Vector2 currentPos, Vector2 currentDir)
    {
        Player closestEscaper = GetClosestEnemy(GameManager.PlayerRole.Escaper, currentPos, true);
        if (!closestEscaper)
        {
            return new Vector2(0.0f, 0.0f);
        }

        var enemySpeed = closestEscaper.GetSpeed();
        var coef = enemySpeed / m_speed;

        var toEscaperVec = (closestEscaper.GetPosition() - currentPos).normalized;
        

        var fi = Vector2.Angle(m_xAxis, toEscaperVec);
        var angleEscaper = Vector2.Angle(m_xAxis, closestEscaper.GetDirection());


        var fiMunisAngleEscaper = fi - angleEscaper;

        var fiMunisAngleEscaper2 = fi - angleEscaper;
        int counter = 0;
        bool isNegative = false;
        while (Mathf.Abs(fiMunisAngleEscaper) > 90.0f)
        {
            counter++;
            if (fiMunisAngleEscaper < 0.0f)
            {
                fiMunisAngleEscaper += 90.0f;
                isNegative = true;
            }
            else
            {
                fiMunisAngleEscaper -= 90.0f;
            }
        }

        var sinEscaper = Mathf.Sin(fiMunisAngleEscaper);


        var temp = (Mathf.Asin(sinEscaper) * 180.0f) / Mathf.PI;

        var sinPursuer = coef * sinEscaper;

        var asin = Mathf.Asin(sinPursuer);
        var angleOfSineInDegrees = (asin * 180.0f) / Mathf.PI;
        //var sinPursuer = coef * sinEscaper;


        for (int i = 0; i < counter; i++)
        {
            if (isNegative)
            {
                angleOfSineInDegrees -= 90.0f;
            }
            else
            {
                angleOfSineInDegrees += 90.0f;
            }

        }


        var newAngle = fi - coef * fiMunisAngleEscaper2;
  


        Vector2 newDir = MathUtil.Rotate(m_xAxis, -newAngle).normalized;

        Debug.Log(newAngle);


        



        //DebugUtil.DrawLine(MathUtil.Vec2ToVec3(currentPos, true), closestEscaper.transform.position, Color.red);

        //Vector2 nextStepDir = (MathUtil.Vec3ToVec2(closestEscaper.transform.position) - currentPos).normalized;
        return newDir;
    }
}
