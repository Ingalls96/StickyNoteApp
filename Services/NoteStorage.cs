using StickyNoteApp.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace StickyNoteApp.Services
{
    class NoteStorage
    {

        //Creates Directory to User's Local AppData folder to store Notes created during launch
        private readonly string _filePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "StickyNoteApp",
            "notes.json");

        //Creates a List of all notes created, Serializes the Data of each note and writes it to local filepath file
        public void SaveNotes(List<Note> notes) 
        {
            //Create Directory
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));
            //JSON Serialize
            string json = JsonSerializer.Serialize(notes, new JsonSerializerOptions { WriteIndented = true });
            //Write to file
            File.WriteAllText(_filePath, json);
        }

        //Checks if there is an existing file containing notes created by user, returns a new List of type Note if file does NOT exist, Deserializes the json file and loads it into the list
        public List<Note> LoadNotes()
        {
            //Check if file exists, return new List if it does NOT exist
            if(!File.Exists(_filePath)) 
                return new List<Note>();
            //Create string containing filePath text being read
            string json = File.ReadAllText(_filePath);
            //Deserialize the JSON file
            return JsonSerializer.Deserialize<List<Note>>(json);
        }
    }
}
