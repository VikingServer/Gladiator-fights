class Program
{
    public static void Main(string[] args)
    {
        Game game = new Game();

        game.Work();
    }
}

abstract class Fighter
{
    protected int Armor;

    public Fighter(string name, double health, double damage, int armor)
    {
        Name = name;
        Health = health;
        Damage = damage;
        Armor = armor;
    }

    public string Name { get; protected set; }
    public double Health { get; protected set; }
    public double Damage { get; protected set; }

    public virtual void ShowStats()
    {
        Console.WriteLine($"{Name}, здоровье: {Health}, наносимый урон: {Damage}, броня: {Armor}");
    }

    public void ShowCurrentHealth()
    {
        Console.WriteLine($"\n{Name} здоровье: {Health}");
    }

    public virtual void TakeDamage(double damage)
    {
        double percent = 100;

        Health -= damage - (damage / percent) * Armor;

        ShowCurrentHealth();
    }

    public virtual void Attack(Fighter enemy)
    {
        UseAbility();

        enemy.TakeDamage(Damage);
    }

    public abstract void UseAbility();
}

class Knight : Fighter
{
    public Knight(string name = "Рыцарь") : base(name, 0, 0, 0)
    {
        int minHealth = 1500;
        int maxHealth = 2000;

        int minDamage = 200;
        int maxDamage = 250;

        int minArmor = 50;
        int maxArmor = 80;

        Random random = new Random();

        Health = random.Next(minHealth, maxHealth + 1);
        Damage = random.Next(minDamage, maxDamage + 1);
        Armor = random.Next(minArmor, maxArmor + 1);
    }

    public override void TakeDamage(double damage)
    {
        base.TakeDamage(damage);
    }

    public override void UseAbility()
    {
        DoCriticalAttack();
    }

    public void DoCriticalAttack()
    {
        int percentCriticalStrike;
        int percent = 100;

        if (TryDoCriticalAttack(out percentCriticalStrike))
        {
            Damage = Damage + Damage / percent * percentCriticalStrike;

            Console.WriteLine("Нанесен критический урон");
        }
    }

    private bool TryDoCriticalAttack(out int percentCriticalStrike)
    {
        Random random = new Random();

        int minPercent = 0;
        int maxPercent = 100;

        int minPercentCriticalStrike = 5;
        int maxPercentCriticalStrike = 40;

        percentCriticalStrike = random.Next(minPercentCriticalStrike, maxPercentCriticalStrike + 1);

        return percentCriticalStrike > random.Next(minPercent, maxPercent + 1);
    }
}

class Goblin : Fighter
{
    private int _countHit;

    public Goblin(string name = "Гоблин") : base(name, 0, 0, 0)
    {
        int minHealth = 1000;
        int maxHealth = 1800;

        int minDamage = 150;
        int maxDamage = 300;

        int minArmor = 20;
        int maxArmor = 60;

        Random random = new Random();

        Health = random.Next(minHealth, maxHealth + 1);
        Damage = random.Next(minDamage, maxDamage + 1);
        Armor = random.Next(minArmor, maxArmor + 1);
    }

    public override void TakeDamage(double damage)
    {
        base.TakeDamage(damage);
    }

    public override void UseAbility()
    {
        if (TryUseAbility())
        {
            DoDoubleDamage();

            Console.WriteLine("Нанесен двойной урон");
        }
    }

    private void DoDoubleDamage()
    {
        int doubleDamage = 2;

        Damage *= doubleDamage;

        _countHit = 0;
    }

    private bool TryUseAbility()
    {
        int maxCountHit = 3;

        _countHit++;

        return maxCountHit == _countHit;
    }
}

class Gnome : Fighter
{
    private double _rage;
    private int _maxRange = 15;
    private double _startHealth;

