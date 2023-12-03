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
    public class ExercisesViewModel : INotifyPropertyChanged
    {
        // The exercise service
        private readonly IExerciseService _exerciseService;



        // The commands
        public StringToIntCommand ExerciseCreateUpdateCommand { get; private set; }
        public ICommand ExerciseSaveCommand { get; private set; }
        public StringToIntCommand ExerciseDeleteCommand { get; private set; }
        public ICommand PickFileCommand { get; private set; }



        // The SelectedExercise is binded in the CreateUpdate page
        private Exercise _selectedExercise;
        public Exercise SelectedExercise {
            get { return _selectedExercise; }
            set
            {
                if (_selectedExercise != value)
                {
                    _selectedExercise = value;
                }
            }
        }



        // The SelectedExerciseImageUrl is binded in the CreateUpdate page for the ImageUrl
        public string SelectedExerciseImageUrl
        {
            get { return _selectedExercise.ImageUrl; }
            set
            {
                if (_selectedExercise.ImageUrl != value)
                {
                    _selectedExercise.ImageUrl = value;
                    // To change the property in the UI
                    OnPropertyChanged(nameof(SelectedExerciseImageUrl));
                }
            }
        }



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



        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        public ExercisesViewModel()
        {
            // Init the exercise service
            _exerciseService = DependencyService.Get<IExerciseService>();

            // Init the commands
            ExerciseCreateUpdateCommand = new StringToIntCommand(ExerciseCreateUpdate);
            ExerciseSaveCommand = new Command(ExerciseSave);
            ExerciseDeleteCommand = new StringToIntCommand(ExerciseDelete);
            PickFileCommand = new Command(async () => await PickFileAsync());

            // Init the Exercises
            ExercisesLoad();
        }



        /// <summary>
        /// This is a method to handle the create or update
        /// </summary>
        /// <param name="exerciseId"></param>
        private void ExerciseCreateUpdate(int exerciseId)
        {
            // If the exerciseId is 0 its create new exercise
            if (exerciseId == 0)
            {
                // Init SelectedExercise with new exercise
                SelectedExercise = new Exercise();

                // For ImageUrl use the AWS S3 default image
                SelectedExercise.ImageUrl = S3Helper.GetExerciseDefaultImageUrl();
            }
            // If the exerciseId is not 0 its update existing exercise
            else
            {
                // Init SelectedExercise with an exercise that has the given id
                SelectedExercise = _exerciseService.GetSingle(exerciseId).Result;
            }

            // Bind the CreateUpdate page with this viewModel
            var createUpdatePage = new ExercisesCreateUpdatePage { BindingContext = this };

            // Go to the CreateUpdate page
            Application.Current.MainPage.Navigation.PushAsync(createUpdatePage);
        }



        /// <summary>
        /// This is a method to handle the validation
        /// </summary>
        /// <returns></returns>
        private string ExerciseValidate()
        {
            var alertMessage = "";

            // If the SelecetedExercise.Name is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedExercise.Name))
                alertMessage += "The Name is required!\n";

            // If the SelecetedExercise.MuscleType is null or whitespace add display alert message
            if (string.IsNullOrWhiteSpace(SelectedExercise.MuscleType))
                alertMessage += "The Muscle Type is required!\n";

            // If the SelecetedExercise.Reps is equal to 0 add display alert message
            if (SelectedExercise.Reps == 0)
                alertMessage += "The Reps cannot be zero!";

            // If the alert message is not null or whitespace, display it
            if (!string.IsNullOrWhiteSpace(alertMessage))
                Application.Current.MainPage.DisplayAlert("Validation Error", alertMessage, "OK");

            return alertMessage;
        }



        /// <summary>
        /// This is a method to handle the save
        /// </summary>
        private void ExerciseSave()
        {
            var alertMessage = ExerciseValidate();
            if (string.IsNullOrWhiteSpace(alertMessage))
            {
                // If the SelectedExercise.Id is 0 its create new exercise
                if (SelectedExercise.Id == 0)
                    _exerciseService.Create(SelectedExercise);
                // If the SelectedExercise.Id is not 0 its update existing exercise
                else
                    _exerciseService.Update(SelectedExercise);

                // Load the Exercises again to display the changes
                ExercisesLoad();

                // Go back to the Exercises page
                Application.Current.MainPage.Navigation.PopAsync();
            }
        }        



        /// <summary>
        /// This is a method to handle the delete
        /// </summary>
        private void ExerciseDelete(int exerciseId)
        {            
            // If the exerciseId is 0 its delete all exercises
            if (exerciseId == 0)
                _exerciseService.DeleteAll();
            // If the exerciseId is not 0 its delete a single exercise
            else
                _exerciseService.DeleteSingle(exerciseId);

            // Load the Exercises again to display the changes
            ExercisesLoad();
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

                        // Update the SelectedExerciseImageUrl for UI
                        SelectedExerciseImageUrl = imageUrl;
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
