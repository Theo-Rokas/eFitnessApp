using eFitnessApp.Services.Abstruct;
using eFitnessApp.Services.Implement;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eFitnessApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            // Init the database context
            DatabaseContext.Initialize();

            // Register the services
            DependencyService.Register<IExerciseService, ExerciseService>();
            DependencyService.Register<IMealService, MealService>();
            DependencyService.Register<IQuoteService, QuoteService>();

            MainPage = new AppShell();            
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
