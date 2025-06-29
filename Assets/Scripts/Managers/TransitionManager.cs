using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Manages screen transitions between UI screens using CanvasGroup and DOTween.
/// Supports different transition types: Instant, Fade, Slide, Scale.
/// </summary>
namespace TicTacToe
{
    public class TransitionManager : MonoBehaviour
    {
        #region Singleton

        private static TransitionManager _instance;
        public static TransitionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TransitionManager>();
                    
                    if (_instance == null)
                    {
                        // Auto-initialization for quick testing
                        GameObject transitionManagerObject = new GameObject("TransitionManager");
                        _instance = transitionManagerObject.AddComponent<TransitionManager>();
                        Debug.Log("TransitionManager auto-created for quick testing");
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Serialized Fields

        [Header("Transition Settings")]
        [SerializeField] private float _defaultTransitionDuration = 0.5f;
        [SerializeField] private Ease _defaultEaseType = Ease.InOutQuad;

        [Header("Slide Transition Settings")]
        [SerializeField] private Vector2 _slideOffset = new Vector2(100f, 0f);

        [Header("Scale Transition Settings")]
        [SerializeField] private Vector3 _scaleStartValue = Vector3.zero;
        [SerializeField] private Vector3 _scaleEndValue = Vector3.one;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            // Singleton pattern implementation
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Debug.LogWarning("Duplicate TransitionManager found. Destroying duplicate.");
                Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Transitions a UI screen using the specified transition type
        /// </summary>
        /// <param name="canvasGroup">The CanvasGroup to transition</param>
        /// <param name="transitionType">Type of transition to perform</param>
        /// <param name="show">Whether to show (true) or hide (false) the screen</param>
        /// <param name="duration">Duration of the transition (uses default if not specified)</param>
        /// <param name="easeType">Ease type for the transition (uses default if not specified)</param>
        /// <returns>Coroutine for the transition</returns>
        public Coroutine TransitionScreen(CanvasGroup canvasGroup, TransitionType transitionType, bool show, 
            float? duration = null, Ease? easeType = null)
        {
            if (canvasGroup == null)
            {
                Debug.LogError("CanvasGroup is null for transition");
                return null;
            }

            float transitionDuration = duration ?? _defaultTransitionDuration;
            Ease ease = easeType ?? _defaultEaseType;

            switch (transitionType)
            {
                case TransitionType.Instant:
                    return StartCoroutine(InstantTransition(canvasGroup, show));
                case TransitionType.Fade:
                    return StartCoroutine(FadeTransition(canvasGroup, show, transitionDuration, ease));
                case TransitionType.Slide:
                    return StartCoroutine(SlideTransition(canvasGroup, show, transitionDuration, ease));
                case TransitionType.Scale:
                    return StartCoroutine(ScaleTransition(canvasGroup, show, transitionDuration, ease));
                default:
                    Debug.LogWarning($"Unknown transition type: {transitionType}. Using instant transition.");
                    return StartCoroutine(InstantTransition(canvasGroup, show));
            }
        }

        /// <summary>
        /// Transitions between two screens (hides one, shows the other)
        /// </summary>
        /// <param name="hideScreen">Screen to hide</param>
        /// <param name="showScreen">Screen to show</param>
        /// <param name="transitionType">Type of transition</param>
        /// <param name="duration">Duration of the transition</param>
        /// <param name="easeType">Ease type for the transition</param>
        /// <returns>Coroutine for the transition</returns>
        public Coroutine TransitionBetweenScreens(CanvasGroup hideScreen, CanvasGroup showScreen, 
            TransitionType transitionType, float? duration = null, Ease? easeType = null)
        {
            return StartCoroutine(TransitionBetweenScreensCoroutine(hideScreen, showScreen, transitionType, duration, easeType));
        }

        #endregion

        #region Private Transition Methods

        /// <summary>
        /// Instant transition - immediately shows or hides the screen
        /// </summary>
        private IEnumerator InstantTransition(CanvasGroup canvasGroup, bool show)
        {
            SetCanvasGroupState(canvasGroup, show);
            yield break;
        }

        /// <summary>
        /// Fade transition - fades the screen in or out using DOTween
        /// </summary>
        private IEnumerator FadeTransition(CanvasGroup canvasGroup, bool show, float duration, Ease easeType)
        {
            // Kill any existing tweens on this CanvasGroup
            canvasGroup.DOKill();

            // Set initial state
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;

            // Create fade tween
            Tween fadeTween = canvasGroup.DOFade(show ? 1f : 0f, duration)
                .SetEase(easeType)
                .SetUpdate(true); // Update even when time is scaled

            // Wait for tween to complete
            yield return fadeTween.WaitForCompletion();

            // Ensure final state is set
            SetCanvasGroupState(canvasGroup, show);
        }

        /// <summary>
        /// Slide transition - slides the screen in or out using DOTween
        /// </summary>
        private IEnumerator SlideTransition(CanvasGroup canvasGroup, bool show, float duration, Ease easeType)
        {
            RectTransform rectTransform = canvasGroup.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform not found for slide transition");
                yield break;
            }

            // Kill any existing tweens on this RectTransform
            rectTransform.DOKill();
            canvasGroup.DOKill();

            // Set initial state
            canvasGroup.alpha = show ? 0f : 1f;
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;

            // Create sequence for simultaneous position and alpha animation
            Sequence slideSequence = DOTween.Sequence();

            // Add position tween
            Vector2 targetPosition = show ? Vector2.zero : _slideOffset;
            slideSequence.Join(rectTransform.DOAnchorPos(targetPosition, duration)
                .SetEase(easeType)
                .SetUpdate(true));

            // Add alpha tween
            slideSequence.Join(canvasGroup.DOFade(show ? 1f : 0f, duration)
                .SetEase(easeType)
                .SetUpdate(true));

            // Wait for sequence to complete
            yield return slideSequence.WaitForCompletion();

            // Ensure final state is set
            SetCanvasGroupState(canvasGroup, show);
            rectTransform.anchoredPosition = Vector2.zero; // Reset position
        }

