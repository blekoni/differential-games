using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


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
        EscapersDestroyed,
        EscapersInSafeZone,
        OutOfTime,
        StoppedByUser
    }

    public struct GameResult
    {
        public FinishGame result;
        public float distCompByPursuer;
        public float distCompByEscaper;
    }

    public enum GameType
    {
        DefaultGame,
        UntilTime,
        EscapeToSafeZone
    }

    public struct GameSettings
    {
        public GameType gameType;
        public int gameTimeBoundaries;
        public Behavior.BehaviorType escaperBehavior;
    }

    public static GameManager m_instance;

    List<Player> m_pursuers = new List<Player>();
    List<Player> m_escapers = new List<Player>();
    GameStatus m_gameStatus = GameStatus.NotStarted;
    GameResult m_gameResult;
    GameSettings m_gameSettings;
    public float m_activeGameDuration = 0.0f;

    public GameObject m_explosion;

    // GameManager is owner of all other managers
    [SerializeField] private UIManager m_UIManager;
    [SerializeField] private MouseManager m_mouseManager;
    [SerializeField] private CameraManager m_cameraManager;
    [SerializeField] private GridManager m_gridManager;
    [SerializeField] private LineRendererManager m_lineRendererManager;


    private void Awake()
    {
        Debug.Assert(m_UIManager);
        Debug.Assert(m_mouseManager);
        Debug.Assert(m_cameraManager);
        Debug.Assert(m_gridManager);
        Debug.Assert(m_lineRendererManager);

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
        m_gameSettings.gameType = GameType.DefaultGame;
        m_gameSettings.gameTimeBoundaries = 10;
        m_gameSettings.escaperBehavior = Behavior.BehaviorType.EscapeFromClosestPursuer;
    }

    private void Update()
    {
        if(m_gameStatus == GameStatus.InProgress && 
            IsGameEnded())
        {
            StopGame(FinishGame.EscapersDestroyed);
        }
        else if(m_gameStatus == GameStatus.Ended)
        {
            //foreach (Escaper escaper in m_escapers)
            //{
            //    if (escaper)
            //    {
            //        escaper.ShowPath(Color.blue);
            //    }
            //}

            //foreach (Pursuer pursuer in m_pursuers)
            //{
            //    if (pursuer)
            //    {
            //        pursuer.ShowPath(Color.red);
            //    }
            //}
        }

        if(m_gameStatus == GameStatus.InProgress)
        {
            m_activeGameDuration += Time.deltaTime;
            m_UIManager.UpdateTimer(m_activeGameDuration);
            IsGameFinished();
        }
    }

    public static GameManager Get()
    {
        return m_instance;
    }

    public void StartGame()
    {
        if(m_gameSettings.gameType == GameType.EscapeToSafeZone
            && !m_gridManager.IsAnyPickedTiles())
        {
            EditorUtility.DisplayDialog("Warning"
                                       ,"At least one cell should be picked for the current game type!"
                                       ,"OK");

            m_UIManager.SetStartButtonActive(false);
            return;
        }

        m_UIManager.HideGameResult();
        m_mouseManager.ClearSelection();
        m_activeGameDuration = 0.0f;
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
        m_UIManager.ShowGameResult(m_gameResult);
        m_gameStatus = GameStatus.Ended;
        foreach (Escaper escaper in m_escapers)
        {
            if (escaper)
            {
                escaper.ShowPath(m_lineRendererManager, Color.blue);
            }
        }

        foreach (Pursuer pursuer in m_pursuers)
        {
            if (pursuer)
            {
                pursuer.ShowPath(m_lineRendererManager, Color.red);
            }
        }
    }

    public void ResetGame()
    {
        MakeAllAlive();
        m_lineRendererManager.Clear();
        m_UIManager.ResetUI();

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
        gameRes.distCompByPursuer = m_pursuers[0].GetTravelDistance();
        gameRes.distCompByEscaper = m_escapers[0].GetTravelDistance();
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
        return m_activeGameDuration;
    }

    public int GetGameTime()
    {
        return m_gameSettings.gameTimeBoundaries;
    }

    private bool IsGameFinished()
    {
        if (m_gameSettings.gameType == GameType.UntilTime)
        {
            TimeSpan time = TimeSpan.FromSeconds(m_activeGameDuration);
            if (time.Seconds >= m_gameSettings.gameTimeBoundaries)
            {
                StopGame(FinishGame.OutOfTime);
                return true;
            }
        }
        else if(m_gameSettings.gameType == GameType.EscapeToSafeZone)
        {
            var gridBounds = m_gridManager.GetPickedBounds();
            if(!gridBounds.HasValue)
            {
                StopGame(FinishGame.EscapersInSafeZone);
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

            StopGame(FinishGame.EscapersInSafeZone);
            return true;
        }


        return false;
    }

    public void SetGameType(GameType gameType)
    {
        m_gameSettings.gameType = gameType;
        if (gameType == GameType.DefaultGame)
        {
            SetAllEscapersBeahvior(m_gameSettings.escaperBehavior);
            m_gridManager.SetDefaultColorToGrid();
            m_UIManager.SetStartButtonActive(true);
            m_lineRendererManager.Clear();
        }
        else if (gameType == GameType.UntilTime)
        {
            SetAllEscapersBeahvior(m_gameSettings.escaperBehavior);
            m_gridManager.SetDefaultColorToGrid();
            m_UIManager.SetStartButtonActive(true);
            m_lineRendererManager.Clear();
        }
        else 
        {
            foreach(var escaper in m_escapers)
            {
                if (escaper)
                {
                    var oldBehaviorType = escaper.GetBehaviorType();
                    if (oldBehaviorType != Behavior.BehaviorType.EscapeToSafeZone)
                    {
                        m_gameSettings.escaperBehavior = oldBehaviorType;
                    }
                    escaper.SetBehavior(Behavior.BehaviorType.EscapeToSafeZone);
                }    
            }
            //m_UIManager.SetStartButtonActive(m_gridManager.IsAnyPickedTiles() || m_gameStatus != GameStatus.NotStarted);
            m_gridManager.SetActiveColorToGrid();
        }

        m_UIManager.RefreshUI();
    }

    public void SetUntilTimeGameType(int gameTimeInSeconds)
    {
        SetGameType(GameType.UntilTime);
        m_gameSettings.gameTimeBoundaries = gameTimeInSeconds;
    }

    public GameType GetGameType()
    {
        return m_gameSettings.gameType;
    }

    public bool IsAnyPickedTiles()
    {
        return m_gridManager.IsAnyPickedTiles();
    }

    public Bounds? GetPickedBounds()
    {
        return m_gridManager.GetPickedBounds();
    }

    public UIManager GetUIManager()
    {
        return m_UIManager;
    }

    private void SetAllEscapersBeahvior(Behavior.BehaviorType type)
    {
        foreach (var escaper in m_escapers)
        {
            if (escaper)
            {
                escaper.SetBehavior(type);
            }
        }
    }
}
