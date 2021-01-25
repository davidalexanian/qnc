using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Converter.Utils;

namespace Converter.Wpf
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region .ctor & methods

        public ViewModel()
        {
            this.userInput = "12345,67";
            this.isValid = true;
            this.ConvertCmd = new AsyncCommand(() => ConvertToWords(), () => this.IsValid);
        }

        private async Task ConvertToWords()
        {
            try {
                await Task.Delay(500);  // emulate some delay
                var response = await new ConverterService().SumToWordsAsync(this.UserInput).ConfigureAwait(false);
                this.Words = response.Result ?? response.Error;
            }
            catch (Exception ex) {
                this.Words = ex.Message;
            }
        }

        #endregion


        #region binding props

        public AsyncCommand ConvertCmd { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public string UserInput {
            get => this.userInput;
            set {
                this.userInput = value;
                if (this.PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(UserInput)));
                }
                this.IsValid = CurrencyToWordConverter.IsValidNumericString(value);
            }
        }
        private string userInput;

        public bool IsValid {
            get => this.isValid;
            set {
                this.isValid = value;
                if (this.PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(IsValid)));
                }
                this.ConvertCmd.RaiseCanExecute();
            }
        }

        private bool isValid;

        public string Words {
            get => this.words;
            set {
                this.words = value;
                if (this.PropertyChanged != null) {
                    PropertyChanged(this, new PropertyChangedEventArgs(nameof(Words)));
                }
            }
        }
        private string words;


        #endregion
    }
}
