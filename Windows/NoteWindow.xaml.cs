using StickyNoteApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

            //Loading the content of the RichTextBox and its different text properties (bold, italics etc)
            if (!string.IsNullOrWhiteSpace(_viewModel.Content))
            {
                try
                {
                    //Grab the entire portion of content in a TextRange
                    TextRange range = new TextRange(
                        ContentEditor.Document.ContentStart,
                        ContentEditor.Document.ContentEnd);
                    //This uses a MemoryStream to load the view models content
                    using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(_viewModel.Content)))
                    {
                        range.Load(ms, DataFormats.Xaml);
                    }
                }
                catch
                {
                    // If something goes wrong (invalid format?), this will fallback to plain text
                    ContentEditor.Document.Blocks.Clear();
                    ContentEditor.Document.Blocks.Add(new Paragraph(new Run(_viewModel.Content)));
                }
            }

            this.Width = _viewModel.Width;
            this.Height = _viewModel.Height;
            this.Left = _viewModel.X;
            this.Top = _viewModel.Y;
        }

        //Brings up the color selector popup
        private void ColorSelectorOpen_Click(object sender, RoutedEventArgs e)
        {
            ColorPopup.IsOpen = true;
        }
        //takes the selected color and sets the sticky note to the desired color
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Background is SolidColorBrush brush)
            {
                _viewModel.BackgroundColor = brush.ToString();
                ColorPopup.IsOpen = false;
            }
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
            if (Application.Current is App app)
            {
                app.SaveAllNotes();
            }
            this.Hide();
        }

        //SyncContent takes the text within Content and syncs it to the view model.
        //This is used because Content is a RichTextBox which does not have binding properties :(
        public void SyncContent()
        {
            TextRange textRange = new TextRange(
                ContentEditor.Document.ContentStart,
                ContentEditor.Document.ContentEnd);

            //Im using a MemoryStream here to save the entire TextRange as a XAML. This was needed to save the bold and italic properties of text
            using (var ms = new MemoryStream())
            {
                // Save to XAML
                textRange.Save(ms, DataFormats.Xaml);
                ms.Position = 0;

                using (var reader = new StreamReader(ms))
                {
                    _viewModel.Content = reader.ReadToEnd();
                }
            }
        }
    }
}
