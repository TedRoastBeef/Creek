using System;
using Creek.Database;

namespace DatabaseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            const string dbFileName = "game.db";

            const double mageAttackValue = 3.3;
            const double mageDefenseValue = 3.4;

            const double warriorAttackValue = 4.4;
            const double warriorDefenseValue = 2.2;

            // create two objects
            IHero mage = new Mage("Merlin", mageAttackValue, mageDefenseValue);
            IHero warrior = new Warrior("Conan", warriorAttackValue, warriorDefenseValue);

            // store them
            using (var odb = OdbFactory.Open(dbFileName))
            {
                odb.Store(mage);
                odb.Store(warrior);
            }

            // retrieve them by classes and by interface
            using (var odb = OdbFactory.Open(dbFileName))
            {
                // work with mages
                var mages = odb.Query<Mage>().Execute<Mage>(true);
                foreach (var hero in mages)
                    Console.WriteLine(hero);

                // work with warriors
                var warriors = odb.Query<Warrior>().Execute<Warrior>(true);
                foreach (var hero in warriors)
                    Console.WriteLine(hero);

                Console.WriteLine("Start working with IHero interface.");

                // work with heroes
                var heroes = odb.Query<IHero>().Execute<IHero>(true);
                foreach (var hero in heroes)
                    Console.WriteLine(hero);

                Console.ReadLine();
            }
        }
    }
}
