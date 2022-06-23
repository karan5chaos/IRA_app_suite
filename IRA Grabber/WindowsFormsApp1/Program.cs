using System;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		string embeddedResource = "WindowsFormsApp1.Resources.Resolve.HotKeys.dll";
		EmbeddedAssembly.Load(embeddedResource, "Resolve.HotKeys.dll");
		string embeddedResource2 = "WindowsFormsApp1.Resources.AForge.dll";
		EmbeddedAssembly.Load(embeddedResource2, "AForge.dll");
		string embeddedResource3 = "WindowsFormsApp1.Resources.AForge.Imaging.dll";
		EmbeddedAssembly.Load(embeddedResource3, "AForge.Imaging.dll");
		string embeddedResource4 = "WindowsFormsApp1.Resources.AForge.Math.dll";
		EmbeddedAssembly.Load(embeddedResource4, "AForge.Math.dll");
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		bool createdNew;
		Mutex mutex = new Mutex(initiallyOwned: true, "Capture", out createdNew);
		if (createdNew)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(defaultValue: false);
			Application.Run(new Main_Form());
		}
	}

	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		return EmbeddedAssembly.Get(args.Name);
	}
}
