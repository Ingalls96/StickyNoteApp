using StickyNoteApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StickyNoteApp.Windows
{
    /// <summary>
    /// Interaction logic for NoteWindow.xaml
    /// </summary>
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

        private void BoldButton_Click(object sender, RoutedEventArgs e)
        {
            //Focus the rich text box holding the bold button
            ContentEditor.Focus();
            //Variable to store the font weight value of the selected text in the note
            var isBold = ContentEditor.Selection.GetPropertyValue(TextElement.FontWeightProperty);
            //Set the font weight to bold or normal by checking if font weight is already bold, and if the selected text is an unset value
            var newWeight = (isBold != DependencyProperty.UnsetValue && (FontWeight)isBold == FontWeights.Bold)
                ? FontWeights.Normal : FontWeights.Bold;

            ContentEditor.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, newWeight);

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            _viewModel.X = this.Left;
            _viewModel.Y = this.Top;
        }
    }
}
