using System;
using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace TextConverter
{
    public class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public Book(string title, string author, string genre)
        {
            this.Title = title;
            this.Author = author;
            this.Genre = genre;
        }
        public void SaveToJson(string filePath)
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, json);
            Console.WriteLine("Данные успешно сохранены в файл JSON.");
        }
        public void SaveToXml(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Book));
                xmlSerializer.Serialize(fileStream, this);
                Console.WriteLine("Данные успешно сохранены в файл XML.");
            }
        }

    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь к файлу(с названием):");
            Console.WriteLine("----------------------------------");
            string filePath = Console.ReadLine();
            string textData = "";
            string[] txt = null;
            if (filePath.EndsWith(".txt"))
            {
                textData = File.ReadAllText(filePath);
                txt = textData.Split("\n");
            }
            else if (filePath.EndsWith(".json"))
            {
                textData = LoadJson(filePath);
            }
            else if (filePath.EndsWith(".xml"))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Book));
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    Book book = xmlSerializer.Deserialize(fs) as Book;
                    textData = book.ToString();
                }
            }
            else
            {
                Console.WriteLine("Неподдерживаемый формат чтения.");
                return;
            }
            Console.Clear();
            Console.WriteLine("Чтобы сохранить файл в другом формате - F1,Чтобы выйти - Esc");
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine(textData);
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.F1)
            {
                Console.Clear();
                Console.WriteLine("Введите путь к файлу(с названием) для сохранения:");
                Console.WriteLine("-------------------------------------------------");
                string filePath2 = Console.ReadLine();
                if (filePath2.EndsWith(".txt"))
                {
                    File.WriteAllText(filePath2, textData);
                }
                else if (filePath2.EndsWith(".json"))
                {
                    List<Book> books = new List<Book>();
                    for (int i = 0; i < txt.Length; i += 3)
                    {
                        string title = txt[i];
                        string author = txt[i + 1];
                        string genre = txt[i + 2];
                        Book book = new Book(title, author, genre);
                        books.Add(book);
                    }
                    foreach (var items in books)
                    {
                        items.SaveToJson(filePath2);
                    }
                }
                else if (filePath2.EndsWith(".xml"))
                {
                    List<Book> books = new List<Book>();
                    for (int i = 0; i < txt.Length; i += 3)
                    {
                        string title = txt[i];
                        string author = txt[i + 1];
                        string genre = txt[i + 2];
                        Book book = new Book(title, author, genre);
                        books.Add(book);
                    }
                    foreach (var items in books)
                    {
                        items.SaveToXml(filePath2);
                    }
                }
                else
                {
                    Console.WriteLine("Неподдерживаемый формат сохранения.");
                    return;
                }
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
        }
        static string LoadJson(string filePath)
        {
            string json = File.ReadAllText(filePath);
            List<Book> book = JsonConvert.DeserializeObject<List<Book>>(json);
            string[] text = null;
            foreach(var item in book)
            {
                text.Append(item.ToString());
            }
            string txt = text.ToString();
            return txt;
        }
    }
}
