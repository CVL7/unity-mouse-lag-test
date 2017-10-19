using System;
using System.Runtime.InteropServices;
using UnityEngine;


public class UnityWin32
{
    public static Vector2 GetCursorPosition()
    {
        POINT point;
        GetCursorPos(out point);
        return point;
    }

    public static void SetCursorPosition(Vector2 pos)
    {
        SetCursorPos((int)pos.x, (int)pos.y);
    }

    public static Vector2 ScreenToClient(IntPtr hWnd, Vector2 point)
    {
        POINT p = point;
        ScreenToClient(hWnd, ref p);
        return p;
    }

    public static Vector2 ClientToScreen(IntPtr hWnd, Vector2 point)
    {
        POINT p = point;
        ClientToScreen(hWnd, ref p);
        return p;
    }

    public static Rect GetClientRect(IntPtr hWnd)
    {
        RECT rect;
        GetClientRect(hWnd, out rect);
        return rect;
    }

    public static MouseInfo GetMouseInfo()
    {
        const uint SPI_GETMOUSE = 0x0003;
        MouseInfo mouseInfo;
        SystemParametersInfo(SPI_GETMOUSE, 0, out mouseInfo, 0);
        return mouseInfo;
    }

    public static void SetMouseInfo(MouseInfo mouseInfo)
    {
        const uint SPI_SETMOUSE = 0x0004;
        SystemParametersInfo(SPI_SETMOUSE, 0, out mouseInfo, 0);
    }

    #region structs

    public struct MouseInfo
    {
        public int xThreshold;
        public int yThreshold;
        public int acceleration;

        public MouseInfo(int xThreshold, int yThreshold, int acceleration)
        {
            this.xThreshold = xThreshold;
            this.yThreshold = yThreshold;
            this.acceleration = acceleration;
        }

        public static MouseInfo disabledAcceleration = new MouseInfo(0, 0, 0);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int x;
        public int y;

        public POINT(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2(POINT point)
        {
            return new Vector2(point.x, point.y);
        }

        public static implicit operator POINT(Vector2 point)
        {
            return new POINT((int)point.x, (int)point.y);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int topLeftX;
        public int topLeftY;
        public int bottomRightX;
        public int bottomRightY;

        public static implicit operator Rect(RECT rect)
        {
            return Rect.MinMaxRect(rect.topLeftX, rect.topLeftY, rect.bottomRightX, rect.bottomRightY);
        }
    }

    #endregion

    #region native methods

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool
        SystemParametersInfo(uint uiAction, uint uiParam, out MouseInfo pvParam, uint fWinIni);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(out POINT point);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetCursorPos(int x, int y);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ScreenToClient(IntPtr hWnd, ref POINT point);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ClientToScreen(IntPtr hWnd, ref POINT point);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(IntPtr hWnd, out RECT rect);

    #endregion
}
