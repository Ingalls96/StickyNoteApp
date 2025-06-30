using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using StickyNoteApp.Models;

namespace StickyNoteApp.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {   
        //Wrap the model
        private Note _note;

        //Constructor creating instance of note
        public NoteViewModel(Note note)
        {
            _note = note;
        }

        //Properties of the note binding to UI
        public string Title
        {
            get => _note.Title;
            set
            {
                if (_note.Title != value)   //Checks if the value of the property has changed for automatic UI updates
                {
                    _note.Title = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Content
        {
            get => _note.Content;
            set
            {
                if(_note.Content != value)
                {
                    _note.Content = value;
                    OnPropertyChanged();
                }
            }
        }

        public double X
        {
            get => _note.X;
            set
            {
                if (_note.X != value)
                { 
                    _note.X = value;
                    OnPropertyChanged();}
            }
        }
        public double Y
        {
            get => _note.Y;
            set
            {
                if (_note.Y != value)
                {
                    _note.Y = value; 
                    OnPropertyChanged();
                }
            }
        }

        public double Width
        {
            get => _note.Width;
            set
            {
                if( _note.Width != value)
                {
                    _note.Width = value; 
                    OnPropertyChanged();
                }
            }
        }
        public double Height
        {
            get => _note.Height;
            set
            {
                if (_note.Height != value)
                {
                    _note.Height = value; 
                    OnPropertyChanged();
                }
            }
        }

        public string BackgroundColor
        {
            get => _note.BackgroundColor;
            set
            {
                if (_note.BackgroundColor != value)
                { 
                    _note.BackgroundColor = value; 
                    OnPropertyChanged();
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    
}
