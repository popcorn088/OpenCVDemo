using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenCVDemo.ViewModels
{
	public class TextBoxViewModel : BindableBase, IDialogAware
	{
        public string Title => string.Empty;
        private string _inputText = "10x10";
        public string InputText
        {
            get => _inputText;
            set => SetProperty(ref _inputText, value);
        }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand OkCommand { get; }
        public event Action<IDialogResult> RequestClose;

        public TextBoxViewModel()
        {
            CancelCommand = new DelegateCommand(() =>
            {
                RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
            });
            OkCommand = new DelegateCommand(() =>
            {
                var parameters = new DialogParameters
                {
                    { "InputText", InputText },
                };
                RequestClose?.Invoke(new DialogResult(ButtonResult.OK, parameters));
            });
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {

        }
    }
}
