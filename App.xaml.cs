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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            //Create a variable to store the LoadNotes output
            var notes = _noteStorage.LoadNotes();
            //foreach loop for every note in notes
            foreach (var note in notes)
            {
                //Create a new ViewModel for each note
                var viewModel = new NoteViewModel(note);
                //Create a new NoteWindow and then grab data context for the current note
                var window = new NoteWindow(viewModel);
                //Load the position and size of the window, then show the window
                window.Left = note.X;
                window.Top = note.Y;
                window.Width = note.Width;
                window.Height = note.Height;

                window.Show();
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
            //Create a List to collect all notes user created
            List<Note> allNotes = new List<Note>();
            //Start a foreach loop for every current window of the program
            foreach (Window window in Current.Windows)
            {
                //Check if each window is a NoteWindow
                if (window is NoteWindow noteWindow)
                {
                    //Grab the Data context of each note by creating a NoteViewModel
                    NoteViewModel noteViewModel = noteWindow.DataContext as NoteViewModel;

                    //If statement making sure the note object is not null, then saving the position and size of the window (x and y for position) (Width Height for size)
                    if (noteViewModel != null)
                    {
                        //Cast the NoteViewModel to a Note object so we can work with the data as a Note
                        Note note = noteViewModel.Note;

                        note.X = noteWindow.Left;
                        note.Y = noteWindow.Top;
                        note.Width = noteWindow.Width;
                        note.Height = noteWindow.Height;

                    //Add the note to the allNotes List
                    allNotes.Add(note);
                    }
                }
            }
            Debug.WriteLine($"Notes to save: {allNotes.Count}");
            _noteStorage.SaveNotes(allNotes);
        }
    }

}
