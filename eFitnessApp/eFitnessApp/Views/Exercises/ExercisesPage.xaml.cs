﻿using eFitnessApp.Services.Implement;
using eFitnessApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eFitnessApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExercisesPage : ContentPage
    {
        public ExercisesPage()
        {
            InitializeComponent();
            BindingContext = new ExercisesViewModel();
        }
    }
}