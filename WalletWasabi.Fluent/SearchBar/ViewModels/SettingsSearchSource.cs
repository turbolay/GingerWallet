using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using DynamicData;
using ReactiveUI;
using WalletWasabi.Fluent.Models.UI;
using WalletWasabi.Fluent.SearchBar.Interfaces;
using WalletWasabi.Fluent.SearchBar.Models;
using WalletWasabi.Fluent.SearchBar.Models.Settings;
using WalletWasabi.Fluent.SearchBar.ViewModels.SearchItems;
using WalletWasabi.Fluent.SearchBar.ViewModels.Sources;
using WalletWasabi.Models;

namespace WalletWasabi.Fluent.SearchBar.ViewModels;

public class SettingsSearchSource : ReactiveObject, ISearchSource
{
	private readonly UiContext _uiContext;
	private readonly IApplicationSettings _applicationSettings;

	public SettingsSearchSource(UiContext uiContext, IObservable<string> query)
	{
		_uiContext = uiContext;
		_applicationSettings = uiContext.ApplicationSettings;

		var filter = query.Select(SearchSource.DefaultFilter);

		Changes = GetSettingsItems()
			.ToObservable()
			.ToObservableChangeSet(x => x.Key)
			.Filter(filter);
	}

	public IObservable<IChangeSet<ISearchItem, ComposedKey>> Changes { get; }

	private IEnumerable<ISearchItem> GetSettingsItems()
	{
		var isEnabled = !_applicationSettings.IsOverridden;

		yield return new ContentSearchItem(content: Setting(selector: x => x.DarkModeEnabled), name: "Dark mode", category: "Appearance", keywords: new List<string> { "Black", "White", "Theme", "Dark", "Light" }, icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 1 };
		yield return new ContentSearchItem(content: Setting(selector: x => x.AutoCopy), name: "Auto copy addresses", category: "Settings", keywords: new List<string>(), icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 2 };
		yield return new ContentSearchItem(content: Setting(selector: x => x.AutoPaste), name: "Auto paste addresses", category: "Settings", keywords: new List<string>(), icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 3 };
		yield return new ContentSearchItem(content: Setting(selector: x => x.HideOnClose), name: "Run in background when closed", category: "Settings", keywords: new List<string>() { "hide", "tray" }, icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 4 };
		yield return new ContentSearchItem(content: Setting(selector: x => x.RunOnSystemStartup), name: "Run Ginger when computer starts", category: "Settings", keywords: new List<string>() { "startup", "boot" }, icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 5 };
		yield return new ContentSearchItem(content: Setting(selector: x => x.EnableGpu), name: "Enable GPU", category: "Settings", keywords: new List<string>(), icon: "nav_settings_regular", isEnabled) { IsDefault = false, Priority = 6 };

		yield return ContentSearchItemNode.Create(
			searchSource: _uiContext.EditableSearchSource,
			setting: Setting(selector: x => x.UseTor),
			name: "Network anonymization (Tor)",
			category: "Settings",
			isDefault: false,
			keywords: new List<string>(),
			icon: "nav_settings_regular",
			priority: 7,
			isEnabled,
			nestedItemConfiguration: new NestedItemConfiguration<TorMode>(
				isDisplayed: mode => mode != TorMode.Disabled,
				item: new ContentSearchItem(
					content: Setting(selector: x => x.TerminateTorOnExit),
					name: "Terminate Tor when Ginger shuts down",
					category: "Settings",
					keywords: new List<string>(),
					icon: "nav_settings_regular",
					isEnabled)
				{
					IsDefault = false,
					Priority = 8
				}));

		yield return ContentSearchItemNode.Create(
			searchSource: _uiContext.EditableSearchSource,
			setting: Setting(selector: x => x.StartLocalBitcoinCoreOnStartup),
			name: "Run Bitcoin Knots on startup",
			category: "Settings",
			isDefault: false,
			keywords: new List<string>(),
			icon: "nav_settings_regular",
			priority: 7,
			isEnabled,
			nestedItemConfiguration: new NestedItemConfiguration<bool>(
				isDisplayed: isVisible => isVisible,
				item: new ContentSearchItem(
					content: Setting(selector: x => x.StopLocalBitcoinCoreOnShutdown),
					name: "Stop Bitcoin Knots on shutdown",
					category: "Settings",
					keywords: new List<string>(),
					icon: "nav_settings_regular",
					isEnabled)
				{
					IsDefault = false,
					Priority = 8
				}));
	}

	private Setting<ApplicationSettings, TProperty> Setting<TProperty>(Expression<Func<ApplicationSettings, TProperty>> selector)
	{
		return new Setting<ApplicationSettings, TProperty>((ApplicationSettings)_applicationSettings, selector);
	}
}
