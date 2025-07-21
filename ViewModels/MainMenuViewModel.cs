using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StickyNoteApp.Models;
using StickyNoteApp.Services;

namespace StickyNoteApp.ViewModels
{
    internal class MainMenuViewModel
    {
        //Member variable to access NoteStorage methods
        private readonly NoteStorage _noteStorage;
        //ObservableCollection can be used here to automatically observe and update the list of notes on a users program
        public ObservableCollection<Note> Notes { get; set; }
        
        public MainMenuViewModel(NoteStorage noteStorage)
        {
            _noteStorage = noteStorage;
            Notes = new ObservableCollection<Note>();
            Load();
        }

        //Load will collect the notes from LoadNotes functions and add them into the ObservableCollection list
        private void Load()
        {
            var loadedNotes = _noteStorage.LoadNotes();

            foreach (var note in loadedNotes)
            {
                Notes.Add(note);
            }
        }
    }
}
