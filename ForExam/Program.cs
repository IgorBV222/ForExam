using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
class MyClass
{
    private Guid Id;   
    public string Name { get; set; }    
    public Guid UnicalId        
    {
        get
        {
            return Id;
        }
    }
    public MyClass(string name)
    {
        Name = name;
        Id = Guid.NewGuid();
    }
}
internal class Person
{
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public Person(string name, string phone, string email)
    {
        Name = name;
        Phone = phone;
        Email = email;   
    }
}
namespace ForExam
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Задача 1. Считать из строки (командной) все числа, вывести отсортированный результат в текстовый файл.");
            var numberList = new List<double>();
            double number;
            Regex regex = new Regex(@"(-){0,1}(\d){1,}([\.\,]\d{1,}){0,}", RegexOptions.IgnoreCase);
            int counter = 0;
            foreach (string arg in args)
            {
                string tmp = Regex.Replace(arg, @"\.", ",");
                MatchCollection matchCollection = regex.Matches(tmp);
                foreach (var item in matchCollection)
                {
                    counter++;
                    try
                    {
                        number = Double.Parse(item.ToString());
                        numberList.Add(number);
                    }
                    catch (Exception e00)
                    {
                        Console.WriteLine(e00.Message);
                        Console.WriteLine($"{item} не удалось преобразовать в Double.");
                    }
                }
            }
            Console.WriteLine();
            Console.Write("Исходные данные командной строки:\n");
            for (int i = 0; i < numberList.Count; i++)
            {
                Console.Write(numberList[i] + "  ");
            }
            Console.WriteLine();
            Console.Write("Отсортированный данные командной строки:\n");
            numberList.Sort();
            foreach (var num in numberList)
            {
                Console.Write(num + "  ");
            }
            Console.WriteLine();
            try
            {
                StreamWriter sw = new StreamWriter(("sorted_command_line_data.txt"), true);
                Console.WriteLine("\nДанные записаны в файл " + "sorted_command_line_data.txt");
                for (int i = 0; i < numberList.Count; i++)
                {
                    sw.Write(numberList[i] + "  ");
                }
                sw.WriteLine();
                numberList.Clear();
                sw.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 2. Из пользовательского ввода создать массив/список записей в учебном журнале вида\n" +
                "\"номер_п/п;ФИО ученика;Оценка\", результат записать в текстовый файл.");
            var nameList = new List<string>();
            var gradeList = new List<int>();
            var regexStopEnter = new Regex(@"(END_INPUT)", RegexOptions.IgnoreCase);
            int counterStopEnter = 0;
            string nameSudent;
            int grade;
            bool success = false;   
            while (counterStopEnter == 0)               
            {
                Console.WriteLine("Для заверщения ввода введите -> END_INPUT");
                Console.WriteLine("Введите ФИО ученика: ");    
                string str_name = Console.ReadLine();
                grade = 0;
                MatchCollection matchesStopEnter = regexStopEnter.Matches(str_name);
                foreach (Match match in matchesStopEnter)
                {
                    counterStopEnter++;
                }
                if (counterStopEnter == 0)
                {
                    nameSudent = str_name;
                    nameList.Add(nameSudent);
                    while (((grade < 1) | (grade > 12)) & counterStopEnter == 0)
                    {
                        Console.WriteLine("Введите оценку от 1 до 12: ");
                        str_name = Console.ReadLine();
                        matchesStopEnter = regexStopEnter.Matches(str_name);
                        foreach (Match match in matchesStopEnter)
                        {
                            counterStopEnter++;                            
                        }
                        if (counterStopEnter == 0)
                        {
                            success = int.TryParse(str_name, out grade);
                            if (success && (grade >= 1) && (grade <= 12))
                            {
                                Console.WriteLine($"Оценка {grade} введена корректно!");   
                                gradeList.Add(grade);
                                
                            }
                            else
                            {
                                Console.WriteLine("Оценка введена некорректно!");
                            }
                        }
                        else
                        {
                            grade = 0;
                            gradeList.Add(grade);
                        }
                    }                    
                }                
            }
            Console.WriteLine(new string('-', 6 + 50 + 6 + 4));
            Console.WriteLine($"|{"№ п/п",-6}|{"ФИО",-50}|{"Оценка",-6}|");
            Console.WriteLine(new string('-', 6 + 50 + 6 + 4));
            for (int i = 0; i < nameList.Count; i++)
            {
                Console.WriteLine($"|{(i + 1),-6}|{nameList[i],-50}|{gradeList[i],-6}|");
                Console.WriteLine(new string('-', 6 + 50 + 6 + 4));
            }
            Console.ReadKey();
            try
            {
                StreamWriter sw = new StreamWriter(("study_journal.txt"), true);
                Console.WriteLine("\nДанные записаны в файл " + "study_journal.txt");
                sw.WriteLine(new string('-', 6 + 50 + 6 + 4));
                sw.WriteLine($"|{"№ п/п",-6}|{"ФИО",-50}|{"Оценка",-6}|");
                sw.WriteLine(new string('-', 6 + 50 + 6 + 4));
                for (int i = 0; i < nameList.Count; i++)
                {
                    sw.WriteLine($"|{(i + 1),-6}|{nameList[i],-50}|{gradeList[i],-6}|");
                    sw.WriteLine(new string('-', 6 + 50 + 6 + 4));
                }
                sw.WriteLine();
                nameList.Clear();
                gradeList.Clear();  
                sw.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 3. Создать из пользовательского ввода сущность (класс или структуру) для хранения контактной информации.\n" +
                "Пользователь в произвольном порядке вводит телефон, имя и адрес электронной почты,\n" +
                "программа при помощи регулярных выражений добавляет эти данные в сущность и выводит на экран.");
            var regexpName = new Regex(@"([A-Я]{1}[а-яё]{1,23}|[A-Z]{1}[a-z]{1,23})");
            var regexpEmail = new Regex(@"([a-яA-Я0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6})");
            var regexPhone = new Regex(@"(8|\+7)(\d){10}");                   
            string name = string.Empty;
            string email = string.Empty;
            string phone = string.Empty;       
            Console.WriteLine("Ведите через пробел в любом порядке:\nимя, номер телефона в формате +7XXXXXXXXXX или 8XXXXXXXXXX и адрес электронной почты:");
            string str = Console.ReadLine();
            MatchCollection matchesName = regexpName.Matches(str);
            if (matchesName.Count > 0)
            {
                foreach (Match match in matchesName)
                {
                    name = matchesName[0].ToString();                        
                }
            }
            else
            {
                name = "default_name";                    
            }              
            MatchCollection matchesEmail = regexpEmail.Matches(str);
            if (matchesEmail.Count > 0)
            {
                foreach (Match match in matchesEmail)
                {
                    email = matchesEmail[0].ToString();                        
                }
            }
            else
            {
                email = "default_email";                    
            }   
            MatchCollection matchesPhone = regexPhone.Matches(str);
            if (matchesPhone.Count > 0)
            {
                foreach (Match match in matchesPhone)
                {
                    phone = matchesPhone[0].ToString();                          
                }
            }
            else
            {
                phone = "default_phone";                    
            }                
            Person person = new Person(name, phone, email);   
            Console.WriteLine($"Имя {person.Name} тел.: {person.Phone}, Email: {person.Email}");
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 4. Из пользовательского ввода создать две строки - в одной все числа, в другой символы, не являющиеся числами. Результат сохранить в файл.");
            Console.WriteLine("Введите строку (числа и символы):");    
            string strNum = "";
            string strChar = "";
            string sentence = Console.ReadLine(); 
            string patternNum = @"(-){0,1}(\d){1,}(\s*)([\.\,]\d{1,}){0,}";
            string patternChar = @"\D";
            MatchCollection matchesNum = Regex.Matches(sentence, patternNum);
            for (int ctr = 0; ctr < matchesNum.Count; ctr++)
            {
                strNum += matchesNum[ctr].Value + " ";
            }
            Console.WriteLine($"Числа: {strNum}");
            MatchCollection matchesChar = Regex.Matches(sentence, patternChar);
            for (int ctr = 0; ctr < matchesChar.Count; ctr++)
            {
                strChar += matchesChar[ctr].Value;
            }
            Console.WriteLine($"Символы: {strChar}");
            try
            {
                StreamWriter sw = new StreamWriter(("num_char.txt"), true);
                Console.WriteLine("\nДанные записаны в файл " + "num_char.txt");                               
                sw.WriteLine($"Числа: {strNum}");
                sw.WriteLine($"Символы: {strChar}");
                sw.WriteLine();
                sw.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 5. Написать программу выводящую системное название типа long и местоположение папок \"Документы\" и \"Cookies\" в текстовый файл.");
            long x = 0;
            string systemTypeLong = x.GetType().ToString();
            Console.WriteLine($"Системное название типа long -> {systemTypeLong}");
            string myDoc = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);            
            Console.WriteLine($"Местоположение папки \"Документы\": {myDoc}");
            string myCookies = Environment.GetFolderPath(Environment.SpecialFolder.Cookies);
            Console.WriteLine($"Местоположение папки \"Cookies\": {myCookies}");
            try
            {
                StreamWriter sw = new StreamWriter(("systemType_folderLocation.txt"), true);
                Console.WriteLine("\nДанные записаны в файл " + "systemType_folderLocation.txt");
                sw.WriteLine($"Системное название типа long -> {systemTypeLong}");
                sw.WriteLine($"Местоположение папки \"Документы\": {myDoc}");
                sw.WriteLine($"Местоположение папки \"Cookies\": {myCookies}");
                sw.WriteLine();                
                sw.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 6. Первый аргумент командной строки задаёт имя, второй -порядок вывода имени(-обратный).\r\n" +
                "Если аргументов нет или недостаточно, то запросить у пользователя. Использовать механизм обработки исключений.");
            string name_input = "";
            string nameOutputOrder = "";
            try
            {
                if (args.Length > 1 & args.Length <= 2)
                {
                    name_input = args[0];
                    nameOutputOrder = args[1];
                }
                else  
                    if (args.Length > 0 & args.Length <= 1)
                {
                    name_input = args[0];
                    Console.WriteLine($"Имя {name_input}");
                    Console.WriteLine("Введите порядок вывода имени:");
                    nameOutputOrder = Console.ReadLine();
                }                    
                else
                {
                    Console.WriteLine("Введите имя:");
                    name_input = Console.ReadLine();
                    Console.WriteLine("Введите порядок вывода имени:");
                    nameOutputOrder = Console.ReadLine();
                }     
            }
            catch
            {
                Console.WriteLine("\nException: аргумены отсутствуют!");
            }
            Console.WriteLine($"Имя {name_input}, порядок вывода {nameOutputOrder}");
            Console.Write($"Обратный вывод имени: ");
            for (int i = name_input.Length-1; i >= 0; i--)
            {
                Console.Write($"{name_input[i]}");
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 7. Реализовать интерфейс, позволяющий задать имя текстового файла и регулярное выражение для поиска.\n" +
                "Результат работы программы вывод в консоль всех совпадений.");  
            Console.WriteLine("Введите путь к файлу: ");
            string inputPath = Console.ReadLine();
            Console.WriteLine("Введите регулярное выражение для поиска: ");
            string regularExpressionToSearch = Console.ReadLine();
            Regex regex_input = new Regex(regularExpressionToSearch);
            string str_match = "";
            try
            {
                StreamReader sr = new StreamReader(inputPath);
                for (int i = 0; !sr.EndOfStream; i++)
                {
                    str_match += sr.ReadLine();
                }
                MatchCollection matchs = regex_input.Matches(str_match);                
                int count = 0;
                foreach (var item in matchs)
                {
                    Console.WriteLine($"Совпадение номер {count} {item}");
                    count++;
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 8. Создать класс со счётчиком. Каждый экземпляр класса должен получать свой уникальный номер.");
            MyClass my = new MyClass("Igor");
            Console.WriteLine($"Уникальный номер {my.UnicalId}, Имя {my.Name}");
            MyClass my2 = new MyClass("Ivan");
            Console.WriteLine($"Уникальный номер {my2.UnicalId}, Имя {my.Name}");
            Console.WriteLine();
            Console.ReadKey();

            Console.WriteLine("Задача 9. Из аргументов командной строки получить текстовый файл, упорядоченный как в словаре: лексикографически.");   
            var wordList = new List<string>();
            try
            {
                if (args.Length > 0)
                {
                    foreach (string word in args)
                    {
                        wordList.Add(word);
                    }
                }
                else
                {
                    Console.WriteLine("\nException: аргумены отсутствуют!");
                }
            }
            catch
            {
                Console.WriteLine("\nException: аргумены отсутствуют!");
            }
            Console.WriteLine();
            Console.Write("Исходные данные командной строки:\n");
            for (int i = 0; i < wordList.Count; i++)
            {
                Console.Write(wordList[i] + "  ");
            }
            Console.WriteLine();
            Console.Write("Отсортированный данные командной строки:\n");
            wordList.Sort();
            foreach (var word in wordList)
            {
                Console.Write(word + "  ");
            }
            Console.WriteLine();
            try
            {
                StreamWriter sw = new StreamWriter(("ordere_as_in_a_dictionary.txt"), true);
                Console.WriteLine("\nДанные записаны в файл " + "ordere_as_in_a_dictionary.txt");
                for (int i = 0; i < wordList.Count; i++)
                {
                    sw.WriteLine(wordList[i] + "  ");
                }
                sw.WriteLine();
                wordList.Clear();
                sw.Close();
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException: " + e.Message);
            }            
        }
    }
}