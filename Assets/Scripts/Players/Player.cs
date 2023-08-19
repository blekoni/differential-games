using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
struct PlayerSettings
{
    public Vector2 position;
    public float speed;
}

public class Player : MonoBehaviour
{
    [SerializeField] float m_speed = 1.0f;
    [SerializeField] bool  m_isAlive = false;
    [SerializeField] bool  m_isVisible = true;
    [SerializeField] bool  m_shouldAliveOnCollision = false;

    protected Behavior m_behaviorHelper;

    protected Vector3 m_moveDirection = new Vector3(1.0f, 0.0f, 0.0f);

    List<Vector2> m_path = new List<Vector2>();

    PlayerSettings m_prevGameSettings;


    public void Start()
    {
        m_prevGameSettings.position = MathUtil.Vec3ToVec2(transform.position);
        m_prevGameSettings.speed = m_speed;
    }

    // Update is called once per frame
    public void Update()
    {
        if (!IsAlive())
        {
            return;
        }

        if (GameManager.Get().GetGameStatus() == GameManager.GameStatus.InProgress)
        {
            var moveVec2d = m_behaviorHelper.GetNextStepDirection(GetPosition(), GetDirection());
            var moveVec = MathUtil.Vec2ToVec3(moveVec2d.normalized);
            RotateToDirection(moveVec);
            transform.position += moveVec * GetSpeed() * Time.deltaTime;
            AddPath(MathUtil.Vec3ToVec2(transform.position));
        }
        else if (GameManager.Get().GetGameStatus() == GameManager.GameStatus.Ended)
        {
            SetIsVisible(false);
        }
    }

    public bool IsAlive()
    {
        return m_isAlive;
    }

    public void SetIsAlive(bool isAlive)
    {
        m_isAlive = isAlive;
        EnableFireSmoke();
        SetIsVisible(isAlive);
    }

    public bool IsVisible()
    {
        return m_isVisible;
    }

    public void SetIsVisible(bool isVisible)
    {
        m_isVisible = isVisible;

        gameObject.active = isVisible;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<MeshRenderer>())
            {
                child.GetComponent<MeshRenderer>().enabled = isVisible;
            }
        }

        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = isVisible;
        }
    }

    protected bool ShouldAliveOnCollision()
    {
        return m_shouldAliveOnCollision;
    }

    protected void SetShouldAliveOnCollision(bool shouldAliveOnCollision)
    {
        m_shouldAliveOnCollision = shouldAliveOnCollision;
    }

    public void EnableFireSmoke()
    {
        var child = this.transform.Find("fire");
        if(!child)
        {
            return;
        }
        var ps = child.GetComponent<ParticleSystem>();
        if(!ps)
        {
            return;
        }
        ps.startSpeed = 2;
    }

    public void Die()
    {
        GameManager.Get().Explode(this.transform.position);
        SetIsAlive(false);
    }

    public Vector2 GetPosition()
    {
        return MathUtil.Vec3ToVec2(GetPosition3D());
    }

    public Vector3 GetPosition3D()
    {
        return transform.position;
    }

    public Vector2 GetDirection()
    {
        return MathUtil.Vec3ToVec2(GerDirection3D());
    }

    public Vector3 GerDirection3D()
    {
        return m_moveDirection;
    }

    public void SetDirection(Vector3 newDir)
    {
        m_moveDirection = newDir;
    }

    public void RotateToDirection(Vector3 moveVec)
    {
        float angle = Vector3.Angle(m_moveDirection.normalized, moveVec.normalized);
        var crossProduct = Vector3.Cross(m_moveDirection, moveVec).normalized;

        if (angle < 1.0f)
            return;

        float sign = crossProduct.y > 0.0f ? 1.0f : -1.0f;
        transform.Rotate(crossProduct, angle);

        m_moveDirection = moveVec;
    }

    public float GetSpeed()
    {
        return m_speed;
    }

    public void SetSpeed(float speed)
    {
        m_speed = speed;
        m_prevGameSettings.speed = speed;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = MathUtil.Vec2ToVec3(position, transform.position.y);
        m_prevGameSettings.position = position;
    }

    public void AddPath(Vector2 position)
    {
        if(m_path.Count != 0)
        {
            var lastPos = m_path[m_path.Count - 1];
            if(Vector2.Distance(lastPos, position) < 0.1f)
            {
                return;
            }
        }
        m_path.Add(position);
    }

    public void ShowPath(Color color)
    {
        Vector2 prevPos = new Vector2(0.0f, 0.0f);
        bool isFirst = true;
        foreach(var pos in m_path)
        {
            if (isFirst)
            {
                prevPos = pos;
                isFirst = false;
                continue;
            }
            DebugUtil.DrawLine(prevPos, pos, color, 0.1f);
            prevPos = pos;
        }

        m_path.Clear();
    }

    public float GetTravelDistance()
    {
        float distance = 0.0f;
        for (var i = 1; i <= m_path.Count - 1; ++i)
        {
            var p1 = m_path[i - 1];
            var p2 = m_path[i % m_path.Count];

            distance += Vector2.Distance(p1, p2);
        }

        return distance;
    }

    public Behavior GetBehavior()
    {
        return m_behaviorHelper;
    }

    virtual public Behavior.BehaviorType GetBehaviorType()
    {
        return m_behaviorHelper.GetBehaviorType();
    }

    virtual public void SetBehavior(Behavior.BehaviorType behaviorType)
    {
        return;
    }

    public void Reset()
    {
        m_path.Clear();
        m_speed = m_prevGameSettings.speed;
        transform.position = (MathUtil.Vec2ToVec3(m_prevGameSettings.position, transform.position.y));
    }

    public void OnGameStart()
    {
        m_prevGameSettings.position = MathUtil.Vec3ToVec2(transform.position);
        m_prevGameSettings.speed = m_speed;
    }

    public bool IsInGameArea(List<Vector2> polygon)
    {
        return MathUtil.IsPointInPolygon(MathUtil.Vec3ToVec2(transform.position), polygon);
    }
}
