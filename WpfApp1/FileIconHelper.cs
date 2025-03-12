using System;
using System.Drawing;       // Add a reference to System.Drawing (for Icon)
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public static class FileIconHelper
{
    // Constants for SHGetFileInfo
    private const uint SHGFI_ICON = 0x100;
    private const uint SHGFI_LARGEICON = 0x0;   // Large icon
    private const uint SHGFI_SMALLICON = 0x1;   // Small icon

    [StructLayout(LayoutKind.Sequential)]
    private struct SHFILEINFO
    {
        public IntPtr hIcon;
        public int iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    }

    [DllImport("Shell32.dll")]
    private static extern IntPtr SHGetFileInfo(
        string pszPath,
        uint dwFileAttributes,
        ref SHFILEINFO psfi,
        uint cbFileInfo,
        uint uFlags
    );

    /// <summary>
    /// Gets the default system icon for the specified file.
    /// </summary>
    /// <param name="filePath">Full path to the file.</param>
    /// <param name="smallIcon">True to retrieve the small icon, false for large icon.</param>
    /// <returns>ImageSource of the system icon, or null if it fails.</returns>
    public static ImageSource? GetFileIcon(string filePath, bool smallIcon = true)
    {
        SHFILEINFO shfi = new SHFILEINFO();
        uint flags = SHGFI_ICON | (smallIcon ? SHGFI_SMALLICON : SHGFI_LARGEICON);

        // Call SHGetFileInfo to get the icon handle
        IntPtr hIcon = SHGetFileInfo(filePath, 0, ref shfi, (uint)Marshal.SizeOf(shfi), flags);

        if (hIcon == IntPtr.Zero)
        {
            // Failed to get icon
            return null;
        }

        // Create the Icon object from the handle
        Icon icon = Icon.FromHandle(shfi.hIcon);

        // Convert to WPF ImageSource
        ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
            icon.Handle,
            new Int32Rect(0, 0, icon.Width, icon.Height),
            BitmapSizeOptions.FromEmptyOptions()
        );

        // Be sure to dispose the icon to avoid memory leaks
        // (but be careful not to destroy the handle before CreateBitmapSourceFromHIcon is done)
        icon.Dispose();

        return imageSource;
    }
}
