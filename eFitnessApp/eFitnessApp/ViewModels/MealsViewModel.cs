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
    public class MealsViewModel : INotifyPropertyChanged
    {
        // The meal service
        private readonly IMealService _mealService;



        // The commands
        public StringToIntCommand MealCreateUpdateCommand { get; private set; }
        public ICommand MealSaveCommand { get; private set; }
        public StringToIntCommand MealDeleteCommand { get; private set; }
        public ICommand PickFileCommand { get; private set; }



        // The SelectedMeal is binded in the CreateUpdate page
        private Meal _selectedMeal;
        public Meal SelectedMeal
        {
            get { return _selectedMeal; }
            set
            {
                if (_selectedMeal != value)
                {
                    _selectedMeal = value;
                }
            }
        }



        // The SelectedMealImageUrl is binded in the CreateUpdate page for the ImageUrl
        public string SelectedMealImageUrl
        {
            get { return _selectedMeal.ImageUrl; }
            set
            {
                if (_selectedMeal.ImageUrl != value)
                {
                    _selectedMeal.ImageUrl = value;
                    // To change the property in the UI
                    OnPropertyChanged(nameof(SelectedMealImageUrl));
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



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public MealsViewModel()
        {
            // Init the meal service
            _mealService = DependencyService.Get<IMealService>();

            // Init the commands
            MealCreateUpdateCommand = new StringToIntCommand(MealCreateUpdate);
            MealSaveCommand = new Command(MealSave);
            MealDeleteCommand = new StringToIntCommand(MealDelete);
            PickFileCommand = new Command(async () => await PickFileAsync());

            // Init the Meals
            MealsLoad();
        }



        /// <summary>
        /// This is a method to handle the create or update
        /// </summary>
        /// <param name="mealId"></param>
        private void MealCreateUpdate(int mealId)
        {
            // If the mealId is 0 its create new meal
            if (mealId == 0)
            {
                // Init SelectedMeal with new meal
                SelectedMeal = new Meal();

                // For ImageUrl use the AWS S3 default image
                SelectedMeal.ImageUrl = S3Helper.GetMealDefaultImageUrl();
            }
            // If the mealId is not 0 its update existing meal
            else
            {
                // Init SelectedMeal with an meal that has the given id
                SelectedMeal = _mealService.GetSingle(mealId).Result;
            }

            // Bind the CreateUpdate page with this viewModel
            var createUpdatePage = new MealsCreateUpdatePage { BindingContext = this };

            // Go to the CreateUpdate page
            Application.Current.MainPage.Navigation.PushAsync(createUpdatePage);
        }



        /// <summary>
        /// This is a method to handle the validation
        /// </summary>
        /// <returns></returns>
        private string MealValidate()
        {
            var alertMessage = "";

            // If the SelecetedMeal.Name is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedMeal.Name))
                alertMessage += "The Name is required!\n";

            // If the SelecetedMeal.NutritionType is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedMeal.NutritionType))
                alertMessage += "The Nutrition Type is required!\n";

            // If the SelecetedMeal.Frequency is equal to 0 add display alert message
            if (SelectedMeal.Frequency == 0)
                alertMessage += "The Frequency cannot be zero!";

            // If the alert message is not null or whitespace, display it
            if (!string.IsNullOrWhiteSpace(alertMessage))
                Application.Current.MainPage.DisplayAlert("Validation Error", alertMessage, "OK");

            return alertMessage;
        }



        /// <summary>
        /// This is a method to handle the save
        /// </summary>
        private void MealSave()
        {
            var alertMessage = MealValidate();
            if (string.IsNullOrWhiteSpace(alertMessage))
            {
                // If the SelectedMeal.Id is 0 its create new meal
                if (SelectedMeal.Id == 0)
                    _mealService.Create(SelectedMeal);
                // If the SelectedMeal.Id is not 0 its update existing meal
                else
                    _mealService.Update(SelectedMeal);

                // Load the Meals again to display the changes
                MealsLoad();

                // Go back to the Meals page
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }



        /// <summary>
        /// This is a method to handle the delete
        /// </summary>
        private void MealDelete(int mealId)
        {
            // If the mealId is 0 its delete all meals
            if (mealId == 0)
                _mealService.DeleteAll();
            // If the mealId is not 0 its delete a single meal
            else
                _mealService.DeleteSingle(mealId);

            // Load the Meals again to display the changes
            MealsLoad();
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
        /// This is a method handle the image file picking
        /// </summary>
        /// <returns></returns>
        private async Task PickFileAsync()
        {
            // Check reading storage permission
            var imagePermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();

            // If the user not allowed reading storage permission
            if (imagePermissionStatus != PermissionStatus.Granted)
                imagePermissionStatus = await Permissions.RequestAsync<Permissions.StorageRead>();

            // If the user allowed reading storage permission
            if (imagePermissionStatus == PermissionStatus.Granted)
            {
                try
                {
                    // Open user gallery to pick image file
                    var imageFile = await FilePicker.PickAsync();

                    // If the image file is not null
                    if (imageFile != null)
                    {
                        // Upload the image file to AWS S3
                        var imageUrl = await S3Helper.UploadFile(imageFile);

                        // Update the SelectedMealImageUrl for UI
                        SelectedMealImageUrl = imageUrl;
                    }
                }
                catch (Exception)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong!", "OK");
                }
            }
        }
    }
}
