using Core.Extensions;
using Core.Input;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace Bremsengine
{
    #region Key Values
    public partial class GeneralManager
    {
        public struct Keys
        {
            public const string PlayerBombs = "Player Bombs";
            public const string PlayerLives = "Player Lives";
        }
        static Dictionary<string, dynamic> gameValues;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializeGameValues()
        {
            gameValues = new();
        }
        [QFSW.QC.Command("Start-Replay")]
        public static void StartReplay()
        {
            Application.OpenURL("https://obsproject.com/");
        }
        public static void StoreGameValue(string key, dynamic data)
        {
            gameValues[key] = data;
        }
        public static bool TryFetchGameValue<T>(string key, out T output)
        {
            object value;
            output = default(T);
            if (gameValues.TryGetValue(key, out value))
            {
                output = (T)value;
            }
            return output != null;
        }
    }
    #endregion
    #region Load Scene
    public partial class GeneralManager
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReinitializeSceneLoader()
        {
            IsLoadingScene = false;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void InitializeStageActions()
        {
            StageActions = new();
        }
        public static void SetStageLoadAction(string key, StageExitAction action)
        {
            StageActions[key] = action;
        }
        [SerializeField] GameObject loadingScreen;
        [SerializeField] TMP_Text loadingScreenText;
        public static bool IsLoadingScene;
        public static string CurrentSceneName => SceneManager.GetActiveScene().name;
        public delegate void StageExitAction();
        static Dictionary<string, StageExitAction> StageActions;
        [QFSW.QC.Command("-loadscene")]
        public static void LoadSceneAfterDelay(string sceneName, float delay, System.Action callback = null)
        {
            IEnumerator CO_LoadScene(string sceneName, float delay)
            {
                void SetLoadingProgress(float progress)
                {
                    if (Instance.loadingScreenText != null)
                    {
                        Instance.loadingScreenText.text = "Loading: " + (progress * 100f).Clamp(0f, 100f).ToString("F0") + "%";
                    }
                }
                float storedTimescale = Time.timeScale;
                IsLoadingScene = true;
                yield return new WaitForSecondsRealtime(delay);
                Time.timeScale = 0f;
                StageExitAction toRun = null;
                foreach (var item in StageActions)
                {
                    toRun += item.Value;
                }
                toRun?.Invoke();
                if (Instance != null && Instance.loadingScreen != null)
                {
                    Instance.loadingScreenText.text = "Loading: 0%";
                    Instance.loadingScreen.SetActive(true);
                    yield return new WaitForSecondsRealtime(0.15f);
                    AsyncOperation o = SceneManager.LoadSceneAsync(sceneName);
                    float progress = 0f;
                    while (o != null && !o.isDone)
                    {
                        progress = progress.Lerp(o.progress / 0.9f, Time.deltaTime * 3f).Clamp(0f, 1f);
                        SetLoadingProgress(progress);
                        yield return null;
                    }
                    SetLoadingProgress(1f);
                    yield return new WaitForSecondsRealtime(0.45f);
                    callback?.Invoke();
                    Instance.loadingScreen.SetActive(false);
                    Time.timeScale = 1f;
                }
                else
                {
                    yield return new WaitForSecondsRealtime(delay);
                    SceneManager.LoadScene(sceneName);
                }
                Time.timeScale = storedTimescale;
                IsLoadingScene = false;
            }
            if (!IsLoadingScene)
            {
                Instance.StartCoroutine(CO_LoadScene(sceneName, delay));
            }
        }
    }
    #endregion
    #region Difficulty
    public partial class GeneralManager
    {
        #region Colors & Names
        public static string GetDifficultyName(Difficulty d)
        {
            string difficultyName = "";
            switch (d)
            {
                case Difficulty.Easy: difficultyName += "Easy"; break;
                case Difficulty.Normal: difficultyName += "Normal"; break;
                case Difficulty.Hard: difficultyName += "Hard"; break;
                case Difficulty.Lunatic: difficultyName += "Lunatic"; break;
                case Difficulty.Ultra: difficultyName += "Ultra"; break;
                case Difficulty.Extra: difficultyName += "Extra"; break;
                default: difficultyName = "Normal"; SetDifficulty(Difficulty.Normal); break;
            }
            return difficultyName;
        }
        public static Color32 GetDifficultyColor(Difficulty d)
        {
            switch (d)
            {
                case Difficulty.Easy: return ColorHelper.PastelGreen;
                case Difficulty.Normal: return ColorHelper.PastelBlue;
                case Difficulty.Hard: return ColorHelper.PastelYellow;
                case Difficulty.Lunatic: return ColorHelper.PastelPurple;
                case Difficulty.Ultra: return ColorHelper.PastelOrange;
                case Difficulty.Extra: return ColorHelper.PastelRed;
                default: return Color.blue;
            }
        }
        #endregion
        public static bool ChurroHardmode => CurrentDifficulty == Difficulty.Ultra;
        public static Difficulty CurrentDifficulty { get; private set; } = Difficulty.Ultra;
        public enum Difficulty
        {
            Easy = 1,
            Normal = 2,
            Hard = 3,
            Lunatic = 4,
            Ultra = 5,
            Extra = 6
        }
        public delegate void DifficultyChange(Difficulty d);
        public static event DifficultyChange OnDifficultyChanged;
        [QFSW.QC.Command("difficulty")]
        public static void SetDifficulty(Difficulty d)
        {
            CurrentDifficulty = d;
            OnDifficultyChanged?.Invoke(d);
        }
        public static bool IsDifficultyGreaterOrThanEqualTo(Difficulty d)
        {
            int selection = (int)d;
            return (int)CurrentDifficulty >= selection;
        }
    }
    #endregion
    #region Funny Explosion
    public partial class GeneralManager
    {
        [SerializeField] GameObject funnyExplosion;
        [SerializeField] ACWrapper funnyExplosionSound;
        public static void FunnyExplosion(Vector2 position, float scale = 1f)
        {
            GameObject x = Instantiate(Instance.funnyExplosion, position, Quaternion.identity);
            Destroy(x, 1.02f);
            x.transform.localScale *= scale;
            x.transform.localScale = new(x.transform.localScale.x.Max(0.25f), x.transform.localScale.y.Max(0.25f), 1);
            Instance.funnyExplosionSound.Play(position);
        }
    }
    #endregion
    #region Pause
    public partial class GeneralManager
    {
        public static bool IsPaused { get; private set; }
        private static float StoredPausedTimescale = 1f;
        public static void SetPause(bool state)
        {
            IsPaused = state;
            if (state)
            {
                //Pause
                StoredPausedTimescale = Time.timeScale;
                Time.timeScale = 0f;
                IsPaused = true;
                Debug.Log("Paused Game");
                PlayerInputController.actions.Player.Disable();
                PlayerInputController.actions.Shmup.Disable();
            }
            else
            {
                //Unpause
                Time.timeScale = StoredPausedTimescale;
                IsPaused = false;
                Debug.Log("Un-paused Game");
                PlayerInputController.actions.Player.Enable();
                PlayerInputController.actions.Shmup.Enable();
            }
        }
        [QFSW.QC.Command("-Pause")]
        public static void PauseGame()
        {
            SetPause(true);
        }
        [QFSW.QC.Command("-Unpause")]
        public static void UnPauseGame()
        {
            SetPause(false);
        }
        public static void TogglePause()
        {
            SetPause(!IsPaused);
        }
        [QFSW.QC.Command("-timescale")]
        public static void Command_SetTimescale(float timescale)
        {
            StoredPausedTimescale = timescale;
            UnPauseGame();
        }
    }
    #endregion
    #region Score
    public partial class GeneralManager
    {
        private static float HiddenScoreValidationSum = 0;
        private static float ScoreValidationMultiplier;
        public static bool ShouldAddScoreKey => ScoreBreakdownAnalysis != null;
        public static void AddScoreAnalysisKey(string scoreKey, float score)
        {
            if (!ShouldAddScoreKey)
                return;
            if (!ScoreBreakdownAnalysis.ContainsKey(scoreKey))
                ScoreBreakdownAnalysis[scoreKey] = 0;
            ScoreBreakdownAnalysis[scoreKey] += score;
        }
        public static bool IsScoreLegit()
        {
            float scoreAccuracy = HiddenScoreValidationSum / ScoreValidationMultiplier;
            if (Mathf.Abs(scoreAccuracy - actualScore) < (actualScore * 0.05f))
            {
                return true;
            }
            return false;
        }
        public static float SumUpScoreAnalysis()
        {
            float sum = 0f;
            foreach (var item in ScoreBreakdownAnalysis)
            {
                sum += item.Value;
            }
            return sum;
        }
        private static Dictionary<string, float> ScoreBreakdownAnalysis;
        [SerializeField] bool breakDownScore = false;
        public static float actualScore;
        public static float HighestScore { get; private set; }
        public static float VisibleScore;
        [SerializeField] float visibleScoreDivisor = 0.01f;
        [SerializeField] float visibleScoreMultiplier = 100f;
        static string HighScoreStringKey = "HiScore_Save";
        static bool IsHighscorePotentiallyOutOfSync = true;
        public delegate void ScoreAction(float score, float hiscore);
        public static ScoreAction OnScoreUpdate;
        public static float LoadHighScore()
        {
            ResyncHighscore();
            return HighestScore;
        }
        public static float ResetScore()
        {
            SetScoreValue(0f);
            ScoreBreakdownAnalysis.Clear();
            HiddenScoreValidationSum = 0;
            return VisibleScore;
        }
        private static void SendUpdateScoreEvent(float scoreValue, float highScoreValue)
        {
            OnScoreUpdate?.Invoke(scoreValue, highScoreValue);
        }
        public static float AddScore(float value)
        {
            SetScoreValue(actualScore + value);
            HiddenScoreValidationSum += value * ScoreValidationMultiplier;
            return value;
        }
        public static void ApplyHighscoreToSave(float value)
        {
            PlayerPrefs.SetFloat(HighScoreStringKey, value);
        }
        static void ResyncHighscore()
        {
            if (!IsHighscorePotentiallyOutOfSync)
                return;

            IsHighscorePotentiallyOutOfSync = false;
            float loadedScore = PlayerPrefs.GetFloat(HighScoreStringKey);

            if (HighestScore < loadedScore) HighestScore = loadedScore;
        }
        private static void SetScoreValue(float value)
        {
            ResyncHighscore();
            actualScore = value;
            VisibleScore = (value.Multiply(Instance.visibleScoreDivisor)).Floor().Multiply(Instance.visibleScoreMultiplier);

            if (actualScore > HighestScore)
            {
                HighestScore = VisibleScore;
            }
            SendUpdateScoreEvent(VisibleScore, HighestScore);
        }
        private void OnApplicationQuit()
        {
            ApplyHighscoreToSave(LoadHighScore());
            if (ScoreBreakdownAnalysis != null)
            {
                foreach (var item in ScoreBreakdownAnalysis)
                {
                    string scoreMessage = "Score Breakdown##".ReplaceLineBreaks("##");
                    scoreMessage += $"Score Partition({item.Key}) : {item.Value.ToString("F0")}##".ReplaceLineBreaks("##");
                    Debug.Log(scoreMessage);
                }
            }
        }
    }
    #endregion
    [DefaultExecutionOrder(-10)]
    public partial class GeneralManager : MonoBehaviour
    {
        public static GeneralManager Instance { get; private set; }
        private void Awake()
        {
            StartInstance();
        }
        private void OnDestroy()
        {
            if (Instance == this)
            {
                CloseInstance();
            }
        }
        private void Start()
        {
            if (Instance == this)
            {
                IsHighscorePotentiallyOutOfSync = true;
                SetScoreValue(0f);
                ScoreValidationMultiplier = Random.Range(1f, 10f);
                InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            }
        }
        private void StartInstance()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
            if (breakDownScore)
            {
                ScoreBreakdownAnalysis = new();
            }
        }
        private void CloseInstance()
        {
            if (Instance != this)
                return;
            Instance = null;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ReInitialize()
        {
            Instance = null;
            ScoreBreakdownAnalysis = null;
        }
    }
}
