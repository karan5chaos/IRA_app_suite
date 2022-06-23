using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[Guid("000630C9-0000-0000-C000-000000000046")]
[TypeIdentifier]
[CompilerGenerated]
public interface _ExchangeUser
{
	string Name
	{
		[DispId(12289)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[DispId(12289)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	void _VtblGap1_12();

	void _VtblGap2_44();

	[DispId(64249)]
	[return: MarshalAs(UnmanagedType.Interface)]
	ExchangeUser GetExchangeUserManager();
}
