using System.Runtime.InteropServices;

namespace ClipboardMasking.Win.Clipboard
{
    public class ClipboardMonitor : NativeWindow
    {
        public event EventHandler? ClipboardUpdate;

        private const int WM_DRAWCLIPBOARD = 0x308;
        private const int WM_CHANGECBCHAIN = 0x030D;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        private IntPtr _nextClipboardViewer;

        public ClipboardMonitor()
        {
            CreateHandle(new CreateParams());
            _nextClipboardViewer = SetClipboardViewer(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DRAWCLIPBOARD)
            {
                ClipboardUpdate?.Invoke(this, EventArgs.Empty);
                // Pass the message to the next viewer.
                User32.SendMessage(_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
            }
            else if (m.Msg == WM_CHANGECBCHAIN)
            {
                if (m.WParam == _nextClipboardViewer)
                {
                    _nextClipboardViewer = m.LParam;
                }
                else if (_nextClipboardViewer != IntPtr.Zero)
                {
                    User32.SendMessage(_nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                }
            }
        }

        public void StopMonitoring()
        {
            ChangeClipboardChain(Handle, _nextClipboardViewer);
        }
    }

    // Helper class for the SendMessage
    internal static class User32
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}