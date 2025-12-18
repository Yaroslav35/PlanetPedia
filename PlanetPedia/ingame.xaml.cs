using System.Collections.Immutable;

namespace PlanetPedia;

public partial class ingame : ContentPage
{
    private bool vibro;
    private Random rand = new Random();
    private int time;
    private int loop = 1;
    private bool running = false;
    private bool time_run = false;
    private int mylifes = 2;
    private int botlifes = 2;
    private int stars;
    private int energy = 10;
    private int target = 0;
    private bool myTurn = true;
    private int loses = 0;

    private List<Hero?> enemyheroes = new List<Hero?>() { null, null, null };
    private List<Sup?> enemysupes = new List<Sup?>() { null, null, null };

    private List<string> heros = new List<string>()
    {
        "1:Астронавт:80:20:150:30:3:astronaut.png",
        "2:Солнце:40:50:60:75:3:sunc.png",
        "3:Луна:150:5:225:8:3:moonc.png",
        "4:Венера:60:40:90:60:3:venusc.png",
        "5:Юпитер:180:3:270:5:4:jupiterc.png",
        "6:Плутон:40:70:60:105:5:pluto.png",
        "7:Пульсар:100:0:150:0:6:netron.png",
        "8:Сверхновая:90:20:135:30:5:supernova.png",
        "9:Экзопланета:30:15:45:22:4:exoplanet.png",
        "10:Пиньята:90:30:135:45:6:pinata.png",
        "11:Кварк:200:80:300:120:3:quark.png",
        "12:Дискошар:95:0:140:0:6:disco.png",
        "13:Тыква:110:1:165:80:5:pumpkin.png",
        "14:Мрачник:80:30:120:45:4:gloomy.png",
        "15:Призрак:70:25:105:38:5:ghost.png",
        "16:Водяная планета:60:30:90:45:4:waterplanet.png",
        "17:Комета:130:40:195:60:5:cometc.png",
        "18:Кокос:150:10:225:15:6:cocos.png",
        "19:Песчаный:100:30:150:45:4:sandy.png",
        "20:Снежная планета:100:5:150:8:6:snow.png",
        "21:Ёлка:80:10:120:15:6:wood.png"
    };

    private List<string> sups = new List<string>()
    {
        "1:Исцеление:2:heal.png",
        "2:Урон:3:damage.png",
        "3:Смесь:5:healdamage.png",
        "4:Ослабление:5:weak.png",
        "5:Доп.Жизнь:8:extralife.png",
        "6:Большое исцеление:4:bigheal.png",
        "7:Чёрная дыра:7:blackhole.png",
        "8:Галактика:6:galaxy.png",
        "9:Экскалибур:6:excalibur.png",
        "10:Антиматерия:3:antimatter.png",
        "11:Странное зелье:3:strange.png",
        "12:Зелье радиации:5:radiation.png",
        "13:Мрачное зелье:999:disablepotion.png",
        "14:Монета обмена:6:change.png",
        "15:Конфеты:5:candies_pumpkin.png",
        "16:Подарок:5:gift.png",
        "17:Зелье ярости:4:rage.png"
    };

    private List<Hero?> heroes = new List<Hero?>() { null, null, null };
    private List<Sup?> supes = new List<Sup?>() { null, null, null };

    bool parts = false;

    // Словарь для быстрого доступа к картам по ID
    private Dictionary<int, string> heroDictionary = new Dictionary<int, string>();
    private Dictionary<int, string> supDictionary = new Dictionary<int, string>();

    public ingame(int stars_get, bool parts_get)
    {
        InitializeComponent();
        stars = stars_get;
        parts = parts_get;
        vibro = Preferences.Get("vibro", true);

        if(stars > 3000 && stars <= 4000)
        {
            botlifes = 3;
            mylifes = 3;
        }
        else if(stars > 4000 && stars <= 5000)
        {
            botlifes = 4;
            mylifes = 4;
        }
        else if(stars > 5000)
        {
            botlifes = 5;
            mylifes = 5;
        }

            // Инициализируем словари
            InitializeDictionaries();
        InitializeGame();
    }

    private void InitializeDictionaries()
    {
        // Инициализируем словарь героев
        foreach (var hero in heros)
        {
            string[] data = hero.Split(':');
            int id = int.Parse(data[0]);
            heroDictionary[id] = hero;
        }

        // Инициализируем словарь зелий
        foreach (var sup in sups)
        {
            string[] data = sup.Split(':');
            int id = int.Parse(data[0]);
            supDictionary[id] = sup;
        }
    }

    private void InitializeGame()
    {
        switch_card();
        Update();
        timer_control();

        // Получаем случайный ID героя, а не индекс
        int heroId = GetRandomHeroId();
        string heroData = heroDictionary[heroId];
        string[] data = heroData.Split(":");
        hero1.BackgroundColor = Colors.LightPink;

        var initialHero = new Hero(data[1], int.Parse(data[2]), int.Parse(data[3]),
                           int.Parse(data[4]), int.Parse(data[5]), 0,
                           int.Parse(data[6]), true, data[7]);

        CheckRageAbility(initialHero);
        heroes[0] = initialHero;

        UpdateTargetHighlight();
        draw();
    }

    private int GetRandomHeroId()
    {
        List<int> availableIds = new List<int>();

        // Определяем доступные ID героев в зависимости от звёзд
        if (stars >= 0 && stars <= 500)
        {
            // 0-500: астронавт солнце луна (ID 1-3)
            availableIds.AddRange(new[] { 1, 2, 3 });
        }
        else if (stars > 500 && stars <= 1000)
        {
            // 501-1000: венера юпитер плутон (ID 4-6)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6 });
        }
        else if (stars > 1000 && stars <= 1500)
        {
            // 1001-1500: пульсар сверхновая экзопланета (ID 7-9)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }
        else if (stars > 1500 && stars <= 2000)
        {
            // 1501-2000: пиньята кварк дискошар (ID 10-12)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        }
        else if (stars > 2000 && stars <= 3000)
        {
            // 2001-3000: тыква мрачник призрак (ID 13-15)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 });
        }
        else if (stars > 3000 && stars <= 4000)
        {
            // 3001-4000: водная планета, комета кокос песчанный (ID 16-19)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 });
        }
        else if (stars > 4000 && stars <= 5000)
        {
            // 4001-5000: снежная планета, ёлка (ID 20-21)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 });
        }
        else
        {
            // 5000+: все карты
            availableIds.AddRange(heroDictionary.Keys);
        }

        if (availableIds.Count == 0)
            return 1; // Возвращаем ID 1 (Астронавт) по умолчанию

        return availableIds[rand.Next(availableIds.Count)];
    }

    private int GetRandomSupId()
    {
        List<int> availableIds = new List<int>();

        // Определяем доступные ID зелий в зависимости от звёзд
        if (stars >= 0 && stars <= 500)
        {
            // 0-500: исцеление урон смесь (ID 1-3)
            availableIds.AddRange(new[] { 1, 2, 3 });
        }
        else if (stars > 500 && stars <= 1000)
        {
            // 501-1000: ослабление доп жизнь большое исцеление (ID 4-6)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6 });
        }
        else if (stars > 1000 && stars <= 1500)
        {
            // 1001-1500: чёрная дыра галактика экскалибур (ID 7-9)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }
        else if (stars > 1500 && stars <= 2000)
        {
            // 1501-2000: антиматерия странное зелье и зелье радиации (ID 10-12)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        }
        else if (stars > 2000 && stars <= 3000)
        {
            // 2001-3000: монета обмена, конфеты (ID 14-15)
            // Странное зелье (ID 11) уже есть с предыдущего уровня
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 });
        }
        else if (stars > 3000 && stars <= 4000)
        {
            // 3001-4000: все предыдущие + ничего нового
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15 });
        }
        else if (stars > 4000 && stars <= 5000)
        {
            // 4001-5000: подарок (ID 16)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16 });
        }
        else if (stars > 5000)
        {
            // 5000+: все зелья включая ярость (ID 17)
            availableIds.AddRange(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 14, 15, 16, 17 });
        }

        // Всегда доступно мрачное зелье (ID 13)
        availableIds.Add(13);

        if (availableIds.Count == 0)
            return 1; // Возвращаем ID 1 (Исцеление) по умолчанию

