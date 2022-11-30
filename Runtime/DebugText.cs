// ReSharper disable RedundantUsingDirective
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Local
// ReSharper disable NotAccessedField.Local

using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable 0649
#pragma warning disable 0414

namespace Kogane
{
    /// <summary>
    /// デバッグテキストの UI を管理するクラス
    /// </summary>
    [AddComponentMenu( "" )]
    [DisallowMultipleComponent]
    public sealed class DebugText : MonoBehaviour
    {
        //====================================================================================
        // 定数
        //====================================================================================
        private const string DISABLE_CONDITION_STRING = "ME9tf7tPn2xAqa2vHqafxRtdp6YZWjpj";

        //====================================================================================
        // 変数(SerializeField)
        //====================================================================================
        [SerializeField] private GameObject    m_closeBaseUI;
        [SerializeField] private GameObject    m_openBaseUI;
        [SerializeField] private Button        m_closeButtonUI;
        [SerializeField] private Button        m_openButtonUI;
        [SerializeField] private CanvasGroup   m_canvasGroup;
        [SerializeField] private GameObject    m_root;
        [SerializeField] private RectTransform m_textBaseUI;
        [SerializeField] private Text          m_textUI;
        [SerializeField] private RectTransform m_textRectUI;
        [SerializeField] private Vector2       m_sizeOffset = Vector2.zero;

        //====================================================================================
        // 変数
        //====================================================================================
        private string       m_currentText;
        private Vector2      m_currentTextSize;
        private Func<string> m_getText;
        private float        m_interval;
        private float        m_timer;
        private bool         m_isNeedUpdate;

        //====================================================================================
        // 変数(static)
        //====================================================================================
        private static bool m_isOpen;

        //====================================================================================
        // 関数
        //====================================================================================
        /// <summary>
        /// 初期化される時に呼び出されます
        /// </summary>
        private void Awake()
        {
#if KOGANE_DISABLE_UI_DEBUG_TEXT
            Destroy( gameObject );
#else
            m_closeButtonUI.onClick.AddListener( () => SetState( false ) );
            m_openButtonUI.onClick.AddListener( () => SetState( true ) );
#endif
        }

#if KOGANE_DISABLE_UI_DEBUG_TEXT
#else
        /// <summary>
        /// 開始する時に呼び出されます
        /// </summary>
        private void Start()
        {
            m_root.SetActive( true );
            m_openBaseUI.SetActive( false );
            m_closeBaseUI.SetActive( true );

            SetState( m_isOpen );
        }
#endif

#if KOGANE_DISABLE_UI_DEBUG_TEXT
#else
        /// <summary>
        /// 更新される時に呼び出されます
        /// </summary>
        [Conditional( DISABLE_CONDITION_STRING )]
        private void Update()
        {
            if ( !m_isOpen ) return;
            if ( !m_isNeedUpdate ) return;

            m_timer += Time.unscaledDeltaTime;

            if ( m_timer < m_interval ) return;

            m_timer -= m_interval;

            UpdateText();
            UpdateSize();
        }

#endif

        /// <summary>
        /// ステートを設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        private void SetState( bool isOpen )
        {
            m_isOpen = isOpen;

            m_openBaseUI.SetActive( isOpen );
            m_closeBaseUI.SetActive( !isOpen );

            if ( !isOpen ) return;

            UpdateText();
            UpdateSize();
        }

        /// <summary>
        /// 表示するかどうかを設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        public void SetVisible( bool isVisible )
        {
            var alpha = isVisible ? 1 : 0;
            m_canvasGroup.alpha = alpha;
        }

        /// <summary>
        /// 表示を設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        public void Setup( string text )
        {
            Setup
            (
                interval: 0,
                isNeedUpdate: false,
                getText: () => text
            );
        }

        /// <summary>
        /// 表示を設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        public void Setup( Func<string> getText )
        {
            Setup
            (
                interval: 1,
                isNeedUpdate: true,
                getText: getText
            );
        }

        /// <summary>
        /// 表示を設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        public void SetupEveryFrame( Func<string> getText )
        {
            Setup
            (
                interval: 0,
                isNeedUpdate: true,
                getText: getText
            );
        }

        /// <summary>
        /// 表示を設定します
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
        [Conditional( DISABLE_CONDITION_STRING )]
#endif
        public void Setup
        (
            float        interval,
            bool         isNeedUpdate,
            Func<string> getText
        )
        {
            m_interval     = interval;
            m_isNeedUpdate = isNeedUpdate;
            m_getText      = getText;

            UpdateText();
            UpdateSize();
        }

        /// <summary>
        /// テキストを更新します
        /// </summary>
        private void UpdateText()
        {
            if ( !m_isOpen ) return;

            var text = m_getText?.Invoke() ?? string.Empty;

            if ( m_textUI.text == text ) return;

            m_textUI.text = text;
        }

        /// <summary>
        /// 描画範囲を更新します
        /// </summary>
        private void UpdateSize()
        {
            StartCoroutine( DoUpdateSize() );
        }

        /// <summary>
        /// 描画範囲を更新します
        /// </summary>
        private IEnumerator DoUpdateSize()
        {
            yield return null;

            if ( !m_isOpen ) yield break;

            var textSize = m_textRectUI.sizeDelta;

            if ( textSize == m_currentTextSize ) yield break;

            var textBaseSize = m_textBaseUI.sizeDelta;

            textBaseSize           = textSize + m_sizeOffset;
            m_textBaseUI.sizeDelta = textBaseSize;
            m_currentTextSize      = textSize;
        }

        //====================================================================================
        // 関数(static)
        //====================================================================================
        /// <summary>
        /// ゲーム起動時に呼び出されます
        /// </summary>
#if KOGANE_DISABLE_UI_DEBUG_TEXT
#else
        [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
        private static void RuntimeInitializeOnLoadMethod()
        {
            m_isOpen = false;
        }
#endif
    }
}