namespace PlanetPedia;

public partial class game : ContentPage
{
    bool running = false;
    int star = 0;
    int currentZone = -1; // Добавляем переменную для отслеживания текущей зоны

    Dictionary<string, bool> states = new Dictionary<string, bool>()
    {
        {"sun", false},
        {"jupiter", false},
        {"netron", false },
        {"quark", false }
    };

    public game()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        star = Preferences.Get("stars", 0);
        stars.Text = $"Звёзды: {star}⭐";
        UpdateZone(); // Используем отдельный метод для обновления зоны
        Update();
        base.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        running = false; // Останавливаем цикл при скрытии страницы
        base.OnDisappearing();
    }

    private void UpdateZone()
    {
        string newZoneText;
        string newImageSource;

        if (star >= 0 && star <= 500)
        {
            newZoneText = "Зона Орбиты";
            newImageSource = "island1.jfif";
            SetZoneIfChanged(0, newZoneText, newImageSource);
        }
        else if (star > 500 && star <= 1000)
        {
            newZoneText = "Зона Солнечной Системы";
            newImageSource = "island2.jfif";
            SetZoneIfChanged(1, newZoneText, newImageSource);
        }
        else if (star > 1000 && star <= 1500)
        {
            newZoneText = "Зона Вне дома";
            newImageSource = "island3.jfif";
            SetZoneIfChanged(2, newZoneText, newImageSource);
        }
        else if (star > 1500 && star <= 2000)
        {
            newZoneText = "Зона Веселья";
            newImageSource = "island4.jfif";
            SetZoneIfChanged(3, newZoneText, newImageSource);
        }
        else if (star > 2000 && star <= 3000)
        {
            newZoneText = "Зона Страха";
            newImageSource = "island5.jfif";
            SetZoneIfChanged(4, newZoneText, newImageSource);
        }
        else if (star > 3000 && star <= 4000)
        {
            newZoneText = "Пляжная зона";
            newImageSource = "island6.jfif";
            SetZoneIfChanged(5, newZoneText, newImageSource);
        }
        else if (star > 4000 && star <= 5000)
        {
            newZoneText = "Зимняя зона";
            newImageSource = "island8.jfif";
            SetZoneIfChanged(6, newZoneText, newImageSource);
        }
        else
        {
            newZoneText = "Адская зона";
            newImageSource = "island7.jpeg";
            SetZoneIfChanged(7, newZoneText, newImageSource);
        }
    }

    private void SetZoneIfChanged(int zoneId, string zoneText, string imageSource)
    {
        // Обновляем только если зона действительно изменилась
        if (currentZone != zoneId)
        {
            currentZone = zoneId;
            zone.Text = zoneText;
            img.Source = imageSource;
        }
    }

    private void collection_Clicked(object sender, EventArgs e)
    {
        main.IsVisible = false;
        way.IsVisible = false;
        cards.IsVisible = true;
    }

    private void back_Clicked(object sender, EventArgs e)
    {
        main.IsVisible = true;
        way.IsVisible = false;
        cards.IsVisible = false;
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        main.IsVisible = false;
        way.IsVisible = true;
        cards.IsVisible = false;
    }

    private void play_Clicked(object sender, EventArgs e)
    {
        int star = Preferences.Get("stars", 0);
        if (star >= 60) star -= 30;
        else star = 0;
        Preferences.Set("stars", star);
        if(star > 4000) Navigation.PushModalAsync(new ingame(star, true));
        else Navigation.PushModalAsync(new ingame(star, false));

    }

    private async void Update()
    {
        running = true;
        while (running)
        {
            await Task.Delay(1000); // Увеличиваем задержку до 1 секунды
            int newStar = Preferences.Get("stars", 0);
            if (newStar > 4000) parts.Text = "Победа: +5 фрагментов ярости";
            else parts.Text = "Получите более 5000 звёзд чтобы получить фрагменты ярости!";

            // Обновляем только если количество звезд изменилось
            if (newStar != star)
            {
                star = newStar;
                stars.Text = $"Звёзды: {star}⭐";
                UpdateZone();
            }

        }
    }

    private void rage_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new rage());
    }

    private void sunrage(object sender, EventArgs e)
    {
        states["sun"] = !states["sun"];

        if (states["sun"])
        {
            sunimg.Source = "sunrage.png";
            sunstates.Text = $"Фрагменты {Math.Min(Preferences.Get("sun_rage", 0), 200)}/200";
            sundesc.Text = "При гибели превращается в сверхновую";
        }
        else
        {
            sunimg.Source = "sunc.png";
            sunstates.Text = "Здоровье: 40; Урон: 50";
            sundesc.Text = "Огромный воин, но с маленьким здоровьем и большим уроном";
        }
    }

    private void jupiterrage(object sender, EventArgs e)
    {
        states["jupiter"] = !states["jupiter"];

        if (states["jupiter"])
        {
            jupiterimg.Source = "jupiterrage.png";
            jupiterstates.Text = $"Фрагменты {Math.Min(Preferences.Get("jupiter_rage", 0), 200)}/200";
            jupiterdesc.Text = "Возвращает урон обратно";
        }
        else
        {
            jupiterimg.Source = "jupiterc.png";
            jupiterstates.Text = "Здоровье: 180; Урон: 3";
            jupiterdesc.Text = "Огромная планета с мощнейшим запасом здоровья, но очень уж дружелюбная";
        }
    }

    private void netronrage(object sender, EventArgs e)
    {
        states["netron"] = !states["netron"];

        if (states["netron"])
        {
            netronimg.Source = "netronrage.png";
            netronstates.Text = $"Фрагменты {Math.Min(Preferences.Get("netron_rage", 0), 200)}/200";
            netrondesc.Text = "Усиленная радиация и иммунитет к ней";
        }
        else
        {
            netronimg.Source = "netron.png";
            netronstates.Text = "Здоровье: 100; Радиация: 10";
            netrondesc.Text = "Опасная звезда, наносящая периодический урон радиацией";
        }
    }

    private void quarkrage(object sender, EventArgs e)
    {
        states["quark"] = !states["quark"];

        if (states["quark"])
        {
            quarkimg.Source = "quarkrage.png";
            quarkstates.Text = $"Фрагменты {Math.Min(Preferences.Get("quark_rage", 0), 200)}/200";
            quarkdesc.Text = "Получает полураспад. Теперь кварк не умирает после атаки, а уменьшает свой урон вдвое. После достижения нулевого урона, погибает";
        }
        else
        {
            quarkimg.Source = "quark.png";
            quarkstates.Text = "Здоровье: 200; Урон: 80";
            quarkdesc.Text = "Маленькая частица с большим уроном. После своего хода умирает";
        }
    }
}