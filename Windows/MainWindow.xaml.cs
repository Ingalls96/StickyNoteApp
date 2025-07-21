using StickyNoteApp.Models;
using StickyNoteApp.ViewModels;
using StickyNoteApp.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using StickyNoteApp.Services;

namespace StickyNoteApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainMenuViewModel(new NoteStorage());
            this.Closing += MainWindow_Closing;
        }

        public void Click_CreateNote(object sender, RoutedEventArgs e)
        {
            Note note = new Note();
            NoteViewModel viewModel = new NoteViewModel(note);
            NoteWindow noteWindow = new NoteWindow(viewModel);

            noteWindow.Show();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Call SaveAllNotes via App instance
            if (Application.Current is App app)
            {
                app.SaveAllNotes();
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current is App app) 
            {
                app.SaveAllNotes();
                this.Close();
            }
        }
    }
}