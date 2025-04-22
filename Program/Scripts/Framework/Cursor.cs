using OpenTK.Windowing.Common;

namespace Fantasma.Framework
{
    public class Cursor
    {
        private static bool _locked = false;
        public static bool Locked
        {
            get
            {
                return _locked;
            }
            set
            {
                if (value)
                    Core.m_window.CursorState = CursorState.Grabbed;
                else
                    Core.m_window.CursorState = CursorState.Normal;
            }
        }
    }
}
