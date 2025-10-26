namespace PlanetPedia;

public partial class game : ContentPage
{
    bool running = false;
    int star = 0;
    int currentZone = -1; // Добавляем переменную для отслеживания текущей зоны

    public game()
    {
        InitializeComponent();
#if ANDROID
        img.Margin = new Thickness(40,-20,40,0);
        play.Margin = new Thickness(20,0,20,0);
#endif
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
        else
        {
            newZoneText = "Пляжная зона";
            newImageSource = "island6.jfif";
            SetZoneIfChanged(5, newZoneText, newImageSource);
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
        Navigation.PushModalAsync(new ingame(star));
    }

    private async void Update()
    {
        running = true;
        while (running)
        {
            await Task.Delay(1000); // Увеличиваем задержку до 1 секунды
            int newStar = Preferences.Get("stars", 0);

            // Обновляем только если количество звезд изменилось
            if (newStar != star)
            {
                star = newStar;
                stars.Text = $"Звёзды: {star}⭐";
                UpdateZone();
            }

            if(newStar > 5000)
            {
                hyperarena.IsEnabled = true;
                hyperarena.Background = Colors.Purple;
                hyperarena.BorderColor = Colors.Pink;
            }
            else
            {
                hyperarena.IsEnabled = false;
                hyperarena.Background = Colors.LightGray;
                hyperarena.BorderColor = Colors.Gray;
            }
        }
    }

    private void hyperarena_Clicked(object sender, EventArgs e)
    {

    }
}