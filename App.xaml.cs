using StickyNoteApp.Models;
using StickyNoteApp.Services;
using StickyNoteApp.ViewModels;
using StickyNoteApp.Windows;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;

namespace StickyNoteApp
{
    public partial class App : Application
    {
        private NoteStorage _noteStorage = new NoteStorage();

        public MainMenuViewModel MainMenuViewModel { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Create and load main menu
            MainMenuViewModel = new MainMenuViewModel(new NoteStorage());
            MainWindow mainWindow = new MainWindow
            {
                DataContext = MainMenuViewModel
            };
            mainWindow.Show();

            // Opens windows for each note loaded into the shared ViewModel
            foreach (var noteVM in MainMenuViewModel.Notes)
            {
                var window = new NoteWindow(noteVM);
                window.Left = noteVM.X;
                window.Top = noteVM.Y;
                window.Width = noteVM.Width;
                window.Height = noteVM.Height;
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            //base.OnExit(e);
            //SaveAllNotes();
        }

        public void SaveAllNotes()
        {
            Debug.WriteLine("Saving all notes...");
            if (MainMenuViewModel == null)
                return;

            // Sync window positions and content
            foreach (Window window in Current.Windows)
            {
                if (window is NoteWindow noteWindow &&
                    noteWindow.DataContext is NoteViewModel vm)
                {
                    //Sync Content from the view model
                    noteWindow.SyncContent();
                    vm.X = noteWindow.Left;
                    vm.Y = noteWindow.Top;
                    vm.Width = noteWindow.Width;
                    vm.Height = noteWindow.Height;
                }
            }

            // Save only the Notes from the shared ViewModel
            var allNotes = MainMenuViewModel.Notes.Select(vm => vm.Note).ToList();
            Debug.WriteLine($"Notes to save: {allNotes.Count}");
            _noteStorage.SaveNotes(allNotes);
        }
    }

}