        return availableIds[rand.Next(availableIds.Count)];
    }

    public void switch_card()
    {
        UpdateCard(buy1, hp1, dmg1, en1, img1, title1);
        UpdateCard(buy2, hp2, dmg2, en2, img2, title2);
        UpdateCard(buy3, hp3, dmg3, en3, img3, title3);
    }

    private void UpdateCard(Border border, Label hp, Label dmg, Label en, Image img, Label title)
    {
        int type = rand.Next(0, 2);

        if (type == 0)
        {
            // Для зелий
            int supId = GetRandomSupId();

            // Специальная логика для зелья ярости (только с 5000+ и 20% шанс)
            if (rand.Next(0, 5) != 0 && stars >= 5000)
            {
                if (supId == 17 && stars >= 5000)
                {
                    // Оставляем зелье ярости
                }
                else if (supId == 17)
                {
                    // Если выпало зелье ярости, но не должно было, берем другое
                    supId = GetRandomSupId();
                    // Убедимся что это не зелье ярости
                    while (supId == 17)
                    {
                        supId = GetRandomSupId();
                    }
                }
            }
            else supId = 17;

            string supData = supDictionary[supId];
            string[] data = supData.Split(':');

            border.BackgroundColor = Colors.LightGreen;
            hp.Text = "0";
            dmg.Text = "0";
            en.Text = data[2];
            img.Source = data[3];
            title.Text = data[1];
        }
        else
        {
            int heroId = GetRandomHeroId();
            string heroData = heroDictionary[heroId];
            string[] data = heroData.Split(':');

            string heroName = data[1];
            bool hasRageAbility = false;

            // Проверяем, есть ли у игрока яростная способность для этого героя (только с 5000+)
            if (stars >= 5000)
            {
                switch (heroName)
                {
                    case "Солнце":
                        hasRageAbility = Preferences.Get("sun_rage", 0) >= 200;
                        break;
                    case "Юпитер":
                        hasRageAbility = Preferences.Get("jupiter_rage", 0) >= 200;
                        break;
                    case "Пульсар":
                        hasRageAbility = Preferences.Get("netron_rage", 0) >= 200;
                        break;
                    case "Кварк":
                        hasRageAbility = Preferences.Get("quark_rage", 0) >= 200;
                        break;
                }
            }

            border.BackgroundColor = Colors.LightPink;
            hp.Text = data[2];
            dmg.Text = data[3];
            en.Text = data[6];
            img.Source = data[7];

            if (hasRageAbility)
            {
                title.Text = $"{heroName} [Яростный]";
                border.BackgroundColor = Colors.Gold;
            }
            else
            {
                title.Text = heroName;
            }
        }
    }

    public async void Update()
    {
        running = true;
        while (running)
        {
            await Task.Delay(100);
            energyl.Text = $"Энергия: {energy}";
            lifel.Text = $"Жизни: {mylifes}";
            timer.Text = $"Время: {time}";
            enemylifel.Text = $"Жизни врага: {botlifes}";
            mylifel.Text = $"Твои жизни: {mylifes}";
        }
    }

    private void BuyCard(int cardIndex)
    {
        Border buyBorder = cardIndex == 0 ? buy1 : cardIndex == 1 ? buy2 : buy3;
        Label titleLabel = cardIndex == 0 ? title1 : cardIndex == 1 ? title2 : title3;
        Label enLabel = cardIndex == 0 ? en1 : cardIndex == 1 ? en2 : en3;
        Label hpLabel = cardIndex == 0 ? hp1 : cardIndex == 1 ? hp2 : hp3;
        Label dmgLabel = cardIndex == 0 ? dmg1 : cardIndex == 1 ? dmg2 : dmg3;
        Image img = cardIndex == 0 ? img1 : cardIndex == 1 ? img2 : img3;

        int energyCost = int.Parse(enLabel.Text);

        if (energy < energyCost) return;

        if (buyBorder.BackgroundColor == Colors.LightPink || buyBorder.BackgroundColor == Colors.Gold)
        {
            BuyHero(titleLabel.Text.Replace(" [Яростный]", ""), int.Parse(hpLabel.Text), int.Parse(dmgLabel.Text),
                   energyCost, img.Source.ToString().Replace("File: ", ""));
        }
        else if (buyBorder.BackgroundColor == Colors.LightGreen)
        {
            BuySup(titleLabel.Text, energyCost, img.Source.ToString().Replace("File: ", ""));
        }

        energy -= energyCost;
        switch_card();
        draw();
    }

    private void BuyHero(string name, int hp, int dmg, int energyCost, string image)
    {
        for (int i = 0; i < heroes.Count; i++)
        {
            if (heroes[i] == null)
            {
                var newHero = new Hero(name, hp, dmg, (int)(hp * 1.5), (int)(dmg * 1.5),
                                   0, energyCost, true, image);

                CheckRageAbility(newHero);
                heroes[i] = newHero;
                return;
            }
            else if (heroes[i].name == name || heroes[i].name == name.Replace(" [Ярость]", "").Replace(" [Готов к ярости]", ""))
            {
                bool wasEnraged = heroes[i].isEnraged;
                int rageType = heroes[i].rageType;
                bool hadRageAbility = heroes[i].hasRageAbility;

                heroes[i] = new Hero(heroes[i].name.Replace(" [Ярость]", "").Replace(" [Готов к ярости]", ""),
                                   (int)(heroes[i].hp * 1.5),
                                   (int)(heroes[i].damage * 1.5),
                                   (int)(heroes[i].max_hp * 1.5),
                                   (int)(heroes[i].max_damage * 1.5),
                                   0, heroes[i].energy + energyCost, true, heroes[i].pic);

                CheckRageAbility(heroes[i]);

                if (wasEnraged && heroes[i].hasRageAbility)
                {
                    heroes[i].isEnraged = true;
                    heroes[i].rageType = rageType;

                    // Восстанавливаем яростную картинку
                    switch (heroes[i].rageType)
                    {
                        case 1: heroes[i].pic = "sunrage.png"; break;
                        case 2: heroes[i].pic = "jupiterrage.png"; break;
                        case 3: heroes[i].pic = "netronrage.png"; break;
                        case 4: heroes[i].pic = "quarkrage.png"; break;
                    }
                }
                else
                {
                    heroes[i].isEnraged = false;
                }

                return;
            }
        }
    }

    private void BuySup(string name, int energyCost, string image)
    {
        var supData = sups.FirstOrDefault(a => a.Contains(name));
        if (supData != null)
        {
            string[] data = supData.Split(':');
            int id = int.Parse(data[0]);

            for (int i = 0; i < supes.Count; i++)
            {
                if (supes[i] == null)
                {
                    supes[i] = new Sup(id, name, energyCost, image);
                    return;
                }
            }
        }
    }

    private void CheckRageAbility(Hero hero)
    {
        if (stars < 5000) return; // Ярость доступна только с 5000+ звёзд

        hero.hasRageAbility = false;
        hero.isEnraged = false;
        hero.rageType = 0;

        switch (hero.name)
        {
            case "Солнце":
                int sunRage = Preferences.Get("sun_rage", 0);
                if (sunRage >= 200)
                {
                    hero.hasRageAbility = true;
                    hero.rageType = 1;
                }
                break;

            case "Юпитер":
                int jupiterRage = Preferences.Get("jupiter_rage", 0);
                if (jupiterRage >= 200)
                {
                    hero.hasRageAbility = true;
                    hero.rageType = 2;
                }
                break;

            case "Пульсар":
                int netronRage = Preferences.Get("netron_rage", 0);
                if (netronRage >= 200)
                {
                    hero.hasRageAbility = true;
                    hero.rageType = 3;
                }
                break;

            case "Кварк":
                int quarkRage = Preferences.Get("quark_rage", 0);
                if (quarkRage >= 200)
                {
                    hero.hasRageAbility = true;
                    hero.rageType = 4;
                }
                break;
        }
    }

    private void CheckEnemyRageAbility(Hero hero)
    {
        if (stars >= 5000 && rand.Next(0, 100) < 30) // Враги могут иметь ярость только с 5000+
        {
            switch (hero.name)
            {
                case "Солнце":
                    hero.hasRageAbility = true;
                    hero.rageType = 1;
                    hero.pic = "sunc.png";
                    break;
                case "Юпитер":
                    hero.hasRageAbility = true;
                    hero.rageType = 2;
                    hero.pic = "jupiterc.png";
                    break;
                case "Пульсар":
                    hero.hasRageAbility = true;
                    hero.rageType = 3;
                    hero.pic = "netron.png";
                    break;
                case "Кварк":
                    hero.hasRageAbility = true;
                    hero.rageType = 4;
                    hero.pic = "quark.png";
                    break;
            }
        }
    }

    private void buy1t_Tapped(object sender, TappedEventArgs e) => BuyCard(0);
    private void buy2t_Tapped(object sender, TappedEventArgs e) => BuyCard(1);
    private void buy3t_Tapped(object sender, TappedEventArgs e) => BuyCard(2);

    private void SellHero(int index)
    {
        if (heroes[index] != null)
        {
            if (heroes[index].hp > 0)
            {
                energy += (int)(heroes[index].energy * 0.7);
            }
            heroes[index] = null;
            draw();
        }
    }

    private void SellSup(int index)
    {
        if (supes[index] != null)
        {
            if (supes[index].energy < 999)
            {
                energy += (int)(supes[index].energy * 0.7);
                supes[index] = null;
                draw();
            }
        }
    }

    private void sellhero1_Clicked(object sender, EventArgs e) => SellHero(0);
    private void sellhero2_Clicked(object sender, EventArgs e) => SellHero(1);
    private void sellhero3_Clicked(object sender, EventArgs e) => SellHero(2);
    private void sellsup1_Clicked(object sender, EventArgs e) => SellSup(0);
    private void sellsup2_Clicked(object sender, EventArgs e) => SellSup(1);
    private void sellsup3_Clicked(object sender, EventArgs e) => SellSup(2);

    public async void timer_control()
    {
        time = 30;
        time_run = true;
        while (time_run)
        {
            await Task.Delay(1000);
            if (time > 0) time--;
            else
            {
                time_run = false;
#if ANDROID || IOS
                if (vibro) Vibration.Default.Vibrate();
#endif
                StartBattle();
            }
        }
    }

    private void StartBattle()
    {
        preparing.IsVisible = false;
        fight.IsVisible = true;
        PrepareBattlefield();
        _ = BattleLoop();
    }

    private void PrepareBattlefield()
    {
        UpdateAllBattleDisplays();
        CreateEnemyTeam();
        UpdateAllEnemyDisplays();
    }

    private void UpdateAllBattleDisplays()
    {
        UpdateBattleDisplay(heroes[0], mhp1, mdmg1, men1, mimg1, mtitle1);
        UpdateBattleDisplay(heroes[1], mhp2, mdmg2, men2, mimg2, mtitle2);
        UpdateBattleDisplay(heroes[2], mhp3, mdmg3, men3, mimg3, mtitle3);
        UpdateSupDisplay(supes[0], men4, mimg4, mtitle4);
        UpdateSupDisplay(supes[1], men5, mimg5, mtitle5);
        UpdateSupDisplay(supes[2], men6, mimg6, mtitle6);
    }

    private void UpdateAllEnemyDisplays()
    {
        UpdateBattleDisplay(enemyheroes[0], ehp1, edmg1, een1, eimg1, etitle1);
        UpdateBattleDisplay(enemyheroes[1], ehp2, edmg2, een2, eimg2, etitle2);
        UpdateBattleDisplay(enemyheroes[2], ehp3, edmg3, een3, eimg3, etitle3);
        UpdateSupDisplay(enemysupes[0], een4, eimg4, etitle4);
        UpdateSupDisplay(enemysupes[1], een5, eimg5, etitle5);
        UpdateSupDisplay(enemysupes[2], een6, eimg6, etitle6);
    }

    private void UpdateShopDisplay(Hero? hero, Label hp, Label dmg, Label en, Image img, Label title)
    {
        if (hero != null && hero.alive)
        {
            hp.Text = hero.hp.ToString();

            if (hero.name == "Тыква")
            {
                int dynamicDamage = 110 - hero.hp;
                if (dynamicDamage < 1) dynamicDamage = 1;
                if (dynamicDamage > 110) dynamicDamage = 110;
                dmg.Text = dynamicDamage.ToString();
            }
            else
            {
                dmg.Text = hero.damage.ToString();
            }

            en.Text = hero.energy.ToString();

            if (hero.isEnraged)
            {
                switch (hero.name)
                {
                    case "Солнце":
                        img.Source = "sunrage.png";
                        break;
                    case "Юпитер":
                        img.Source = "jupiterrage.png";
                        break;
                    case "Пульсар":
                        img.Source = "netronrage.png";
                        break;
                    case "Кварк":
                        img.Source = "quarkrage.png";
                        break;
                    default:
                        img.Source = hero.pic;
                        break;
                }
            }
            else
            {
                img.Source = hero.pic;
            }

            if (hero.isEnraged)
            {
                title.Text = $"{hero.name} [Ярость]";
            }
            else if (hero.hasRageAbility && !hero.isEnraged)
            {
                title.Text = $"{hero.name} [Готов к ярости]";
            }
            else
            {
                title.Text = hero.name;
            }
        }
        else if (hero != null && !hero.alive)
        {
            hp.Text = "0";
            dmg.Text = "0";
            en.Text = "0";
            img.Source = hero.pic;
            title.Text = $"{hero.name} (мертв)";
        }
        else
        {
            hp.Text = "0";
            dmg.Text = "0";
            en.Text = "0";
            img.Source = "ico.png";
            title.Text = "Пусто";
        }
    }

    private void UpdateBattleDisplay(Hero? hero, Label hp, Label dmg, Label en, Image img, Label title)
    {
        if (hero != null && hero.alive)
        {
            hp.Text = hero.hp.ToString();

            if (hero.name == "Тыква")
            {
                int dynamicDamage = 110 - hero.hp;
                if (dynamicDamage < 1) dynamicDamage = 1;
                if (dynamicDamage > 110) dynamicDamage = 110;
                dmg.Text = dynamicDamage.ToString();
            }
            else
            {
                dmg.Text = hero.damage.ToString();
            }

            en.Text = hero.radiation.ToString();

            if (hero.isEnraged)
            {
                switch (hero.name)
                {
                    case "Солнце":
                        img.Source = "sunrage.png";
                        break;
                    case "Юпитер":
                        img.Source = "jupiterrage.png";
                        break;
                    case "Пульсар":
                        img.Source = "netronrage.png";
                        break;
                    case "Кварк":
                        img.Source = "quarkrage.png";
                        break;
                    default:
                        img.Source = hero.pic;
                        break;
                }
            }
            else
            {
                img.Source = hero.pic;
            }

            if (hero.isEnraged)
            {
                title.Text = $"{hero.name} [Ярость]";
            }
            else if (hero.hasRageAbility && !hero.isEnraged)
            {
                title.Text = $"{hero.name} [Готов к ярости]";
            }
            else
            {
                title.Text = hero.name;
            }
        }
        else if (hero != null && !hero.alive)
        {
            hp.Text = "0";
            dmg.Text = "0";
            en.Text = "0";
            img.Source = hero.pic;
            title.Text = $"{hero.name} (мертв)";
        }
        else
        {
            hp.Text = "0";
            dmg.Text = "0";
            en.Text = "0";
            img.Source = "ico.png";
            title.Text = "Пусто";
        }
    }

    private void UpdateSupDisplay(Sup? sup, Label en, Image img, Label title)
    {
        if (sup != null)
        {
            en.Text = sup.energy.ToString();
            img.Source = sup.pic;
            title.Text = sup.name;
        }
        else
        {
            en.Text = "0";
            img.Source = "ico.png";
            title.Text = "Пусто";
        }
    }

    private void CreateEnemyTeam()
    {
        int enemyenergy = 10 * loop - 5 * loses;
        int h = 0, s = 0;
        int iter = 0;

        while (enemyenergy > 2 && iter < 10000)
        {
            int type = rand.Next(0, 2);

            if (type == 0 && s < 3)
            {
                int supId = GetRandomSupId();

                // Специальная логика для зелья ярости у врага
                if (supId == 17 && stars >= 5000 && rand.Next(0, 5) == 0 && enemyenergy >= 4)
                {
                    // Оставляем зелье ярости
                }
                else if (supId == 17)
                {
                    // Если выпало зелье ярости, но не должно было, берем другое
                    supId = GetRandomSupId();
                    while (supId == 17)
                    {
                        supId = GetRandomSupId();
                    }
                }

                string supData = supDictionary[supId];
                string[] data = supData.Split(':');
                if (enemyenergy >= int.Parse(data[2]))
                {
                    enemysupes[s] = new Sup(supId, data[1], int.Parse(data[2]), data[3]);
                    enemyenergy -= int.Parse(data[2]);
                    s++;
                }
            }
            else if (type == 1 && h < 3)
            {
                int heroId = GetRandomHeroId();
                string heroData = heroDictionary[heroId];
                string[] data = heroData.Split(':');
                if (enemyenergy >= int.Parse(data[6]))
                {
                    var enemyHero = new Hero(data[1], int.Parse(data[2]), int.Parse(data[3]),
                                           int.Parse(data[4]), int.Parse(data[5]), 0,
                                           int.Parse(data[6]), true, data[7]);

                    CheckEnemyRageAbility(enemyHero);
                    enemyheroes[h] = enemyHero;
                    enemyenergy -= int.Parse(data[6]);
                    h++;
                }
            }
            iter++;
        }

        if (enemyheroes.All(h => h == null))
        {
            var defaultHero = new Hero("Астронавт", 100, 20, 150, 30, 0, 3, true, "astronaut.png");
            enemyheroes[0] = defaultHero;
        }
    }

    public async Task BattleLoop()
    {
        turn.Text = "Бой начался!";
        await Task.Delay(500);

        int mychoose = 0;
        myTurn = true;

        while (HasAliveHeroes(heroes) && HasAliveHeroes(enemyheroes))
        {
            if (myTurn)
            {
                bool turnCompleted = await PlayerTurn(mychoose);
                if (turnCompleted)
                {
                    mychoose = (mychoose + 1) % 3;
                    myTurn = !myTurn;
                }
            }
            else
            {
                bool turnCompleted = await EnemyTurn();
                if (turnCompleted)
                {
                    myTurn = !myTurn;
                }
            }

            ApplyRadiationDamage();

            UpdateAllBattleDisplays();
            UpdateAllEnemyDisplays();

            await Task.Delay(300);
        }

#if ANDROID || IOS
        if (vibro) Vibration.Default.Vibrate();
#endif
        await EndBattle();
    }

    private bool HasAliveHeroes(List<Hero?> heroesList)
    {
        return heroesList.Any(h => h != null && h.alive);
    }

    private async Task<bool> PlayerTurn(int heroIndex)
    {
        if (heroes[heroIndex] == null || !heroes[heroIndex].alive)
        {
            turn.Text = "Ход пропущен (нет героя)";
            await Task.Delay(300);
            return true;
        }

        if (!HasAliveHeroes(enemyheroes))
        {
            turn.Text = "Ход пропущен (нет врагов)";
            await Task.Delay(300);
            return true;
        }

        turn.Text = "Ваш ход!";

        var hero = heroes[heroIndex];
        HighlightHero(heroIndex, true);

        var aliveEnemies = enemyheroes
            .Select((hero, index) => (hero, index))
            .Where(x => x.hero != null && x.hero.alive)
            .ToList();

        if (aliveEnemies.Count == 0)
        {
            HighlightHero(heroIndex, false);
            return true;
        }

        int enemyIndex = aliveEnemies[rand.Next(aliveEnemies.Count)].index;
        var enemy = enemyheroes[enemyIndex];

        HighlightEnemy(enemyIndex, true);
        await ExecuteHeroAbility(heroIndex, enemyIndex);

        await Task.Delay(300);
        HighlightEnemy(enemyIndex, false);
        HighlightHero(heroIndex, false);

        return true;
    }

    private async Task<bool> EnemyTurn()
    {
        if (!HasAliveHeroes(enemyheroes))
        {
            turn.Text = "Противник пропускает ход (нет героев)";
            await Task.Delay(300);
            return true;
        }

        if (!HasAliveHeroes(heroes))
        {
            turn.Text = "Противник пропускает ход (нет целей)";
            await Task.Delay(300);
            return true;
        }

        turn.Text = "Ход противника!";
        await Task.Delay(300);

        int actionType = rand.Next(0, 2);
        bool turnCompleted = false;

        if (actionType == 0)
        {
            turnCompleted = await ExecuteEnemyAttack();
        }
        else
        {
            turnCompleted = await ExecuteEnemySup();
        }

        return turnCompleted;
    }

    private async Task<bool> ExecuteEnemyAttack()
    {
        var aliveEnemies = enemyheroes
            .Select((hero, index) => (hero, index))
            .Where(x => x.hero != null && x.hero.alive)
            .ToList();

        var alivePlayers = heroes
            .Select((hero, index) => (hero, index))
            .Where(x => x.hero != null && x.hero.alive)
            .ToList();

        if (aliveEnemies.Count == 0 || alivePlayers.Count == 0)
            return true;

        int enemyIndex = aliveEnemies[rand.Next(aliveEnemies.Count)].index;
        int playerIndex = alivePlayers[rand.Next(alivePlayers.Count)].index;

        var enemy = enemyheroes[enemyIndex];
        var player = heroes[playerIndex];

        HighlightEnemy(enemyIndex, true);
        HighlightHero(playerIndex, true);

        turn.Text = $"{enemy.name} атакует {player.name}!";

        // Новые карты
        if (enemy.name == "Ёлка")
        {
            // Ёлка наносит небольшой урон радиацией всем
            foreach (var hero in heroes)
            {
                if (hero != null && hero.alive)
                {
                    hero.radiation += 5; // Небольшой урон радиацией всем
                }
            }
            turn.Text = "Ёлка наносит радиацию всем героям!";
        }
        else if (enemy.name == "Снежная планета")
        {
            // Снежная планета немного уменьшает урон всем героям
            foreach (var hero in heroes)
            {
                if (hero != null && hero.alive)
                {
                    hero.damage = Math.Max(0, hero.damage - 5);
                }
            }
            turn.Text = "Снежная планета уменьшает урон всех героев!";
        }
        else if (enemy.name == "Призрак")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки призрака!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (new string[] { "Астронавт", "Солнце", "Луна", "Венера", "Юпитер", "Плутон" }.Contains(enemy.name))
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                int damageDealt = enemy.damage;

                // Проверяем ярость Юпитера
                if (enemy.name == "Юпитер" && enemy.isEnraged)
                {
                    player.hp -= damageDealt;
                    enemy.hp -= damageDealt;
                    turn.Text = $"{enemy.name} в ярости отражает {damageDealt} урона обратно!";

                    if (enemy.hp <= 0)
                    {
                        CheckEnemyDeath(enemyIndex);
                    }
                    CheckPlayerDeath(playerIndex);
                }
                else
                {
                    player.hp -= damageDealt;
                    CheckPlayerDeath(playerIndex);
                }
            }
        }
        else if (enemy.name == "Экзопланета")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Пульсар")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} избежал радиации!";
            }
            else
            {
                if (enemy.isEnraged)
                {
                    player.radiation += 25;
                    turn.Text = $"{enemy.name} в ярости накладывает 25 радиации!";
                }
                else
                {
                    player.radiation += 10;
                }
            }
        }
        else if (enemy.name == "Сверхновая")
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroes[i] != null && heroes[i].alive)
                {
                    if (heroes[i].name == "Призрак" && rand.Next(0, 2) == 0)
                    {
                        // Призрак избежал урона
                    }
                    else
                    {
                        heroes[i].hp -= 20;
                        CheckPlayerDeath(i);
                    }
                }
            }
        }
        else if (enemy.name == "Пиньята")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= 30;
                player.radiation += 10;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Кварк")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                int damageDealt = enemy.damage;
                player.hp -= damageDealt;
                CheckPlayerDeath(playerIndex);

                if (!enemy.isEnraged)
                {
                    enemy.alive = false;
                    enemyheroes[enemyIndex] = null;
                    turn.Text = $"{enemy.name} жертвует собой для нанесения урона!";
                }
                else
                {
                    int oldDamage = enemy.damage;
                    enemy.damage = Math.Max(1, enemy.damage / 2);

                    if (enemy.damage <= 1)
                    {
                        enemy.alive = false;
                        enemyheroes[enemyIndex] = null;
                        turn.Text = $"{enemy.name} исчерпал свою ярость и погибает!";
                    }
                    else
                    {
                        turn.Text = $"{enemy.name} в ярости! Урон уменьшен с {oldDamage} до {enemy.damage}!";
                    }
                }
            }
        }
        else if (enemy.name == "Дискошар")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} избежал ослабления!";
            }
            else
            {
                player.damage -= 10;
                if (player.damage < 0) player.damage = 0;
            }
        }
        else if (enemy.name == "Тыква")
        {
            int dynamicDamage = 110 - enemy.hp;
            if (dynamicDamage < 1) dynamicDamage = 1;
            if (dynamicDamage > 110) dynamicDamage = 110;

            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= dynamicDamage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Мрачник")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Водяная планета")
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                if (heroes[i] != null && heroes[i].alive)
                {
                    if (heroes[i].name == "Призрак" && rand.Next(0, 2) == 0)
                    {
                        // Призрак избежал урона
                    }
                    else
                    {
                        heroes[i].hp -= enemy.damage;
                        CheckPlayerDeath(i);
                    }
                }
            }
            turn.Text = $"{enemy.name} наносит массовый урон всем героям!";
        }
        else if (enemy.name == "Комета")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Кокос")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
            }
        }
        else if (enemy.name == "Песчаный")
        {
            if (player.name == "Призрак" && rand.Next(0, 2) == 0)
            {
                turn.Text = $"{player.name} увернулся от атаки!";
            }
            else
            {
                player.hp -= enemy.damage;
                CheckPlayerDeath(playerIndex);
                enemy.hp = Math.Min(enemy.max_hp, enemy.hp + 10);
                turn.Text = $"{enemy.name} атаковал и исцелил себя на 10 HP!";
            }
        }

        UpdateAllBattleDisplays();
        UpdateAllEnemyDisplays();

        await Task.Delay(300);
        HighlightEnemy(enemyIndex, false);
        HighlightHero(playerIndex, false);

        return true;
    }

    private async Task<bool> ExecuteEnemySup()
    {
        var availableSups = enemysupes
            .Select((sup, index) => (sup, index))
            .Where(x => x.sup != null && x.sup.id != 13)
            .ToList();

        if (availableSups.Count == 0)
        {
            return await ExecuteEnemyAttack();
        }

        var aliveEnemies = enemyheroes
            .Select((hero, index) => (hero, index))
            .Where(x => x.hero != null && x.hero.alive)
            .ToList();

        if (aliveEnemies.Count == 0)
            return true;

        var selectedSup = availableSups[rand.Next(availableSups.Count)];
        int targetIndex = aliveEnemies[rand.Next(aliveEnemies.Count)].index;

        var sup = selectedSup.sup;
        var target = enemyheroes[targetIndex];

        if (sup.id == 17) // Зелье ярости
        {
            if (!target.hasRageAbility)
            {
                return await ExecuteEnemyAttack();
            }

            if (target.isEnraged)
            {
                return await ExecuteEnemyAttack();
            }

            target.isEnraged = true;

            switch (target.rageType)
            {
                case 1:
                    target.pic = "sunrage.png";
                    turn.Text = $"Враг активировал ярость у {target.name}!";
                    break;
                case 2:
                    target.pic = "jupiterrage.png";
                    turn.Text = $"Враг активировал ярость у {target.name}!";
                    break;
                case 3:
                    target.pic = "netronrage.png";
                    turn.Text = $"Враг активировал ярость у {target.name}!";
                    target.radiation = 0;
                    break;
                case 4:
                    target.pic = "quarkrage.png";
                    turn.Text = $"Враг активировал ярость у {target.name}!";
                    break;
            }

            enemysupes[selectedSup.index] = null;
            UpdateAllEnemyDisplays();
            await Task.Delay(300);
            return true;
        }
        else if (sup.id == 16) // Подарок
        {
            // Подарок превращается в случайное усиление
            int randomEffect = rand.Next(1, 8); // 1-7 случайные эффекты
            turn.Text = $"Враг использует Подарок! Получено случайное усиление!";

            switch (randomEffect)
            {
                case 1: // Исцеление
                    target.hp = Math.Min(target.max_hp, target.hp + 20);
                    turn.Text = $"Подарок: {target.name} исцелен на 20 HP!";
                    break;
                case 2: // Урон
                    target.damage = Math.Min(target.max_damage, target.damage + 20);
                    turn.Text = $"Подарок: {target.name} получил +20 к урону!";
                    break;
                case 3: // Смесь
                    target.hp = Math.Min(target.max_hp, target.hp + 20);
                    target.damage = Math.Min(target.max_damage, target.damage + 15);
                    turn.Text = $"Подарок: {target.name} получил +20 HP и +15 к урону!";
                    break;
                case 4: // Ослабление врага
                    var alivePlayers = heroes.Where(h => h != null && h.alive).ToList();
                    if (alivePlayers.Count > 0)
                    {
                        var randomPlayer = alivePlayers[rand.Next(alivePlayers.Count)];
                        randomPlayer.damage = Math.Max(0, randomPlayer.damage - 20);
                        turn.Text = $"Подарок: {randomPlayer.name} получил -20 к урону!";
                    }
                    break;
                case 5: // Большое исцеление
                    target.hp = Math.Min(target.max_hp, target.hp + 30);
                    turn.Text = $"Подарок: {target.name} исцелен на 30 HP!";
                    break;
                case 6: // Чёрная дыра
                    var alivePlayersForBlackHole = heroes.Where(h => h != null && h.alive).ToList();
                    if (alivePlayersForBlackHole.Count > 0)
                    {
                        var randomPlayer = alivePlayersForBlackHole[rand.Next(alivePlayersForBlackHole.Count)];
                        randomPlayer.alive = false;
                        randomPlayer.hp = 0;
                        turn.Text = $"Подарок: {randomPlayer.name} уничтожен черной дырой!";
                    }
                    break;
                case 7: // Экскалибур
                    target.damage = Math.Min(target.max_damage, target.damage + 35);
                    turn.Text = $"Подарок: {target.name} получил Экскалибур (+35 урона)!";
                    break;
            }

            enemysupes[selectedSup.index] = null;
            UpdateAllBattleDisplays();
            UpdateAllEnemyDisplays();
            await Task.Delay(300);
            return true;
        }

        turn.Text = $"Противник использует {sup.name}!";

        switch (sup.id)
        {
            case 1:
                target.hp = Math.Min(target.max_hp, target.hp + 20);
                break;
            case 2:
                target.damage = Math.Min(target.max_damage, target.damage + 20);
                break;
            case 3:
                target.hp = Math.Min(target.max_hp, target.hp + 20);
                target.damage = Math.Min(target.max_damage, target.damage + 15);
                break;
            case 4:
                var alivePlayers = heroes.Where(h => h != null && h.alive).ToList();
                if (alivePlayers.Count > 0)
                {
                    var randomPlayer = alivePlayers[rand.Next(alivePlayers.Count)];
                    randomPlayer.damage = Math.Max(0, randomPlayer.damage - 20);
                }
                break;
            case 5:
                botlifes++;
                break;
            case 6:
                target.hp = Math.Min(target.max_hp, target.hp + 30);
                break;
            case 7:
                var alivePlayersForBlackHole = heroes.Where(h => h != null && h.alive).ToList();
                if (alivePlayersForBlackHole.Count > 0)
                {
                    var randomPlayer = alivePlayersForBlackHole[rand.Next(alivePlayersForBlackHole.Count)];
                    randomPlayer.alive = false;
                    randomPlayer.hp = 0;
                }
                break;
            case 8:
                // Галактика - эффект применяется в конце раунда
                break;
            case 9:
                target.damage = Math.Min(target.max_damage, target.damage + 35);
                break;
            case 10:
                foreach (var enemy in enemyheroes)
                {
                    if (enemy != null) enemy.radiation = 0;
                }
                break;
            case 11:
                int chance = rand.Next(1, 5);
                switch (chance)
                {
                    case 1: target.hp += 20; break;
                    case 2: target.hp = Math.Max(0, target.hp - 20); break;
                    case 3: target.damage += 10; break;
                    case 4: target.damage = Math.Max(0, target.damage - 10); break;
                }
                break;
            case 12:
                foreach (var player in heroes)
                {
                    if (player != null && player.alive)
                    {
                        player.radiation += 10;
                    }
                }
                break;
            case 14:
                int tempHp = target.hp;
                int tempDamage = target.damage;
                target.hp = tempDamage;
                target.damage = tempHp;
                target.max_hp = Math.Max(target.max_hp, target.hp);
                target.max_damage = Math.Max(target.max_damage, target.damage);
                break;
            case 15:
                int randomHeroId = GetRandomHeroId();
                string heroData = heroDictionary[randomHeroId];
                string[] newHeroData = heroData.Split(':');
                enemyheroes[targetIndex] = new Hero(
                    newHeroData[1],
                    int.Parse(newHeroData[2]),
                    int.Parse(newHeroData[3]),
                    int.Parse(newHeroData[4]),
                    int.Parse(newHeroData[5]),
                    0,
                    int.Parse(newHeroData[6]),
                    true,
                    newHeroData[7]
                );
                break;
        }

        enemysupes[selectedSup.index] = null;

        UpdateAllBattleDisplays();
        UpdateAllEnemyDisplays();

        await Task.Delay(300);
        return true;
    }

    private async Task ExecuteHeroAbility(int heroIndex, int enemyIndex)
    {
        var hero = heroes[heroIndex];
        var enemy = enemyheroes[enemyIndex];

        if (hero == null || enemy == null || !enemy.alive) return;

        turn.Text = $"{hero.name} атакует {enemy.name}!";

        // Новые карты
        if (hero.name == "Ёлка")
        {
            // Ёлка наносит небольшой урон радиацией всем врагам
            foreach (var enemyHero in enemyheroes)
            {
                if (enemyHero != null && enemyHero.alive)
                {
                    enemyHero.radiation += 5; // Небольшой урон радиацией всем врагам
                }
            }
            turn.Text = "Ёлка наносит радиацию всем врагам!";
        }
        else if (hero.name == "Снежная планета")
        {
            // Снежная планета немного уменьшает урон всем врагам
            foreach (var enemyHero in enemyheroes)
            {
                if (enemyHero != null && enemyHero.alive)
                {
                    enemyHero.damage = Math.Max(0, enemyHero.damage - 5);
                }
            }
            turn.Text = "Снежная планета уменьшает урон всех врагов!";
        }
        else if (hero.name == "Призрак")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (new string[] { "Астронавт", "Солнце", "Луна", "Венера", "Юпитер", "Плутон" }.Contains(hero.name))
        {
            int damageDealt = hero.damage;

            // Проверяем ярость Юпитера
            if (hero.name == "Юпитер" && hero.isEnraged)
            {
                enemy.hp -= damageDealt;
                hero.hp -= damageDealt;
                turn.Text = $"{hero.name} в ярости отражает {damageDealt} урона обратно!";

                if (hero.hp <= 0)
                {
                    CheckPlayerDeath(heroIndex);
                }
                CheckEnemyDeath(enemyIndex);
            }
            else
            {
                enemy.hp -= damageDealt;
                CheckEnemyDeath(enemyIndex);
            }
        }
        else if (hero.name == "Экзопланета")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Пульсар")
        {
            if (hero.isEnraged)
            {
                enemy.radiation += 25;
                turn.Text = $"{hero.name} в ярости накладывает 25 радиации!";
            }
            else
            {
                enemy.radiation += 10;
            }
        }
        else if (hero.name == "Сверхновая")
        {
            for (int i = 0; i < enemyheroes.Count; i++)
            {
                if (enemyheroes[i] != null && enemyheroes[i].alive)
                {
                    enemyheroes[i].hp -= 20;
                    CheckEnemyDeath(i);
                }
            }
        }
        else if (hero.name == "Пиньята")
        {
            enemy.hp -= 30;
            enemy.radiation += 10;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Кварк")
        {
            int damageDealt = hero.damage;
            enemy.hp -= damageDealt;
            CheckEnemyDeath(enemyIndex);

            if (!hero.isEnraged)
            {
                hero.alive = false;
                heroes[heroIndex] = null;
                turn.Text = $"{hero.name} жертвует собой для нанесения урона!";
            }
            else
            {
                int oldDamage = hero.damage;
                hero.damage = Math.Max(1, hero.damage / 2);

                if (hero.damage <= 1)
                {
                    hero.alive = false;
                    heroes[heroIndex] = null;
                    turn.Text = $"{hero.name} исчерпал свою ярость и погибает!";
                }
                else
                {
                    turn.Text = $"{hero.name} в ярости! Урон уменьшен с {oldDamage} до {hero.damage}!";
                }
            }
        }
        else if (hero.name == "Дискошар")
        {
            enemy.damage -= 10;
            if (enemy.damage < 0) enemy.damage = 0;
        }
        else if (hero.name == "Тыква")
        {
            int dynamicDamage = 110 - hero.hp;
            if (dynamicDamage < 1) dynamicDamage = 1;
            if (dynamicDamage > 110) dynamicDamage = 110;

            enemy.hp -= dynamicDamage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Мрачник")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Водяная планета")
        {
            for (int i = 0; i < enemyheroes.Count; i++)
            {
                if (enemyheroes[i] != null && enemyheroes[i].alive)
                {
                    enemyheroes[i].hp -= hero.damage;
                    CheckEnemyDeath(i);
                }
            }
            turn.Text = $"{hero.name} наносит массовый урон всем врагам!";
        }
        else if (hero.name == "Комета")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Кокос")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
        }
        else if (hero.name == "Песчаный")
        {
            enemy.hp -= hero.damage;
            CheckEnemyDeath(enemyIndex);
            hero.hp = Math.Min(hero.max_hp, hero.hp + 10);
            turn.Text = $"{hero.name} атаковал и исцелил себя на 10 HP!";
        }

        UpdateAllBattleDisplays();
        UpdateAllEnemyDisplays();

        await Task.Delay(300);
    }

    private void CheckEnemyDeath(int enemyIndex)
    {
        var enemy = enemyheroes[enemyIndex];
        if (enemy != null && enemy.hp <= 0)
        {
            // Проверяем ярость Солнца у врага
            if (enemy.name == "Солнце" && enemy.isEnraged)
            {
                enemy.alive = true;
                enemy.hp = 90;
                enemy.damage = 20;
                enemy.max_hp = 135;
                enemy.max_damage = 30;
                enemy.name = "Сверхновая";
                enemy.pic = "supernova.png";
                enemy.isEnraged = false;
                enemy.hasRageAbility = false;

                turn.Text = "Враг: Солнце в ярости превращается в Сверхновую!";
                UpdateAllEnemyDisplays();
                return;
            }

            // Сбрасываем ярость при смерти для других героев
            if (enemy.isEnraged)
            {
                enemy.isEnraged = false;
            }

            enemy.alive = false;
            enemy.hp = 0;

            if (enemy.name == "Экзопланета")
            {
                int a = rand.Next(0, 3);
                enemyheroes[enemyIndex] = a switch
                {
                    0 => new Hero("Венера", 60, 40, 90, 60, 0, 3, true, "venusc.png"),
                    1 => new Hero("Юпитер", 200, 0, 300, 5, 0, 5, true, "jupiterc.png"),
                    2 => new Hero("Плутон", 25, 70, 45, 105, 0, 5, true, "pluto.png"),
                    _ => enemyheroes[enemyIndex]
                };
            }
            else if (enemy.name == "Мрачник")
            {
                for (int i = 0; i < supes.Count; i++)
                {
                    if (supes[i] == null)
                    {
                        supes[i] = new Sup(13, "Мрачное зелье", 999, "disablepotion.png");
                        break;
                    }
                }
                enemyheroes[enemyIndex] = null;
            }
            else if (enemy.name == "Кокос")
            {
                for (int i = 0; i < enemyheroes.Count; i++)
                {
                    if (enemyheroes[i] != null && enemyheroes[i].alive)
                    {
                        enemyheroes[i].hp = Math.Min(enemyheroes[i].max_hp, enemyheroes[i].hp + 40);
                    }
                }
                enemyheroes[enemyIndex] = null;
                turn.Text = "Кокос при смерти исцелил всех союзников на 40 HP!";
            }
            else
            {
                enemyheroes[enemyIndex] = null;
            }

            UpdateAllEnemyDisplays();
        }
    }

    private void CheckPlayerDeath(int playerIndex)
    {
        var player = heroes[playerIndex];
        if (player != null && player.hp <= 0)
        {
            // Проверяем ярость Солнца
            if (player.name == "Солнце" && player.isEnraged)
            {
                player.alive = true;
                player.hp = 90;
                player.damage = 20;
                player.max_hp = 135;
                player.max_damage = 30;
                player.name = "Сверхновая";
                player.pic = "supernova.png";
                player.isEnraged = false;
                player.hasRageAbility = false;

                turn.Text = "Солнце в ярости превращается в Сверхновую!";
                UpdateAllBattleDisplays();
                return;
            }

            // Сбрасываем ярость при смерти для других героев
            if (player.isEnraged)
            {
                player.isEnraged = false;
            }

            player.alive = false;
            player.hp = 0;

            if (player.name == "Экзопланета")
            {
                int a = rand.Next(0, 3);
                heroes[playerIndex] = a switch
                {
                    0 => new Hero("Венера", 60, 40, 90, 60, 0, 3, true, "venusc.png"),
                    1 => new Hero("Юпитер", 200, 0, 300, 5, 0, 5, true, "jupiterc.png"),
                    2 => new Hero("Плутон", 25, 70, 45, 105, 0, 5, true, "pluto.png"),
                    _ => heroes[playerIndex]
                };
            }
            else if (player.name == "Мрачник")
            {
                for (int i = 0; i < enemysupes.Count; i++)
                {
                    if (enemysupes[i] == null)
                    {
                        enemysupes[i] = new Sup(13, "Мрачное зелье", 999, "disablepotion.png");
                        break;
                    }
                }
                heroes[playerIndex] = null;
            }
            else if (player.name == "Кокос")
            {
                for (int i = 0; i < heroes.Count; i++)
                {
                    if (heroes[i] != null && heroes[i].alive)
                    {
                        heroes[i].hp = Math.Min(heroes[i].max_hp, heroes[i].hp + 40);
                    }
                }
                heroes[playerIndex] = null;
                turn.Text = "Кокос при смерти исцелил всех союзников на 40 HP!";
            }
            else
            {
                heroes[playerIndex] = null;
            }

            UpdateAllBattleDisplays();
        }
    }

    private void ApplyRadiationDamage()
    {
        bool needUpdate = false;

        var playersToDie = new List<int>();
        var enemiesToDie = new List<int>();

        // Проверяем игроков
        for (int i = 0; i < heroes.Count; i++)
        {
            var hero = heroes[i];
            if (hero != null && hero.alive)
            {
                // Пульсар в ярости иммунен к радиации
                if (hero.name == "Пульсар" && hero.isEnraged)
                {
                    hero.radiation = 0;
                    continue;
                }

                int oldHp = hero.hp;
                hero.hp -= hero.radiation;
                if (hero.hp <= 0)
                {
                    hero.hp = 0;
                    playersToDie.Add(i);
                    needUpdate = true;
                }
                else if (oldHp != hero.hp)
                {
                    needUpdate = true;
                }
            }
        }

        for (int i = 0; i < enemyheroes.Count; i++)
        {
            var enemy = enemyheroes[i];
            if (enemy != null && enemy.alive)
            {
                // Пульсар врага в ярости иммунен к радиации
                if (enemy.name == "Пульсар" && enemy.isEnraged)
                {
                    enemy.radiation = 0;
                    continue;
                }

                int oldHp = enemy.hp;
                enemy.hp -= enemy.radiation;
                if (enemy.hp <= 0)
                {
                    enemy.hp = 0;
                    enemiesToDie.Add(i);
                    needUpdate = true;
                }
                else if (oldHp != enemy.hp)
                {
                    needUpdate = true;
                }
            }
        }

        foreach (int index in playersToDie)
        {
            CheckPlayerDeath(index);
        }

        foreach (int index in enemiesToDie)
        {
            CheckEnemyDeath(index);
        }

        if (needUpdate || playersToDie.Count > 0 || enemiesToDie.Count > 0)
        {
            UpdateAllBattleDisplays();
            UpdateAllEnemyDisplays();
        }
    }

    private void HighlightHero(int index, bool highlight)
    {
        var color = highlight ? Colors.DarkRed : Colors.LightPink;
        if (index == 0) my1.BackgroundColor = color;
        else if (index == 1) my2.BackgroundColor = color;
        else if (index == 2) my3.BackgroundColor = color;
    }

    private void HighlightEnemy(int index, bool highlight)
    {
        var color = highlight ? Colors.DarkRed : Colors.LightPink;
        if (index == 0) enemy1.BackgroundColor = color;
        else if (index == 1) enemy2.BackgroundColor = color;
        else if (index == 2) enemy3.BackgroundColor = color;
    }

    private async Task EndBattle()
    {
        if (supes.Any(s => s != null && s.id == 8) || enemysupes.Any(s => s != null && s.id == 8))
        {
            energy += 8;
            for (int i = 0; i < supes.Count; i++)
            {
                if (supes[i] != null && supes[i].id == 8)
                    supes[i] = null;
            }
            for (int i = 0; i < enemysupes.Count; i++)
            {
                if (enemysupes[i] != null && enemysupes[i].id == 8)
                    enemysupes[i] = null;
            }
        }

        // Оставляем только сброс радиации, но сохраняем ярость
        foreach (var hero in heroes)
        {
            if (hero != null)
            {
                hero.radiation = 0; // Сбрасываем только радиацию
                // Ярость НЕ сбрасываем: hero.isEnraged остается как есть
            }
        }

        foreach (var enemy in enemyheroes)
        {
            if (enemy != null)
            {
                enemy.isEnraged = false;
                enemy.radiation = 0;

                // Возвращаем оригинальную картинку для врагов
                if (enemy.hasRageAbility)
                {
                    switch (enemy.rageType)
                    {
                        case 1:
                            enemy.pic = "sunc.png";
                            break;
                        case 2:
                            enemy.pic = "jupiterc.png";
                            break;
                        case 3:
                            enemy.pic = "netron.png";
                            break;
                        case 4:
                            enemy.pic = "quark.png";
                            break;
                    }
                }
            }
        }

        bool playerHasAliveHeroes = HasAliveHeroes(heroes);
        bool enemyHasAliveHeroes = HasAliveHeroes(enemyheroes);

        if (!playerHasAliveHeroes)
        {
            mylifes--;
            loses++;
            energy += 10;
        }
        if (!enemyHasAliveHeroes) botlifes--;

        energy += 10;
        loop++;

        await Task.Delay(500);

        if (mylifes <= 0)
        {
            await Navigation.PopModalAsync();
        }
        else if (botlifes <= 0)
        {
            int exp = Preferences.Get("exp", 0);
            Preferences.Set("exp", exp + 1000);
            Preferences.Set("stars", stars + 60);

            if (parts && stars >= 5000) // Фрагменты ярости только с 5000+
            {
                Random rand = new Random();
                int maxRage = 200;

                // Создаем список доступных вариантов
                var availableRages = new List<int>();

                if (Preferences.Get("sun_rage", 0) < maxRage)
                    availableRages.Add(1);
                if (Preferences.Get("jupiter_rage", 0) < maxRage)
                    availableRages.Add(2);
                if (Preferences.Get("netron_rage", 0) < maxRage)
                    availableRages.Add(3);
                if (Preferences.Get("quark_rage", 0) < maxRage)
                    availableRages.Add(4);

                if (availableRages.Count > 0)
                {
                    // Выбираем случайный из доступных
                    int selectedRage = availableRages[rand.Next(0, availableRages.Count)];
                    int add = 0;

                    switch (selectedRage)
                    {
                        case 1:
                            add = Preferences.Get("sun_rage", 0);
                            Preferences.Set("sun_rage", add + 10);
                            break;
                        case 2:
                            add = Preferences.Get("jupiter_rage", 0);
                            Preferences.Set("jupiter_rage", add + 10);
                            break;
                        case 3:
                            add = Preferences.Get("netron_rage", 0);
                            Preferences.Set("netron_rage", add + 10);
                            break;
                        case 4:
                            add = Preferences.Get("quark_rage", 0);
                            Preferences.Set("quark_rage", add + 10);
                            break;
                    }
                }
            }

            await Navigation.PopModalAsync();
        }
        else
        {
            enemyheroes = new List<Hero?>() { null, null, null };
            enemysupes = new List<Sup?>() { null, null, null };

            fight.IsVisible = false;
            preparing.IsVisible = true;

            draw();

            timer_control();
        }
    }

    private void SetTarget(int index)
    {
        if (heroes[index] != null && heroes[index].alive)
        {
            target = index;
            UpdateTargetHighlight();
            turn.Text = $"Цель: {heroes[index].name}";
        }
        else
        {
            turn.Text = "Нельзя выбрать эту цель!";
        }
    }

    private void UpdateTargetHighlight()
    {
        my1.Stroke = Colors.Black;
        my2.Stroke = Colors.Black;
        my3.Stroke = Colors.Black;

        if (target == 0) my1.Stroke = Colors.DarkRed;
        else if (target == 1) my2.Stroke = Colors.DarkRed;
        else if (target == 2) my3.Stroke = Colors.DarkRed;
    }

    private async void tar1_Tapped(object sender, TappedEventArgs e) => SetTarget(0);
    private async void tar2_Tapped(object sender, TappedEventArgs e) => SetTarget(1);
    private async void tar3_Tapped(object sender, TappedEventArgs e) => SetTarget(2);

    private void sup1_Tapped(object sender, TappedEventArgs e) => use_sup(0);
    private void sup2_Tapped(object sender, TappedEventArgs e) => use_sup(1);
    private void sup3_Tapped(object sender, TappedEventArgs e) => use_sup(2);

    public void use_sup(int n)
    {
        if (n < 0 || n >= supes.Count)
        {
            turn.Text = "Неверный номер зелья!";
            return;
        }

        if (supes[n] == null)
        {
            turn.Text = "Нет зелья в этой ячейке!";
            return;
        }

        if (target < 0 || target >= heroes.Count || heroes[target] == null || !heroes[target].alive)
        {
            turn.Text = "Сначала выберите живого героя!";
            return;
        }

        var hero = heroes[target];
        var sup = supes[n];

        if (sup.id == 17) // Зелье ярости (только для 5000+)
        {
            if (stars < 5000)
            {
                turn.Text = "Зелье ярости доступно только с 5000 звёзд!";
                return;
            }

            if (!hero.hasRageAbility)
            {
                turn.Text = $"У {hero.name} нет доступа к яростной способности!";
                return;
            }

            if (hero.isEnraged)
            {
                turn.Text = $"{hero.name} уже в ярости!";
                return;
            }

            hero.isEnraged = true;

            switch (hero.rageType)
            {
                case 1: // Солнце
                    hero.pic = "sunrage.png";
                    turn.Text = $"{hero.name} теперь превратится в Сверхновую при смерти!";
                    break;
                case 2: // Юпитер
                    hero.pic = "jupiterrage.png";
                    turn.Text = $"{hero.name} теперь отражает урон обратно атакующему!";
                    break;
                case 3: // Нейтронная звезда (Пульсар)
                    hero.pic = "netronrage.png";
                    turn.Text = $"{hero.name} теперь наносит больше радиации и иммунен к ней!";
                    hero.radiation = 0;
                    break;
                case 4: // Кварк
                    hero.pic = "quarkrage.png";
                    turn.Text = $"{hero.name} теперь не умирает от своей атаки, но урон уменьшается!";
                    break;
            }

            supes[n] = null;

            UpdateAllBattleDisplays();
            draw();
            return;
        }
        else if (sup.id == 16) // Подарок
        {
            if (stars < 4000)
            {
                turn.Text = "Подарок доступен только с 4000 звёзд!";
                return;
            }

            // Подарок превращается в случайное усиление
            int randomEffect = rand.Next(1, 8); // 1-7 случайные эффекты
            turn.Text = $"Подарок превращается в случайное усиление!";

            switch (randomEffect)
            {
                case 1: // Исцеление
                    hero.hp = Math.Min(hero.max_hp, hero.hp + 20);
                    turn.Text = $"Подарок: {hero.name} исцелен на 20 HP!";
                    break;
                case 2: // Урон
                    hero.damage = Math.Min(hero.max_damage, hero.damage + 20);
                    turn.Text = $"Подарок: {hero.name} получил +20 к урону!";
                    break;
                case 3: // Смесь
                    hero.hp = Math.Min(hero.max_hp, hero.hp + 20);
                    hero.damage = Math.Min(hero.max_damage, hero.damage + 15);
                    turn.Text = $"Подарок: {hero.name} получил +20 HP и +15 к урону!";
                    break;
                case 4: // Ослабление врага
                    var aliveEnemies = enemyheroes.Where(h => h != null && h.alive).ToList();
                    if (aliveEnemies.Count > 0)
                    {
                        var randomEnemy = aliveEnemies[rand.Next(aliveEnemies.Count)];
                        randomEnemy.damage = Math.Max(0, randomEnemy.damage - 20);
                        turn.Text = $"Подарок: {randomEnemy.name} получил -20 к урону!";
                        UpdateAllEnemyDisplays();
                    }
                    else
                    {
                        turn.Text = "Подарок: Нет целей для ослабления!";
                    }
                    break;
                case 5: // Большое исцеление
                    hero.hp = Math.Min(hero.max_hp, hero.hp + 30);
                    turn.Text = $"Подарок: {hero.name} исцелен на 30 HP!";
                    break;
                case 6: // Чёрная дыра
                    var aliveEnemiesForBlackHole = enemyheroes.Where(h => h != null && h.alive).ToList();
                    if (aliveEnemiesForBlackHole.Count > 0)
                    {
                        var randomEnemy = aliveEnemiesForBlackHole[rand.Next(aliveEnemiesForBlackHole.Count)];
                        randomEnemy.alive = false;
                        randomEnemy.hp = 0;
                        turn.Text = $"Подарок: {randomEnemy.name} уничтожен черной дырой!";
                        UpdateAllEnemyDisplays();
                    }
                    else
                    {
                        turn.Text = "Подарок: Нет целей для черной дыры!";
                    }
                    break;
                case 7: // Экскалибур
                    hero.damage = Math.Min(hero.max_damage, hero.damage + 35);
                    turn.Text = $"Подарок: {hero.name} получил Экскалибур (+35 урона)!";
                    break;
            }

            supes[n] = null;
            UpdateAllBattleDisplays();
            UpdateAllEnemyDisplays();
            return;
        }

        turn.Text = $"Используется {sup.name} на {hero.name}!";

        switch (sup.id)
        {
            case 1:
                hero.hp = Math.Min(hero.max_hp, hero.hp + 20);
                turn.Text = $"{hero.name} исцелен на 20 HP!";
                break;
            case 2:
                hero.damage = Math.Min(hero.max_damage, hero.damage + 20);
                turn.Text = $"{hero.name} получил +20 к урону!";
                break;
            case 3:
                hero.hp = Math.Min(hero.max_hp, hero.hp + 20);
                hero.damage = Math.Min(hero.max_damage, hero.damage + 15);
                turn.Text = $"{hero.name} получил +20 HP и +15 к урону!";
                break;
            case 4:
                var aliveEnemies = enemyheroes.Where(h => h != null && h.alive).ToList();
                if (aliveEnemies.Count > 0)
                {
                    var randomEnemy = aliveEnemies[rand.Next(aliveEnemies.Count)];
                    randomEnemy.damage = Math.Max(0, randomEnemy.damage - 20);
                    turn.Text = $"{randomEnemy.name} получил -20 к урону!";
                    UpdateAllEnemyDisplays();
                }
                else
                {
                    turn.Text = "Нет целей для ослабления!";
                }
                break;
            case 5:
                mylifes++;
                turn.Text = "Получена дополнительная жизнь!";
                break;
            case 6:
                hero.hp = Math.Min(hero.max_hp, hero.hp + 30);
                turn.Text = $"{hero.name} исцелен на 30 HP!";
                break;
            case 7:
                var aliveEnemiesForBlackHole = enemyheroes.Where(h => h != null && h.alive).ToList();
                if (aliveEnemiesForBlackHole.Count > 0)
                {
                    var randomEnemy = aliveEnemiesForBlackHole[rand.Next(aliveEnemiesForBlackHole.Count)];
                    randomEnemy.alive = false;
                    randomEnemy.hp = 0;
                    turn.Text = $"{randomEnemy.name} уничтожен черной дырой!";
                    UpdateAllEnemyDisplays();
                }
                else
                {
                    turn.Text = "Нет целей для черной дыры!";
                }
                break;
            case 8:
                turn.Text = "Эффект Галактики активирован!";
                break;
            case 9:
                hero.damage = Math.Min(hero.max_damage, hero.damage + 35);
                turn.Text = $"{hero.name} получил +35 к урону!";
                break;
            case 10:
                foreach (var playerHero in heroes)
                {
                    if (playerHero != null)
                    {
                        playerHero.radiation = 0;
                        playerHero.hp = Math.Min(playerHero.max_hp, playerHero.hp + 10);
                    }
                }
                turn.Text = "Радиация очищена у всех героев!";
                break;
            case 11:
                int chance = rand.Next(1, 5);
                switch (chance)
                {
                    case 1:
                        hero.hp = Math.Min(hero.max_hp, hero.hp + 20);
                        turn.Text = $"{hero.name} получил +20 HP!";
                        break;
                    case 2:
                        hero.hp = Math.Max(1, hero.hp - 20);
                        turn.Text = $"{hero.name} потерял 20 HP!";
                        break;
                    case 3:
                        hero.damage = Math.Min(hero.max_damage, hero.damage + 10);
                        turn.Text = $"{hero.name} получил +10 к урону!";
                        break;
                    case 4:
                        hero.damage = Math.Max(1, hero.damage - 10);
                        turn.Text = $"{hero.name} потерял 10 урона!";
                        break;
                }
                break;
            case 12:
                foreach (var enemy in enemyheroes)
                {
                    if (enemy != null && enemy.alive)
                    {
                        enemy.radiation += 10;
                    }
                }
                turn.Text = "Все враги получили радиацию!";
                UpdateAllEnemyDisplays();
                break;
            case 13:
                turn.Text = "Мрачное зелье нельзя использовать!";
                return;
            case 14:
                int tempHp = hero.hp;
                int tempDamage = hero.damage;
                hero.hp = Math.Min(hero.max_hp, tempDamage);
                hero.damage = Math.Min(hero.max_damage, tempHp);

                if (hero.hp > hero.max_hp) hero.max_hp = hero.hp;
                if (hero.damage > hero.max_damage) hero.max_damage = hero.damage;

                turn.Text = $"{hero.name} поменял HP и урон местами!";
                break;
            case 15:
                int randomHeroId = GetRandomHeroId();
                string heroData = heroDictionary[randomHeroId];
                string[] newHeroData = heroData.Split(':');
                string oldName = hero.name;

                heroes[target] = new Hero(
                    newHeroData[1],
                    int.Parse(newHeroData[2]),
                    int.Parse(newHeroData[3]),
                    int.Parse(newHeroData[4]),
                    int.Parse(newHeroData[5]),
                    hero.radiation,
                    int.Parse(newHeroData[6]),
                    true,
                    newHeroData[7]
                );
                turn.Text = $"{oldName} превратился в {heroes[target].name}!";
                break;
            default:
                turn.Text = "Неизвестное зелье!";
                return;
        }

        if (sup.id != 13)
        {
            supes[n] = null;
        }

        UpdateAllBattleDisplays();
        UpdateAllEnemyDisplays();
    }

    public void draw()
    {
        UpdateShopDisplay(heroes[0], herohp1, herodmg1, heroen1, heroimg1, herotitle1);
        UpdateShopDisplay(heroes[1], herohp2, herodmg2, heroen2, heroimg2, herotitle2);
        UpdateShopDisplay(heroes[2], herohp3, herodmg3, heroen3, heroimg3, herotitle3);
        UpdateSupDisplay(supes[0], supen1, supimg1, suptitle1);
        UpdateSupDisplay(supes[1], supen2, supimg2, suptitle2);
        UpdateSupDisplay(supes[2], supen3, supimg3, suptitle3);
    }

    private void skip_Clicked(object sender, EventArgs e)
    {
        time = 5;
    }
}

public class Hero
{
    public string name;
    public int hp;
    public int damage;
    public int max_hp;
    public int max_damage;
    public int radiation;
    public int energy;
    public bool alive;
    public string pic;
    public bool hasRageAbility;
    public bool isEnraged;
    public int rageType;

    public Hero(string name, int hp, int dmg, int max_hp, int max_dmg, int radiation,
                int energy, bool alive, string pic)
    {
        this.name = name;
        this.hp = hp;
        this.damage = dmg;
        this.max_hp = max_hp;
        this.max_damage = max_dmg;
        this.radiation = radiation;
        this.energy = energy;
        this.alive = alive;
        this.pic = pic;
        this.hasRageAbility = false;
        this.isEnraged = false;
        this.rageType = 0;
    }
}

public class Sup
{
    public int id;
    public string name;
    public int energy;
    public string pic;

    public Sup(int id, string name, int energy, string pic)
    {
        this.id = id;
        this.name = name;
        this.energy = energy;
        this.pic = pic;
    }
}