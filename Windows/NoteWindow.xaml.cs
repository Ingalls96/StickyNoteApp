using StickyNoteApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StickyNoteApp.Windows
{
    public partial class NoteWindow : Window
    {
        private NoteViewModel _viewModel;
        public NoteWindow(NoteViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            this.Width = _viewModel.Width;
            this.Height = _viewModel.Height;
            this.Left = _viewModel.X;
            this.Top = _viewModel.Y;
        }
        private void BulletToggle_Click(object sender, RoutedEventArgs e)
        {
            ContentEditor.Focus();
            if (EditingCommands.ToggleBullets.CanExecute(null, ContentEditor))
            {
                EditingCommands.ToggleBullets.Execute(null, ContentEditor);
            }
        }

        //Sets the font weight to normal or bold depending on toggle buttons status (IsChecked)
        private void BoldToggle_Click(object sender, RoutedEventArgs e)
        {
            ContentEditor.Focus();
            //Set newWeight variable to bold or normal text by checking IsChecked on BoldToggle
            var newWeight = BoldToggle.IsChecked == true
                ? FontWeights.Bold
                : FontWeights.Normal;
            ////Set ContentEditor font weight to the newWeight value
            ContentEditor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);
        }

        //Sets the Toggle checked status of the toggle button based on the font weight of the current text selected in ContentEditor
        private void ContentEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //get the current font weight selected
            var weight = ContentEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);

            //Toggle the button based on the value of variable weight
            if (weight != DependencyProperty.UnsetValue && (FontWeight)weight == FontWeights.Bold)
            {
                BoldToggle.IsChecked = true;
            }
            else
            {
                BoldToggle.IsChecked = false;
            }
        }

        //Makes sticky note window draggable without the default window style header
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            _viewModel.X = this.Left;
            _viewModel.Y = this.Top;
        }
    }
}
