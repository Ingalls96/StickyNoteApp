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
        //Toggles bullet points on each line of the content area of the sticky note window
        private void BulletToggle_Click(object sender, RoutedEventArgs e)
        {
            ContentEditor.Focus();
            //Check if the ToggleBullets command is executable and toggle bullet points
            if (EditingCommands.ToggleBullets.CanExecute(null, ContentEditor))
            {
                EditingCommands.ToggleBullets.Execute(null, ContentEditor);
            }
        }
        //Toggles and sets Italic style font to content area of sticky note
        private void ItalicToggle_Click(Object sender, RoutedEventArgs e)
        {
            ContentEditor.Focus();
            var newFont = ItalicToggle.IsChecked == true
                ? FontStyles.Italic
                : FontStyles.Normal;

            ContentEditor.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, newFont);
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

        //Sets the Toggle checked status of the toggle buttons based on the current status of each toggle buttons value
        private void ContentEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            //get the current font weight selected
            var weight = ContentEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            //Get the current font style selected
            var font = ContentEditor.Selection.GetPropertyValue(TextElement.FontStyleProperty);

            //Toggle the button based on the value of variable weight
            if (weight != DependencyProperty.UnsetValue && (FontWeight)weight == FontWeights.Bold)
            {
                BoldToggle.IsChecked = true;
            }
            else
            {
                BoldToggle.IsChecked = false;
            }

            //Toggle italics button based on the value of the variable font
            if (font != DependencyProperty.UnsetValue && (FontStyle)font == FontStyles.Italic)
            {
                ItalicToggle.IsChecked = true;
            }
            else
            {
                ItalicToggle.IsChecked = false;
            }
        }

        //Makes sticky note window draggable without the default window style header
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            _viewModel.X = this.Left;
            _viewModel.Y = this.Top;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
