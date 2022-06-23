using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WindowsFormsApp1.Properties;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
internal sealed class Settings : ApplicationSettingsBase
{
	private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());

	public static Settings Default => defaultInstance;

	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	[UserScopedSetting]
	public bool setboot
	{
		get
		{
			return (bool)this["setboot"];
		}
		set
		{
			this["setboot"] = value;
		}
	}

	[DefaultSettingValue("C:/temp")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public string setpath
	{
		get
		{
			return (string)this["setpath"];
		}
		set
		{
			this["setpath"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("")]
	[UserScopedSetting]
	public string setscreen
	{
		get
		{
			return (string)this["setscreen"];
		}
		set
		{
			this["setscreen"] = value;
		}
	}

	[DefaultSettingValue("\\\\pmumnetapp-1\\GPO_Production\\RCP\\Deep_Learning_Program_Global_New\\LMBO Signs & Objects\\2018\\Q118\\Shared Documents")]
	[ApplicationScopedSetting]
	[DebuggerNonUserCode]
	public string log_path => (string)this["log_path"];

	[DefaultSettingValue("Auto")]
	[UserScopedSetting]
	[DebuggerNonUserCode]
	public string mode
	{
		get
		{
			return (string)this["mode"];
		}
		set
		{
			this["mode"] = value;
		}
	}

	[DebuggerNonUserCode]
	[DefaultSettingValue("True")]
	[UserScopedSetting]
	public bool Auto_mode
	{
		get
		{
			return (bool)this["Auto_mode"];
		}
		set
		{
			this["Auto_mode"] = value;
		}
	}

	[DebuggerNonUserCode]
	[UserScopedSetting]
	[DefaultSettingValue("1")]
	public int notif_style
	{
		get
		{
			return (int)this["notif_style"];
		}
		set
		{
			this["notif_style"] = value;
		}
	}

	[UserScopedSetting]
	[DebuggerNonUserCode]
	[DefaultSettingValue("False")]
	public bool enable_widget
	{
		get
		{
			return (bool)this["enable_widget"];
		}
		set
		{
			this["enable_widget"] = value;
		}
	}
}
