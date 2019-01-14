using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelephoneBook
{
    public class Record
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, {nameof(Phone)}: {Phone}";
        }
    }
    public abstract class Command
    {
        protected readonly CommandLoop _loop;
        protected Command(CommandLoop loop)
        {
            _loop = loop;
        }
        public abstract void Do(string commandArgs);
        public abstract string Name { get; }

        protected string RemovePrefix(string line)
        {
            Debug.Assert(line.StartsWith(Name, StringComparison.CurrentCultureIgnoreCase));

            if (line.Length > Name.Length)
            {
                return line.Substring(Name.Length + 1);
            }
            return "";
        }
    }

    public  abstract class CommandLoop
    {
        public bool Stop { get; set; }

        public List<Record> Records { get; set; } = new List<Record>();

        protected readonly CommandFactory _commandFactory;

        public CommandLoop()
        {
            _commandFactory = new CommandFactory(this);
        }

        public abstract void DoLoop();

        public abstract void WriteLine(string line);

        public abstract void ReadKey();


    }
    public class ConsoleCommandLoop : CommandLoop
    {

        public override void DoLoop()
        {
            while (!Stop)
            {
                var line = Console.ReadLine();
                var command = _commandFactory.GetCommand(line);

                command.Do(line);
            }
        }

        public override void WriteLine(string line)
        {
            Console.WriteLine(line);
        }

        public override void ReadKey()
        {
            Console.ReadKey();
        }
    }
    public class CommandFactory
    {
        private readonly CommandLoop _loop;
        public CommandFactory(CommandLoop loop)
        {
            _loop = loop;
        }
        public Command GetCommand(string line)
        {
            if (line.StartsWith("Выход", StringComparison.CurrentCultureIgnoreCase))
                return new ExitCommand(_loop);

            if (line.StartsWith("Добавить", StringComparison.CurrentCultureIgnoreCase))
                return new AddCommand(_loop);

            if (line.StartsWith("Найти", StringComparison.CurrentCultureIgnoreCase))
                return new SearchCommand(_loop);

            if (line.StartsWith("Удалить", StringComparison.CurrentCultureIgnoreCase))
                return new DeleteCommand(_loop);

            return new UnknownCommand(_loop);
        }
    }
    public class DeleteCommand : Command
    {
        public DeleteCommand(CommandLoop loop) : base(loop)
        { }
        
        public override void Do(string commandargs)
        {
            var arg = RemovePrefix(commandargs);
            var countDeleted = 0;
            var phoneLength = arg.IndexOf(" ");
            if (phoneLength == -1)
            {
                var searchResult = _loop.Records.Where(r => r.Name.Equals(arg, StringComparison.CurrentCultureIgnoreCase) || r.Phone.Equals(arg, StringComparison.CurrentCultureIgnoreCase));
                foreach (var record in searchResult.ToArray())
                {
                    Console.WriteLine("Удалена запись \"" + record.ToString() + "\"");
                    _loop.Records.Remove(record);
                    countDeleted++;
                }
            }
            else
            {
                var phone = arg.Substring(0, phoneLength);
                var name = arg.Substring(phoneLength + 1);
                var searchResult = _loop.Records.Where(r => r.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) && r.Phone.Equals(phone, StringComparison.CurrentCultureIgnoreCase));
                foreach (var record in searchResult.ToArray())
                {
                    Console.WriteLine("Удалена запись \"" + record.ToString() + "\"");
                    _loop.Records.Remove(record);
                    countDeleted++;
                }
            }
            if (countDeleted == 0)
            {
                _loop.WriteLine("Не найдено записей для удаления");
            }
            else
                _loop.WriteLine("Всего удалено записей из справочника: " + countDeleted);
        }

        public override string Name => "Удалить";
    }

    public class SearchCommand : Command
    {
        public SearchCommand(CommandLoop loop) : base(loop)
        {
        }

        public override void Do(string commandargs)
        {
            bool somethingfound = false;
            bool f = false;
            var arg = RemovePrefix(commandargs);
            var phoneLength = arg.IndexOf(" ");
            if (phoneLength == -1)
            {
                var searchResult = _loop.Records.Where(r => r.Name.Contains(arg) || r.Phone.Contains(arg));
                foreach (var record in searchResult)
                {
                    somethingfound = true;
                    if (!f)
                    {
                        Console.WriteLine("Найденные записи по запросу \"" + arg + "\":");
                        f = true;
                    }
                    _loop.WriteLine(record.ToString());
                }
            }
            else
            {
                var phone = arg.Substring(0, phoneLength);
                var name = arg.Substring(phoneLength + 1);
                var searchResult = _loop.Records.Where(r => r.Name.Contains(name) || r.Phone.Contains(phone));
                foreach (var record in searchResult)
                {
                    somethingfound = true;
                    if (!f)
                    {
                        Console.WriteLine("Найденные записи по запросу \"" + arg + "\":");
                        f = true;
                    }
                    _loop.WriteLine(record.ToString());
                }
            }
            if (!somethingfound)
                _loop.WriteLine("Ничего не найдено");
        }

        public override string Name { get { return "Найти"; } }
    }

    public class AddCommand : Command
    {
        public AddCommand(CommandLoop loop) : base(loop)
        {
        }
        
        public override void Do(string commandargs)
        {
            var arg = RemovePrefix(commandargs);
            var phoneLength = arg.IndexOf(" ");

            if (phoneLength == -1 || phoneLength == arg.Length + 1)
            {
                Console.WriteLine("Чтобы добавить контакт, введите номер телефона и имя абонента!");
            }
            else
            {
                var phone = arg.Substring(0, phoneLength);
                var name = arg.Substring(phoneLength + 1);
                _loop.Records.Add(new Record
                {
                    Name = name,
                    Phone = phone
                });
            }
        }

        public override string Name => "Добавить";
    }

    public class UnknownCommand : Command
    {
        public UnknownCommand(CommandLoop loop) : base(loop)
        { }

        public override void Do(string commandargs)
        {
            _loop.WriteLine("Команда непонятна, попробуйте еще раз");
        }

        public override string Name { get { throw new NotSupportedException(); } }
    }

    public class ExitCommand : Command
    {
        public ExitCommand(CommandLoop loop) : base(loop)
        { }
        public override void Do(string commandArgs)
        {
            _loop.Stop = true;
            _loop.WriteLine("Для завершения программы нажмите любую клавишу");
            _loop.ReadKey();
        }

        public override string Name { get { throw new NotSupportedException(); } }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Калугина Дарья, гр. РИ-250004. Телефонный справочник.\nПрограмма принимает команды в следующем формате:\nДобавить Телефон ФИО\nНайти Текст для поиска\nУдалить Телефон/ФИО\nВыход");
            new ConsoleCommandLoop().DoLoop();
        }
    }
}
