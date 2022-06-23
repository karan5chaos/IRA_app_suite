using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[CompilerGenerated]
[TypeIdentifier]
[Guid("0006304B-0000-0000-C000-000000000046")]
public interface AddressEntry
{
	string Type
	{
		[DispId(12290)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[DispId(12290)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	void _VtblGap1_14();

	void _VtblGap2_6();

	[DispId(64241)]
	[return: MarshalAs(UnmanagedType.Interface)]
	ExchangeUser GetExchangeUser();
}
