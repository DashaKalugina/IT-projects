using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelephoneBook;

namespace TelephoneBookTest
{
    [TestFixture]
    public class CommandTests
    {
        [Test]
        public void CreateCommandFactoryTest()
        {
            new CommandFactory(new TestCommandLoop());
        }

        [Test]
        public void AddContactTest()
        {
            ObjectMother.Reset();
            ObjectMother.CommandFactory.GetCommand("Добавить 03 Скорая").Do("Добавить 03 Скорая");
            Assert.That(ObjectMother.Loop.Records.Any(r => r.Name == "Скорая"));
            Assert.That(ObjectMother.Loop.Records.Any(r => r.Phone == "03"));
        }
        [Test]
        public void AddAndSearchContactTest()
        {
            ObjectMother.Reset();
            ObjectMother.CommandFactory.GetCommand("добавить 9090287694 Бабушка").Do("добавить 9090287694 Бабушка");
            ObjectMother.CommandFactory.GetCommand("Найти Бабушка").Do("Найти Бабушка");
            Assert.That(ObjectMother.Loop.Lines.Any(l => l.Contains("Бабушка")));
        }
        [Test]
        public void AddAndRemoveContactTest()
        {
            ObjectMother.Reset();
            ObjectMother.CommandFactory.GetCommand("Добавить 3434567859 Ростелеком").Do("Добавить 3434567859 Ростелеком");
            ObjectMother.CommandFactory.GetCommand("Удалить Ростелеком").Do("Удалить Ростелеком");
            Assert.That(ObjectMother.Loop.Lines.All(l => !l.Contains("Ростелеком")));

            ObjectMother.CommandFactory.GetCommand("Добавить 03 Скорая").Do("Добавить 03 Скорая");
            ObjectMother.CommandFactory.GetCommand("Удалить 03 Скорая").Do("Удалить 03 Скорая");
            Assert.That(ObjectMother.Loop.Lines.All(l => !l.Contains("Скорая")));
        }

        [Test]
        public void UnknownCommandTest()
        {
            ObjectMother.Reset();
            ObjectMother.CommandFactory.GetCommand("fhfjh48494").Do("fhfjh48494");
            Assert.That(ObjectMother.Loop.Lines.Any());
        }

        [Test]
        public void ExitFromProgramTest()
        {
            ObjectMother.Reset();
            ObjectMother.CommandFactory.GetCommand("Выход").Do("Выход");
            Assert.That(ObjectMother.Loop.KeyIsAwaited);
        }
        
    }

    class ObjectMother
    {
        private static CommandFactory _factory;
        private static TestCommandLoop _loop;

        public static TestCommandLoop Loop => _loop;

        static ObjectMother()
        {
            Reset();
        }

        public static void Reset()
        {
            _loop = new TestCommandLoop();
            _factory = new CommandFactory(_loop);
        }

        public static CommandFactory CommandFactory => _factory;
    }

    class TestCommandLoop : CommandLoop
    {
        public override void DoLoop()
        {
            throw new NotSupportedException("Вызов цикла из тестов не предусмотрен");
        }

        public bool KeyIsAwaited { get; set; }

        public override void ReadKey()
        {
            KeyIsAwaited = true;
        }

        public List<string> Lines { get; } = new List<string>();

        public override void WriteLine(string line)
        {
            Lines.Add(line);
        }
    }
}
