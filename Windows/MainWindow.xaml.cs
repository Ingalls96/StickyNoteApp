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
        //Creates a new sticky note
        public void Click_CreateNote(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainMenuViewModel menuVM)
            {
                Note newNote = new Note();
                NoteViewModel newNoteVM = new NoteViewModel(newNote);
                menuVM.Notes.Add(newNoteVM);

                var noteWindow = new NoteWindow(newNoteVM);
                noteWindow.Show();
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Call SaveAllNotes via App instance
            if (Application.Current is App app)
            {
                foreach (Window window in app.Windows)
                {
                    window.Hide();
                }
                app.SaveAllNotes();
            }
        }
        //Makes window draggable without Built in Window toolbar
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Custom close button that saves all notes then shuts down program
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current is App app) 
            {
                //Hide each sticky note before saving them
                foreach (Window window in Application.Current.Windows)
                {
                    window.Hide();
                }
                app.SaveAllNotes();
                this.Close();
            }
            Application.Current.Shutdown();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        //Dictionary for tracking open sticky note windows
        private readonly Dictionary<NoteViewModel, NoteWindow> openNoteWindows = new();
        
        //Grabs the selected note from the Dictionary of openNoteWindows and shows the window
        private void OpenNote_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is NoteViewModel noteVM)
            {
                if (openNoteWindows.TryGetValue(noteVM, out var existingWindow))
                {
                    if (!existingWindow.IsVisible)
                        existingWindow.Show();

                    existingWindow.Activate();
                }
                else
                {
                    var newWindow = new NoteWindow(noteVM);
                    newWindow.Closed += (s, args) => openNoteWindows.Remove(noteVM);
                    openNoteWindows[noteVM] = newWindow;
                    newWindow.Show();
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var note = button?.Tag as NoteViewModel;

            if (note != null) 
            {
                var viewModel = DataContext as MainMenuViewModel;
                viewModel?.Notes.Remove(note);
                
            }
        }
    }
}