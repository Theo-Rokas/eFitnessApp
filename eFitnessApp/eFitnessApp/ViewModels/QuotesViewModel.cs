using eFitnessApp.Helpers;
using eFitnessApp.Models;
using eFitnessApp.Services.Abstruct;
using eFitnessApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace eFitnessApp.ViewModels
{
    public class QuotesViewModel : INotifyPropertyChanged
    {
        // The exercise service
        private readonly IQuoteService _quoteService;



        // The commands
        public StringToIntCommand QuoteCreateUpdateCommand { get; private set; }
        public ICommand QuoteSaveCommand { get; private set; }
        public StringToIntCommand QuoteDeleteCommand { get; private set; }



        // The SelectedQuote is binded in the CreateUpdate page
        private Quote _selectedQuote;
        public Quote SelectedQuote
        {
            get { return _selectedQuote; }
            set
            {
                if (_selectedQuote != value)
                {
                    _selectedQuote = value;
                }
            }
        }



        // The Quotes are binded in the Quote page
        private ObservableCollection<Quote> _quotes;
        public ObservableCollection<Quote> Quotes
        {
            get { return _quotes; }
            set
            {
                if (_quotes != value)
                {
                    _quotes = value;
                    // To change the property in the UI
                    OnPropertyChanged(nameof(Quotes));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public QuotesViewModel()
        {
            // Init the quote service
            _quoteService = DependencyService.Get<IQuoteService>();

            // Init the commands
            QuoteCreateUpdateCommand = new StringToIntCommand(QuoteCreateUpdate);
            QuoteSaveCommand = new Command(QuoteSave);
            QuoteDeleteCommand = new StringToIntCommand(QuoteDelete);

            // Init the Quotes
            QuotesLoad();
        }



        /// <summary>
        /// This is a method to handle the create or update
        /// </summary>
        /// <param name="quoteId"></param>
        private void QuoteCreateUpdate(int quoteId)
        {
            // If the quoteId is 0 its create new quote
            if (quoteId == 0)
            {
                // Init SelectedQuote with new quote
                SelectedQuote = new Quote();
            }
            // If the quoteeId is not 0 its update existing quote
            else
            {
                // Init SelectedQuote with an quote that has the given id
                SelectedQuote = _quoteService.GetSingle(quoteId).Result;
            }

            // Bind the CreateUpdate page with this viewModel
            var createUpdatePage = new QuotesCreateUpdatePage { BindingContext = this };

            // Go to the CreateUpdate page
            Application.Current.MainPage.Navigation.PushAsync(createUpdatePage);
        }



        /// <summary>
        /// This is a method to handle the validation
        /// </summary>
        /// <returns></returns>
        private string QuoteValidate()
        {
            var alertMessage = "";

            // If the SelecetedQuote.Author is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedQuote.Author))
                alertMessage += "The Author is required!\n";

            // If the SelecetedQuote.Words is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedQuote.Words))
                alertMessage += "The Words are required!";

            // If the alert message is not null or whitespace, display it
            if (!string.IsNullOrWhiteSpace(alertMessage))
                Application.Current.MainPage.DisplayAlert("Validation Error", alertMessage, "OK");

            return alertMessage;
        }



        /// <summary>
        /// This is a method to handle the save
        /// </summary>
        private void QuoteSave()
        {
            var alertMessage = QuoteValidate();
            if (string.IsNullOrWhiteSpace(alertMessage))
            {
                // If the SelectedQuote.Id is 0 its create new quote
                if (SelectedQuote.Id == 0)
                    _quoteService.Create(SelectedQuote);
                // If the SelectedQuote.Id is not 0 its update existing exercise
                else
                    _quoteService.Update(SelectedQuote);

                // Load the Quotes again to display the changes
                QuotesLoad();

                // Go back to the Quotes page
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }



        /// <summary>
        /// This is a method to handle the delete
        /// </summary>
        private void QuoteDelete(int quoteId)
        {
            // If the quoteId is 0 its delete all quotes
            if (quoteId == 0)
                _quoteService.DeleteAll();
            // If the quoteId is not 0 its delete a single quote
            else
                _quoteService.DeleteSingle(quoteId);

            // Load the Quotes again to display the changes
            QuotesLoad();
        }


        /// <summary>
        /// This a method to handle the Quotes load
        /// </summary>
        private void QuotesLoad()
        {
            // Get all quotes
            var quotes = _quoteService.GetAll().Result;

            // Init the Quotes
            Quotes = new ObservableCollection<Quote>(quotes);
        } 
    }
}
