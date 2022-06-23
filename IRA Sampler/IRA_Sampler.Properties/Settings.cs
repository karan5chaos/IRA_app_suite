using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AForge.Math.Geometry;

namespace IRA_Sampler.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("10")]
	public decimal percent
	{
		get
		{
			return (decimal)this["percent"];
		}
		set
		{
			this["percent"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("")]
	public string s_location
	{
		get
		{
			return (string)this["s_location"];
		}
		set
		{
			this["s_location"] = value;
		}
	}

	[DefaultSettingValue("")]
	[DebuggerNonUserCode]
	[UserScopedSetting]
	public string o_location
	{
		get
		{
			return (string)this["o_location"];
		}
		set
		{
			this["o_location"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>add_user</string>\r\n</ArrayOfString>")]
	[UserScopedSetting]
	public StringCollection user_list
	{
		get
		{
			return (StringCollection)this["user_list"];
		}
		set
		{
			this["user_list"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>Blurry Sign with text</string>\r\n  <string>Blurry Sign with number</string>\r\n  <string>Blurry POT</string>\r\n  <string>HWY Blurry Sign - Readable</string>\r\n  <string>HWY Blurry Sign - Unreadable</string>\r\n  <string>Reclassification - Missed</string>\r\n  <string>Reclassification - Incorrect</string>\r\n  <string>Not a Sign</string>\r\n  <string>Partially Visible Sign</string>\r\n  <string>Incorrectly Rejected</string>\r\n  <string>Incorrectly Undetermined</string>\r\n</ArrayOfString>")]
	public StringCollection err_list
	{
		get
		{
			return (StringCollection)this["err_list"];
		}
		set
		{
			this["err_list"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <string>Error 1</string>\r\n</ArrayOfString>")]
	public StringCollection err_list_2d
	{
		get
		{
			return (StringCollection)this["err_list_2d"];
		}
		set
		{
			this["err_list_2d"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("")]
	public string ls_selected
	{
		get
		{
			return (string)this["ls_selected"];
		}
		set
		{
			this["ls_selected"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("False")]
	public bool enable_ImageDetection
	{
		get
		{
			return (bool)this["enable_ImageDetection"];
		}
		set
		{
			this["enable_ImageDetection"] = value;
		}
	}

	[UserScopedSetting]
	[DefaultSettingValue("Square")]
	[DebuggerNonUserCode]
	public PolygonSubType shape_type
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			return (PolygonSubType)this["shape_type"];
		}
		set
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			this["shape_type"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool allowpopup
	{
		get
		{
			return (bool)this["allowpopup"];
		}
		set
		{
			this["allowpopup"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("False")]
	public bool thanks
	{
		get
		{
			return (bool)this["thanks"];
		}
		set
		{
			this["thanks"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("False")]
	public bool fill_colour
	{
		get
		{
			return (bool)this["fill_colour"];
		}
		set
		{
			this["fill_colour"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	[UserScopedSetting]
	public bool sh_err_desc
	{
		get
		{
			return (bool)this["sh_err_desc"];
		}
		set
		{
			this["sh_err_desc"] = value;
		}
	}

	[DefaultSettingValue("True")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public bool sh_err_comments
	{
		get
		{
			return (bool)this["sh_err_comments"];
		}
		set
		{
			this["sh_err_comments"] = value;
		}
	}

	[DefaultSettingValue("False")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public bool alternate_algo
	{
		get
		{
			return (bool)this["alternate_algo"];
		}
		set
		{
			this["alternate_algo"] = value;
		}
	}

	[DefaultSettingValue("False")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public bool process_ir
	{
		get
		{
			return (bool)this["process_ir"];
		}
		set
		{
			this["process_ir"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	[UserScopedSetting]
	public bool process_2d
	{
		get
		{
			return (bool)this["process_2d"];
		}
		set
		{
			this["process_2d"] = value;
		}
	}
}
