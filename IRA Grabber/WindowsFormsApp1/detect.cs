using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1;

public static class detect
{
	public static void ChangeColour(this Bitmap bmp, byte inColourR, byte inColourG, byte inColourB, byte outColourR, byte outColourG, byte outColourB)
	{
		PixelFormat format = PixelFormat.Format24bppRgb;
		Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
		BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadWrite, format);
		IntPtr scan = bitmapData.Scan0;
		int num = bitmapData.Stride * bmp.Height;
		byte[] array = new byte[num];
		Marshal.Copy(scan, array, 0, num);
		for (int i = 0; i < array.Length; i += 3)
		{
			if (array[i] == inColourR && array[i + 1] == inColourG && array[i + 2] == inColourB)
			{
				array[i] = outColourR;
				array[i + 1] = outColourG;
				array[i + 2] = outColourB;
			}
		}
		Marshal.Copy(array, 0, scan, num);
		bmp.UnlockBits(bitmapData);
	}
}
