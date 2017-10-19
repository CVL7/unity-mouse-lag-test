using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


public sealed class CrispMouse
{
    private static readonly CrispMouse instance = new CrispMouse();
    public static CrispMouse Instance
    {
        get { return instance; }
    }

    private bool _mouseLook;
    public bool mouseLook
    {
        get { return _mouseLook; }
        set
        {
            _mouseLook = value;
            Cursor.visible = !value;
        }
    }
    private UnityWin32.MouseInfo prevMouseInfo;

    public Vector2 position
    {
        get
        {
#if UNITY_EDITOR
            return ScreenToEditorView(UnityWin32.GetCursorPosition());
#else
                var activeWindow = UnityWin32.GetForegroundWindow();
                var clientRect = UnityWin32.GetClientRect(activeWindow);
                var cursorPos = UnityWin32.ScreenToClient(activeWindow, UnityWin32.GetCursorPosition());
                cursorPos.y = clientRect.yMax - cursorPos.y;
                return cursorPos;
#endif
        }

        set
        {
#if UNITY_EDITOR
            UnityWin32.SetCursorPosition(EditorViewToScreen(value));
#else
                var activeWindow = UnityWin32.GetForegroundWindow();
                var cursorPos = UnityWin32.ClientToScreen(activeWindow, new Vector2(value.x, Screen.height - value.y));
                UnityWin32.SetCursorPosition(cursorPos);
#endif
        }
    }

    public Vector2 positionRelative
    {
        get
        {
            var center = new Vector2(Screen.width / 2, Screen.height / 2);
            var mousePos = position;
            position = center;
            var dx = mousePos.x - center.x;
            var dy = mousePos.y - center.y;
            return new Vector2(dx, dy);
        }
    }

    private CrispMouse()
    {
        DisableMouseAcceleration();

#if UNITY_EDITOR
        gameView = EditorWindow.GetWindow(System.Type.GetType("UnityEditor.GameView,UnityEditor"));
#endif
    }

    ~CrispMouse()
    {
        UnityWin32.SetMouseInfo(prevMouseInfo);
    }

    private void DisableMouseAcceleration()
    {
        if (prevMouseInfo.Equals(UnityWin32.MouseInfo.disabledAcceleration))
        {
            prevMouseInfo = UnityWin32.GetMouseInfo();
            UnityWin32.SetMouseInfo(UnityWin32.MouseInfo.disabledAcceleration);
        }
    }

    private bool IsMouseAccelerated()
    {
        return UnityWin32.GetMouseInfo().acceleration == 1;
    }

#if UNITY_EDITOR
    private EditorWindow gameView;

    private Vector2 ScreenToEditorView(Vector2 cursorPos)
    {
        return EditorWindowConversion(cursorPos, true);
    }

    private Vector2 EditorViewToScreen(Vector2 cursorPos)
    {
        return EditorWindowConversion(cursorPos, false);
    }

    private Vector2 EditorWindowConversion(Vector2 cursorPos, bool ScreenToEditor)
    {
        var topMargin = (int)gameView.position.y;
        var leftMargin = (int)gameView.position.x;
        var toolbarHeight = (int)EditorStyles.toolbar.fixedHeight - 1;
        var topPaddingCanvas = Mathf.RoundToInt((gameView.position.height - toolbarHeight - Screen.height) / 2);
        var leftPaddingCanvas = Mathf.RoundToInt((gameView.position.width - Screen.width) / 2);

        if (ScreenToEditor)
        {
            cursorPos.x -= leftMargin + leftPaddingCanvas;
            cursorPos.y -= topMargin + toolbarHeight + topPaddingCanvas;
            cursorPos.y = Screen.height - cursorPos.y;
        }
        else
        {
            cursorPos.x += leftMargin + leftPaddingCanvas;
            cursorPos.y = topMargin + toolbarHeight + topPaddingCanvas + Screen.height - cursorPos.y;
        }
        return cursorPos;
    }
#endif
}
