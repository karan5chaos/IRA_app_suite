using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[Guid("00063002-0000-0000-C000-000000000046")]
[CompilerGenerated]
[TypeIdentifier]
public interface _NameSpace
{
	Recipient CurrentUser
	{
		[DispId(8449)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap1_4();
}
