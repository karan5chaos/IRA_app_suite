using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[Guid("0006303B-0000-0000-C000-000000000046")]
[DefaultMember("Item")]
[TypeIdentifier]
[CompilerGenerated]
public interface Recipients : IEnumerable
{
	void _VtblGap1_6();

	[DispId(111)]
	[return: MarshalAs(UnmanagedType.Interface)]
	Recipient Add([In][MarshalAs(UnmanagedType.BStr)] string Name);

	void _VtblGap2_1();

	[DispId(126)]
	bool ResolveAll();
}
