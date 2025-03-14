using System.Windows.Input;
using ReactiveUI;
using WalletWasabi.Fluent.Common.ViewModels;
using WalletWasabi.Fluent.Extensions;
using WalletWasabi.Fluent.Models.UI;

namespace WalletWasabi.Fluent.OpenDirectory.ViewModels;

public abstract class OpenFileViewModel : TriggerCommandViewModel
{
	public OpenFileViewModel(UiContext uiContext)
	{
		UiContext = uiContext;
	}

	public abstract string FilePath { get; }

	public override ICommand TargetCommand =>
		ReactiveCommand.CreateFromTask(async () =>
		{
			try
			{
				await UiContext.FileSystem.OpenFileInTextEditorAsync(FilePath);
			}
			catch (Exception ex)
			{
				await ShowErrorAsync("Open", ex.ToUserFriendlyString(), "Ginger Wallet was unable to open the file");
			}
		});
}
