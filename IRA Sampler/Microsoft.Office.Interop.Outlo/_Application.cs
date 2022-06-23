using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[TypeIdentifier]
[Guid("00063001-0000-0000-C000-000000000046")]
[CompilerGenerated]
public interface _Application
{
	void _VtblGap1_9();

	[DispId(266)]
	[return: MarshalAs(UnmanagedType.IDispatch)]
	object CreateItem([In] OlItemType ItemType);
}
