using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[CompilerGenerated]
[Guid("00063045-0000-0000-C000-000000000046")]
[TypeIdentifier]
public interface Recipient
{
	AddressEntry AddressEntry
	{
		[DispId(121)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
		[DispId(121)]
		[param: In]
		[param: MarshalAs(UnmanagedType.Interface)]
		set;
	}

	void _VtblGap1_5();
}
