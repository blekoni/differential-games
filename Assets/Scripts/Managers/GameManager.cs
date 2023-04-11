using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum PlayerRole
    {
        Puriuer,
        Escaper
    }

    public enum GameStatus
    {
        NotStarted,
        InProgress,
        Ended
    }

    public enum FinishGame
    {
        AllEscapersDestroyed,
        EscaperOutOfZone,
        OutOfTime,
        StoppedByUser
    }

    public struct GameResult
    {
        public FinishGame result;
        public float distanceCompByPursuer;
        public float distanceCompByEscaper;
    }

    public static GameManager m_instance;

    List<Player> m_pursuers = new List<Player>();
    List<Player> m_escapers = new List<Player>();
    GameStatus m_gameStatus = GameStatus.NotStarted;
    GameResult m_gameResult;

    [SerializeField] List<Vector2> m_gameAreaPolygon;

    public GameObject m_explosion;

    private void Awake()
    {
        DebugUtil.DrawPolyline(m_gameAreaPolygon, Color.yellow, 1000);

        var pursuers = GameObject.FindGameObjectsWithTag("Pursuer");
        foreach (var pursuer in pursuers)
        {
            if (!pursuer)
            {
                continue;
            }

            var component = pursuer.GetComponent("Pursuer") as Pursuer;
            if (component)
            {
                m_pursuers.Add(component);
            }
        }

        var escapers = GameObject.FindGameObjectsWithTag("Escaper");
        foreach (var escaper in escapers)
        {
            if (!escaper)
            {
                continue;
            }

            var component = escaper.GetComponent("Escaper") as Escaper;
            if (component)
            {
                m_escapers.Add(component);
            }
        }

        m_instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if(m_gameStatus == GameStatus.InProgress && 
            IsGameEnded())
        {
            StopGame(FinishGame.AllEscapersDestroyed);
        }
        else if(m_gameStatus == GameStatus.Ended)
        {
            foreach (Escaper escaper in m_escapers)
            {
                if (escaper)
                {
                    escaper.ShowPath(Color.blue);
                }
            }

            foreach (Pursuer pursuer in m_pursuers)
            {
                if (pursuer)
                {
                    pursuer.ShowPath(Color.red);
                }
            }
        }
    }

    public static GameManager Get()
    {
        return m_instance;
    }

    public void StartGame()
    {
        MakeAllAlive();
        m_gameStatus = GameStatus.InProgress;
    }

    public void StopGame(FinishGame finishGame)
    {
        //MakeAllAlive();
        m_gameResult = CreateGameResult(finishGame);
        m_gameStatus = GameStatus.Ended;
    }

    private GameResult CreateGameResult(FinishGame finishGame)
    {
        GameResult gameRes = new GameResult();
        gameRes.result = finishGame;
        gameRes.distanceCompByPursuer = m_pursuers[0].GetTravelDistance();
        gameRes.distanceCompByEscaper = m_escapers[0].GetTravelDistance();
        return gameRes;
    }


    public GameResult GetGameResult()
    {
        return m_gameResult;
    }

    public void MakeAllAlive()
    {
        foreach (Escaper escaper in m_escapers)
        {
            if (escaper)
            {
                escaper.SetIsAlive(true);
            }
        }

        foreach (Pursuer pursuer in m_pursuers)
        {
            if (pursuer)
            {
                pursuer.SetIsAlive(true);
            }
        }
    }

    public List<Player> GetPusuers()
    {
        return m_pursuers;
    }

    public List<Player> GetEscapers()
    {
        return m_escapers;
    }

    public bool IsGameEnded()
    {
        foreach(Escaper escaper in m_escapers)
        {
            if(escaper)
            {
               if(escaper.IsAlive())
                {
                    return false;
                }
            }
        }

        return true;
    }

    public void Explode(Vector3 explodePosition)
    {
        GameObject explosion = GameObject.Instantiate(m_explosion);
        explosion.transform.position = explodePosition;
    }

    public List<Vector2>GetGameAreaPolygon()
    {
        return m_gameAreaPolygon;
    }

    public GameStatus GetGameStatus()
    {
        return m_gameStatus;
    }
}