    public Gnome(string name = "Гном") : base(name, 0, 0, 0)
    {
        int minHealth = 1800;
        int maxHealth = 2000;

        int minDamage = 100;
        int maxDamage = 170;

        int minArmor = 80;
        int maxArmor = 100;

        Random random = new Random();

        Health = random.Next(minHealth, maxHealth + 1);
        Damage = random.Next(minDamage, maxDamage + 1);
        Armor = random.Next(minArmor, maxArmor + 1);

        _startHealth = Health;
    }

    public override void TakeDamage(double damage)
    {
        base.TakeDamage(damage);

        HoardiRage(damage);
    }

    public override void UseAbility()
    {
        if (TryUseAbility())
        {
            Heal();

            Console.WriteLine("Здоровье пополненно");
        }
    }

    private void HoardiRage(double damage)
    {
        double percent = 100;

        _rage += damage / percent;
    }

    private bool TryUseAbility()
    {
        return _rage >= _maxRange;
    }

    private void Heal()
    {
        Random random = new Random();
        int minReplenishedHealth = 100;
        int maxReplenishedHealth = 300;

        double replenishedHealth = random.Next(minReplenishedHealth, maxReplenishedHealth);

        if (TryHeal(replenishedHealth) == false)
            Health += replenishedHealth;

        _rage = 0;
    }

    private bool TryHeal(double replenishedHealth)
    {
        return _startHealth < replenishedHealth + Health;
    }
}

class Magician : Fighter
{
    private int _mana;
    private double _damageOfFireball;
    private int _countManaForFireball = 10;

    public Magician(string name = "Маг") : base(name, 0, 0, 0)
    {
        int minHealth = 1900;
        int maxHealth = 2100;

        int minDamage = 90;
        int maxDamage = 180;

        int minArmor = 1;
        int maxArmor = 30;

        int minMana = 90;
        int maxMana = 100;

        Random random = new Random();

        Health = random.Next(minHealth, maxHealth + 1);
        Damage = random.Next(minDamage, maxDamage + 1);
        Armor = random.Next(minArmor, maxArmor + 1);
        _mana = random.Next(minMana, maxMana);

        _damageOfFireball = Damage * 2 + _mana;
    }

    public override void ShowStats()
    {
        Console.WriteLine($"{Name}, здоровье: {Health}, наносимый урон: {Damage}, броня: {Armor}, мана {_mana}");
    }

    public override void Attack(Fighter enemy)
    {
        if (TryUseAbility())
        {
            Сonjure(enemy);

            Console.Write("Удар fireball");

            enemy.TakeDamage(Damage);
        }
        else
        {
            enemy.TakeDamage(Damage);
        }
    }

    public override void UseAbility() { }

    private bool TryUseAbility()
    {
        return _mana > _countManaForFireball;
    }

    private void Сonjure(Fighter enemy)
    {
        _mana -= _countManaForFireball;

        enemy.TakeDamage(_damageOfFireball);
    }
}

class Elf : Fighter
{
    public Elf(string name = "Эльф") : base(name, 0, 0, 0)
    {
        int minHealth = 1500;
        int maxHealth = 1900;

        int minDamage = 250;
        int maxDamage = 300;

        int minArmor = 50;
        int maxArmor = 70;

        Random random = new Random();

        Health = random.Next(minHealth, maxHealth + 1);
        Damage = random.Next(minDamage, maxDamage + 1);
        Armor = random.Next(minArmor, maxArmor + 1);
    }

    public override void TakeDamage(double damage)
    {
        if (TryDodgeDamage())
            Console.WriteLine("\nУклон от атаки соперника");
        else
            base.TakeDamage(damage);
    }

    public override void UseAbility() { }

    private bool TryDodgeDamage()
    {
        Random random = new Random();

        int chanceDodge = 50;

        int minPercent = 0;
        int maxPercent = 100;

        return chanceDodge > random.Next(minPercent, maxPercent + 1);
    }
}

class Game
{
    private Fighter _firstFighter;
    private Fighter _secondFighter;
    private bool _isActive;

