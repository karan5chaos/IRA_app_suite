using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;

public static class ImageMerger
{
	public static Bitmap MergeImages(List<string> imageUrls, WebProxy proxy = null)
	{
		List<Bitmap> images = ConvertUrlsToBitmaps(imageUrls, proxy);
		return Merge(images);
	}

	private static List<Bitmap> ConvertUrlsToBitmaps(List<string> imageUrls, WebProxy proxy = null)
	{
		List<Bitmap> list = new List<Bitmap>();
		foreach (string imageUrl in imageUrls)
		{
			try
			{
				WebClient webClient = new WebClient();
				if (proxy != null)
				{
					webClient.Proxy = proxy;
				}
				byte[] buffer = webClient.DownloadData(imageUrl);
				MemoryStream stream = new MemoryStream(buffer);
				Image image = Image.FromStream(stream);
				list.Add((Bitmap)image);
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
		}
		return list;
	}

	public static Bitmap MergeImages(string folderPath, ImageFormat imageFormat)
	{
		List<Bitmap> images = ConvertUrlsToBitmaps(folderPath, imageFormat);
		return Merge(images);
	}

	private static List<Bitmap> ConvertUrlsToBitmaps(string folderPath, ImageFormat imageFormat)
	{
		List<Bitmap> list = new List<Bitmap>();
		List<string> list2 = Directory.GetFiles(folderPath, "*." + imageFormat, SearchOption.AllDirectories).ToList();
		foreach (string item2 in list2)
		{
			try
			{
				Bitmap item = (Bitmap)Image.FromFile(item2);
				list.Add(item);
			}
			catch (Exception ex)
			{
				Console.Write(ex.Message);
			}
		}
		return list;
	}

	public static Bitmap MergeImages(List<Bitmap> bitmaps)
	{
		return Merge(bitmaps);
	}

	private static Bitmap Merge(IEnumerable<Bitmap> images)
	{
		IList<Bitmap> list = (images as IList<Bitmap>) ?? images.ToList();
		int num = 0;
		int num2 = 0;
		foreach (Bitmap item in list)
		{
			num = ((item.Width > num) ? item.Width : num);
			num2 = ((item.Height > num2) ? item.Height : num2);
		}
		Bitmap bitmap = new Bitmap(num, num2);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			foreach (Bitmap item2 in list)
			{
				graphics.DrawImage(item2, 0, 0);
			}
		}
		return bitmap;
	}
}
