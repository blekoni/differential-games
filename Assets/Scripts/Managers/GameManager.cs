using System;
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

    public enum GameType
    {
        TypicalGame,
        UntilTime,
        UntilOutOfZone
    }

    public struct GameSettings
    {
        public GameType gameType;
        public int gameTime;
    }

    public static GameManager m_instance;

    List<Player> m_pursuers = new List<Player>();
    List<Player> m_escapers = new List<Player>();
    GameStatus m_gameStatus = GameStatus.NotStarted;
    GameResult m_gameResult;
    GameSettings m_gameSettings;
    public float m_gameTime = 0.0f;

    public GameObject m_explosion;

    private void Awake()
    {
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
        m_gameSettings.gameType = GameType.TypicalGame;
        m_gameSettings.gameTime = 10;
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

        if(m_gameStatus == GameStatus.InProgress)
        {
            m_gameTime += Time.deltaTime;
            IsGameFinished();
        }
    }

    public static GameManager Get()
    {
        return m_instance;
    }

    public void StartGame()
    {
        MouseManager.Get().ClearSelection();
        m_gameTime = 0.0f;
        MakeAllAlive();
        foreach (Escaper escaper in m_escapers)
        {
            if (escaper)
            {
                escaper.OnGameStart();
            }
        }

        foreach (Pursuer pursuer in m_pursuers)
        {
            if (pursuer)
            {
                pursuer.OnGameStart();
            }
        }
        m_gameStatus = GameStatus.InProgress;
    }

    public void StopGame(FinishGame finishGame)
    {
        m_gameResult = CreateGameResult(finishGame);
        m_gameStatus = GameStatus.Ended;
    }

    public void ResetGame()
    {
        MakeAllAlive();
        DebugUtil.Clean();
        //SetGameType(GameType.UntilTime);
        m_gameStatus = GameStatus.NotStarted;
        foreach (Escaper escaper in m_escapers)
        {
            if (escaper)
            {
                escaper.Reset();
            }
        }

        foreach (Pursuer pursuer in m_pursuers)
        {
            if (pursuer)
            {
                pursuer.Reset();
            }
        }

        SetGameType(m_gameSettings.gameType);
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

    public GameStatus GetGameStatus()
    {
        return m_gameStatus;
    }

    public float GetCurrentGameTime()
    {
        return m_gameTime;
    }

    public int GetGameTime()
    {
        return m_gameSettings.gameTime;
    }

    private bool IsGameFinished()
    {
        if(m_gameSettings.gameType == GameType.UntilTime)
        {
            TimeSpan time = TimeSpan.FromSeconds(m_gameTime);
            if (time.Seconds >= m_gameSettings.gameTime)
            {
                StopGame(FinishGame.OutOfTime);
                return true;
            }
        }
        else if(m_gameSettings.gameType == GameType.UntilOutOfZone)
        {
            var gridBounds = GridManager.Get().GetPickedBounds();
            if(!gridBounds.HasValue)
            {
                StopGame(FinishGame.EscaperOutOfZone);
                return false;
            }

            foreach (Escaper escaper in m_escapers)
            {
                if (escaper)
                {
                    var escaperPos = escaper.GetPosition();
                    if (!gridBounds.Value.Contains(MathUtil.Vec2ToVec3(escaperPos)))
                    {
                        return false;
                    }
                }
            }

            StopGame(FinishGame.EscaperOutOfZone);
            return true;
        }


        return false;
    }

    public void SetGameType(GameType gameType)
    {
        m_gameSettings.gameType = gameType;
        if (gameType == GameType.TypicalGame)
        {
            GridManager.Get().SetDefaultColorToGrid();
            DebugUtil.Clean();
        }
        else if (gameType == GameType.UntilTime)
        {
            GridManager.Get().SetDefaultColorToGrid();
            DebugUtil.Clean();
        }
        else 
        {
            foreach(var escaper in m_escapers)
            {
                if (escaper)
                {
                    escaper.SetBehavior(Behavior.BehaviorType.EscapeFromArea);
                }    
            }
            GridManager.Get().SetActiveColorToGrid();
        }
    }

    public void SetUntilTimeGameType(int gameTimeInSeconds)
    {
        SetGameType(GameType.UntilTime);
        m_gameSettings.gameTime = gameTimeInSeconds;
    }

    public GameType GetGameType()
    {
        return m_gameSettings.gameType;
    }
}