    public void Work()
    {
        const string CommandChooseFirstFighter = "1";
        const string CommandChooseSecondFighter = "2";
        const string CommandStartBattle = "3";
        const string CommandShowAllFighters = "4";
        const string CommandExit = "5";

        _isActive = true;

        while (_isActive)
        {
            Console.WriteLine($"\n{CommandChooseFirstFighter} - выбрать первого бойца" +
                              $"\n{CommandChooseSecondFighter} - выбрать второго бойца\n{CommandStartBattle} - начать битву" +
                              $"\n{CommandShowAllFighters} - показать всех бойцов\n{CommandExit} - выход из программы");

            Console.Write("\nВыберите команду: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case CommandChooseFirstFighter:
                    TryGetFighter(FillFighters(), out _firstFighter);
                    break;

                case CommandChooseSecondFighter:
                    TryGetFighter(FillFighters(), out _secondFighter);
                    break;

                case CommandStartBattle:
                    TryFight();
                    break;

                case CommandShowAllFighters:
                    ShowAllFighters(FillFighters());
                    break;

                case CommandExit:
                    LeaveProgram();
                    break;

                default:
                    Console.WriteLine("Такой команды нет");
                    break;
            }
        }
    }

    private Fighter[] FillFighters()
    {
        Fighter[] fighters = { new Knight(), new Goblin(), new Gnome(), new Magician(), new Elf() };

        return fighters;
    }

    private int GetInt(string userInput)
    {
        int number;

        while (int.TryParse(userInput, out number) == false)
        {
            Console.Write("Введите число: ");
            userInput = Console.ReadLine();
        }

        return number;
    }

    private void ShowAllFighters(Fighter[] fighters)
    {
        Console.WriteLine();

        for (int i = 0; i < fighters.Length; i++)
        {
            Console.Write(i + 1 + " ");
            fighters[i].ShowStats();
        }
    }

    private bool TryGetFighter(Fighter[] fighters, out Fighter fighter)
    {
        ShowAllFighters(fighters);

        int userNumberFighter = GetNumberOfFighter();


        if (userNumberFighter >= 0 && userNumberFighter <= fighters.Length)
        {
            fighter = fighters[userNumberFighter];

            Console.WriteLine("Боец выбран");

            return true;
        }
        else
        {
            fighter = null;

            Console.WriteLine("Такого бойца нет");

            return false;
        }
    }

    private int GetNumberOfFighter()
    {
        Console.Write("\nВведите номер бойца: ");
        string userInput = Console.ReadLine();

        int userNumberFighter = GetInt(userInput) - 1;

        return userNumberFighter;
    }

    private void TryFight()
    {
        if (_firstFighter != null && _secondFighter != null)
        {
            Console.WriteLine(new string('-', 15) + "Бой" + new string('-', 15));
            Fight();
        }
        else
        {
            if (_firstFighter == null)
                Console.WriteLine("\nПервый боец не выбран");
            else if (_secondFighter == null)
                Console.WriteLine("\nВторой боец не выбран");
        }
    }

    private void Fight()
    {
        Console.WriteLine("\nБойцы: ");

        _firstFighter.ShowStats();
        _secondFighter.ShowStats();

        int timeAtack = 3000;

        while (_firstFighter.Health > 0 && _secondFighter.Health > 0)
        {
            _firstFighter.Attack(_secondFighter);

            _secondFighter.Attack(_firstFighter);

            Thread.Sleep(timeAtack);
        }

        DetermineWinner();
    }

    private void DetermineWinner()
    {
        if (_firstFighter.Health > 0)
            Console.WriteLine($"{_firstFighter.Name} - победитель");
        else if (_secondFighter.Health > 0)
            Console.WriteLine($"\n{_secondFighter.Name} - победитель");
        else
            Console.WriteLine("Оба бойца погибли");
    }

    private void LeaveProgram()
    {
        Console.WriteLine("Конец программы");

        _isActive = false;
    }
}