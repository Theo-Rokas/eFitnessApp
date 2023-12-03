using eFitnessApp.Helpers;
using eFitnessApp.Models;
using eFitnessApp.Services.Abstruct;
using eFitnessApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace eFitnessApp.ViewModels
{
    class HomeViewModel : INotifyPropertyChanged
    {
        // The exercise service
        private readonly IExerciseService _exerciseService;

        // The meal service
        private readonly IMealService _mealService;

        // The exercise service
        private readonly IQuoteService _quoteService;



        // The Exercises are binded in the Exercise page
        private ObservableCollection<Exercise> _exercises;
        public ObservableCollection<Exercise> Exercises
        {
            get { return _exercises; }
            set
            {
                if (_exercises != value)
                {
                    _exercises = value;
                    // To change the property in the UI
                    OnPropertyChanged(nameof(Exercises));
                }
            }
        }

        // The Meals are binded in the Meal page
        private ObservableCollection<Meal> _meals;
        public ObservableCollection<Meal> Meals
        {
            get { return _meals; }
            set
            {
                if (_meals != value)
                {
                    _meals = value;
                    // To change the property in the UI
                    OnPropertyChanged(nameof(Meals));
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



        public HomeViewModel()
        {
            // Init the exercise service
            _exerciseService = DependencyService.Get<IExerciseService>();           

            // Init the meal service
            _mealService = DependencyService.Get<IMealService>();

            // Init the quote service
            _quoteService = DependencyService.Get<IQuoteService>();

            // Init the Exercises
            ExercisesLoad();

            // Init the Meals
            MealsLoad();

            // Init the Quotes
            QuotesLoad();
        }

        /// <summary>
        /// This a method to handle the Exercises load
        /// </summary>
        private void ExercisesLoad()
        {
            // Get all exercises
            var exercises = _exerciseService.GetAll().Result;

            // Init the Exercises
            Exercises = new ObservableCollection<Exercise>(exercises);
        }

        /// <summary>
        /// This a method to handle the Meals load
        /// </summary>
        private void MealsLoad()
        {
            // Get all meals
            var meals = _mealService.GetAll().Result;

            // Init the Meals
            Meals = new ObservableCollection<Meal>(meals);
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