        /// <summary>
        /// Scale transition - scales the screen in or out using DOTween
        /// </summary>
        private IEnumerator ScaleTransition(CanvasGroup canvasGroup, bool show, float duration, Ease easeType)
        {
            RectTransform rectTransform = canvasGroup.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogError("RectTransform not found for scale transition");
                yield break;
            }

            // Kill any existing tweens on this RectTransform
            rectTransform.DOKill();
            canvasGroup.DOKill();

            // Set initial state
            canvasGroup.alpha = show ? 0f : 1f;
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;

            // Create sequence for simultaneous scale and alpha animation
            Sequence scaleSequence = DOTween.Sequence();

            // Add scale tween
            Vector3 targetScale = show ? _scaleEndValue : _scaleStartValue;
            scaleSequence.Join(rectTransform.DOScale(targetScale, duration)
                .SetEase(easeType)
                .SetUpdate(true));

            // Add alpha tween
            scaleSequence.Join(canvasGroup.DOFade(show ? 1f : 0f, duration)
                .SetEase(easeType)
                .SetUpdate(true));

            // Wait for sequence to complete
            yield return scaleSequence.WaitForCompletion();

            // Ensure final state is set
            SetCanvasGroupState(canvasGroup, show);
            rectTransform.localScale = _scaleEndValue; // Reset scale
        }

        /// <summary>
        /// Transitions between two screens
        /// </summary>
        private IEnumerator TransitionBetweenScreensCoroutine(CanvasGroup hideScreen, CanvasGroup showScreen, 
            TransitionType transitionType, float? duration, Ease? easeType)
        {
            // Start both transitions simultaneously
            Coroutine hideCoroutine = TransitionScreen(hideScreen, transitionType, false, duration, easeType);
            Coroutine showCoroutine = TransitionScreen(showScreen, transitionType, true, duration, easeType);

            // Wait for both transitions to complete
            if (hideCoroutine != null) yield return hideCoroutine;
            if (showCoroutine != null) yield return showCoroutine;
        }

        /// <summary>
        /// Sets the final state of a CanvasGroup
        /// </summary>
        private void SetCanvasGroupState(CanvasGroup canvasGroup, bool show)
        {
            canvasGroup.alpha = show ? 1f : 0f;
            canvasGroup.interactable = show;
            canvasGroup.blocksRaycasts = show;
        }

        #endregion

        #region Editor Support

#if UNITY_EDITOR
        [ContextMenu("Test Fade Transition")]
        private void TestFadeTransition()
        {
            CanvasGroup testGroup = FindObjectOfType<CanvasGroup>();
            if (testGroup != null)
            {
                StartCoroutine(FadeTransition(testGroup, true, 1f, Ease.InOutQuad));
            }
        }

        [ContextMenu("Test Slide Transition")]
        private void TestSlideTransition()
        {
            CanvasGroup testGroup = FindObjectOfType<CanvasGroup>();
            if (testGroup != null)
            {
                StartCoroutine(SlideTransition(testGroup, true, 1f, Ease.InOutQuad));
            }
        }

        [ContextMenu("Test Scale Transition")]
        private void TestScaleTransition()
        {
            CanvasGroup testGroup = FindObjectOfType<CanvasGroup>();
            if (testGroup != null)
            {
                StartCoroutine(ScaleTransition(testGroup, true, 1f, Ease.InOutQuad));
            }
        }
#endif

        #endregion
    }
} 