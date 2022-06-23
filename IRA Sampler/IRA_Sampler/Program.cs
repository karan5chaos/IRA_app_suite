using System;
using System.Reflection;
using System.Windows.Forms;

namespace IRA_Sampler;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		string embeddedResource = "IRA_Sampler.Resc.ClosedXML.dll";
		EmbeddedAssembly.Load(embeddedResource, "ClosedXML.dll");
		string embeddedResource2 = "IRA_Sampler.Resc.DocumentFormat.OpenXml.dll";
		EmbeddedAssembly.Load(embeddedResource2, "DocumentFormat.OpenXml.dll");
		string embeddedResource3 = "IRA_Sampler.Resc.AForge.dll";
		EmbeddedAssembly.Load(embeddedResource3, "AForge.dll");
		string embeddedResource4 = "IRA_Sampler.Resc.AForge.Imaging.dll";
		EmbeddedAssembly.Load(embeddedResource4, "AForge.Imaging.dll");
		string embeddedResource5 = "IRA_Sampler.Resc.AForge.Imaging.Formats.dll";
		EmbeddedAssembly.Load(embeddedResource5, "AForge.Imaging.Formats.dll");
		string embeddedResource6 = "IRA_Sampler.Resc.AForge.Math.dll";
		EmbeddedAssembly.Load(embeddedResource6, "AForge.Math.dll");
		string embeddedResource7 = "IRA_Sampler.Resc.Cyotek.Windows.Forms.ImageBox.dll";
		EmbeddedAssembly.Load(embeddedResource7, "Cyotek.Windows.Forms.ImageBox.dll");
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new Main_Form());
	}

	private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
	{
		return EmbeddedAssembly.Get(args.Name);
	}
}
