using System.Runtime.InteropServices;

namespace ClipboardMasking.Win.Clipboard
{
    public class ClipboardMonitor : NativeWindow
    {
        public event EventHandler? ClipboardUpdate;

        // Modern clipboard listener API
        private const int WM_CLIPBOARDUPDATE = 0x031D;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        public ClipboardMonitor()
        {
            CreateHandle(new CreateParams());
            // Register this window to receive WM_CLIPBOARDUPDATE messages
            AddClipboardFormatListener(Handle);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_CLIPBOARDUPDATE)
            {
                ClipboardUpdate?.Invoke(this, EventArgs.Empty);
            }

            base.WndProc(ref m);
        }

        public void StopMonitoring()
        {
            try
            {
                RemoveClipboardFormatListener(Handle);
            }
            finally
            {
                if (Handle != IntPtr.Zero)
                {
                    DestroyHandle();
                }
            }
        }
    }
}