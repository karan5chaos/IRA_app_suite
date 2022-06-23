using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Office.Interop.Outlook;

[ComImport]
[Guid("00063034-0000-0000-C000-000000000046")]
[TypeIdentifier]
[CompilerGenerated]
public interface _MailItem
{
	string Body
	{
		[DispId(37120)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[DispId(37120)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	OlImportance Importance
	{
		[DispId(23)]
		get;
		[DispId(23)]
		[param: In]
		set;
	}

	string Subject
	{
		[DispId(55)]
		[return: MarshalAs(UnmanagedType.BStr)]
		get;
		[DispId(55)]
		[param: In]
		[param: MarshalAs(UnmanagedType.BStr)]
		set;
	}

	Recipients Recipients
	{
		[DispId(63508)]
		[return: MarshalAs(UnmanagedType.Interface)]
		get;
	}

	void _VtblGap1_8();

	void _VtblGap2_10();

	void _VtblGap3_14();

	void _VtblGap4_6();

	[DispId(61606)]
	void Display([Optional][In][MarshalAs(UnmanagedType.Struct)] object Modal);

	void _VtblGap5_37();

	void _VtblGap6_32();

	[DispId(61557)]
	void Send();
}
