using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWrittingTest.Core
{
    public class FileWriter
    {

        /// <summary>
        /// 
        /// </summary>
        private FileStream _fileStream;

        /// <summary>
        /// Stream file writer 
        /// </summary>
        private StreamWriter _streamWriter;

        /// <summary>
        /// Stream file reader
        /// </summary>
        private StreamReader _streamReader;

        public FileWriter(string path, string name)
        {
            _fileStream = File.Open(Path.Combine(path, name) + ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
            _streamReader = new StreamReader(_fileStream);
            _streamWriter = new StreamWriter(_fileStream);
        }

        public bool Peek()
        {
            return _streamReader.Peek() >= 0;
        }

        public string ReadLine()
        {
            return _streamReader.ReadLine();
        }

        public void SetLenght(int newLenght)
        {
            //_streamReader.DiscardBufferedData();
            _fileStream.SetLength(newLenght);
        }

        public void BeginUpdate()
        {
            _streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
        }

        public void Update(string record)
        {            
            _streamWriter.WriteLine(record);
        }

        public void EndUpdate()
        {
            _streamWriter.Flush();
        }

    }
}
