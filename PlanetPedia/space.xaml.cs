using Microsoft.Maui.Graphics;
using System.Diagnostics;

namespace PlanetPedia;

public partial class space : ContentPage
{
    bool running = false;
    bool way_open = false;
    int lvl = 1;
    int expe = 0;
    int strike = 1;
    bool anim = false;
    Dictionary<int, string> titles = new Dictionary<int, string>()
        {
            {1, "Заморожённый январь"},
            {2, "Звёздный февраль" },
            {3, "Галактический март" },
            {4, "Планетарный апрель" },
            {5, "Инопланетный май"},
            {6, "Солнечный июнь" },
            {7, "Звездопадный июль" },
            {8, "Экзоавгуст" },
            {9, "Сентябрь сверхновых" },
            {10, "Ядерный октябрь" },
            {11, "Атмосферный ноябрь" },
            {12, "Декабрь созвездий" }
        };
    public space()
	{
		InitializeComponent();

        List<VisualElement> elements = new List<VisualElement>() {title, bor1, bor2, bor3, bor4, bor5, bor6};
        foreach (VisualElement element in elements) element.Opacity = 0;
        anim = true;

        bor1.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor2.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor3.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor4.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor5.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        bor6.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<VisualElement> elements = new List<VisualElement>() { title, bor1, bor2, bor3, bor4, bor5, bor6};
        expe = Preferences.Get("exp",0);
        while (expe >= 100 * lvl && lvl <= 100)
        {
            expe -= 100 * lvl;
            lvl++;
        }
        if (lvl == 101) lvl = 100;
        levelico.Text = lvl.ToString();
        level.Text = $"Уровень {lvl}";
        bonustitle.Text = titles[DateTime.Now.Month];
        if (lvl < 100) exp.Text = $"Опыт: {expe} / {lvl * 100}";
        else exp.Text = "Опыт: MAX";
            color();

        List<string> dates = Preferences.Get("dates", "").Split(";").ToList();
        if (!dates.Contains(DateTime.UtcNow.ToString("dd.MM.yyyy")))
        {
            bonus.Text = "Забрать";
            bonus.IsEnabled = true;
        }
        else
        {
            bonus.Text = "Заходи завтра";
            bonus.IsEnabled = false;
        }
        Update();

        await Task.Delay(300);
        foreach (VisualElement element in elements)
        {
            float tr = anim ? 0f : 1f;
            while (tr < 1)
            {
                tr += 0.1f;
                element.Opacity = tr;
                await Task.Delay(10);
            }
            await Task.Delay(10);
        }
        anim = false;
    }

    private void levelico_Clicked(object sender, EventArgs e)
    {
        way_open = !way_open;
        if (way_open)
        {
            way.IsVisible = true;
            content.IsVisible = false;
            way.Children.Clear();

            for (int i = 1; i <= 100; i++)
            {
                Label header = new Label()
                {
                    Text = $"Уровень {i}",
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 16,
                    FontAttributes = FontAttributes.Italic,
                    Margin = new Thickness(0, 5)
                };
                Button stair = new Button()
                {
                    Text = i.ToString(),
                    WidthRequest = 200,
                    HeightRequest = 200,
                    CornerRadius = 100,
                    BorderWidth = 7,
                    BorderColor = Colors.Black,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 80,
                    HorizontalOptions = LayoutOptions.Center,
                    IsEnabled = false,
                    Margin = new Thickness(0, 5, 0, 30)
                };
                way.Children.Add(header);
                way.Children.Add(stair);

                if (i <= 10) stair.Background = Colors.LightGray;
                else if (i <= 20 && i > 10) stair.Background = Colors.LightGreen;
                else if (i <= 30 && i > 20) stair.Background = Colors.LightYellow;
                else if (i <= 40 && i > 30) stair.Background = Colors.PeachPuff;
                else if (i <= 50 && i > 40) stair.Background = Colors.Orange;
                else if (i <= 60 && i > 50) stair.Background = Colors.HotPink;
                else if (i <= 70 && i > 60) stair.Background = Colors.Red;
                else if (i <= 80 && i > 70) stair.Background = Colors.DarkRed;
                else if (i <= 90 && i > 80)
                {
                    var gradientBrush = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1)
                    };

                    gradientBrush.GradientStops.Add(new GradientStop(Colors.Cyan, 0.0f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 1.0f));

                    stair.Background = gradientBrush;
                }
                else if (i < 100 && i > 90)
                {
                    var gradientBrush = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1)
                    };

                    gradientBrush.GradientStops.Add(new GradientStop(Colors.LightYellow, 0.0f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.HotPink, 1.0f));

                    stair.Background = gradientBrush;
                }
                else
                {
                    var gradientBrush = new LinearGradientBrush
                    {
                        StartPoint = new Point(0, 0),
                        EndPoint = new Point(1, 1)
                    };

                    gradientBrush.GradientStops.Add(new GradientStop(Colors.HotPink, 0.0f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.LightYellow, 0.25f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.5f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 0.75f));
                    gradientBrush.GradientStops.Add(new GradientStop(Colors.Magenta, 1.0f));

                    stair.Background = gradientBrush;
                }

                if (i <= 80 && i > 60) stair.TextColor = Colors.White;
                else stair.TextColor = Colors.Black;

                if (lvl == i && lvl != 100) way.Children.Add(separator);
            }
        }
        else
        {
            way.IsVisible = false;
            content.IsVisible = true;
        }
    }

    private void color()
    {
        if (lvl <= 10) levelico.Background = Colors.LightGray;
        else if (lvl <= 20 && lvl > 10) levelico.Background = Colors.LightGreen;
        else if (lvl <= 30 && lvl > 20) levelico.Background = Colors.LightYellow;
        else if (lvl <= 40 && lvl > 30) levelico.Background = Colors.PeachPuff;
        else if (lvl <= 50 && lvl > 40) levelico.Background = Colors.Orange;
        else if (lvl <= 60 && lvl > 50) levelico.Background = Colors.HotPink;
        else if (lvl <= 70 && lvl > 60) levelico.Background = Colors.Red;
        else if (lvl <= 80 && lvl > 70) levelico.Background = Colors.DarkRed;
        else if (lvl <= 90 && lvl > 80)
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Colors.Cyan, 0.0f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 1.0f));

            levelico.Background = gradientBrush;
        }
        else if (lvl < 100 && lvl > 90)
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightYellow, 0.0f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.HotPink, 1.0f));

            levelico.Background = gradientBrush;
        }
        else
        {
            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(1, 1)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Colors.HotPink, 0.0f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightYellow, 0.25f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightGreen, 0.5f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.LightBlue, 0.75f));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Magenta, 1.0f));

            levelico.Background = gradientBrush;
        }

        if (lvl <= 80 && lvl > 60) levelico.TextColor = Colors.White;
        else levelico.TextColor = Colors.Black;
    }

    private void bonus_Clicked(object sender, EventArgs e)
    {
        string date = Preferences.Get("dates", "");
        Preferences.Set("dates", date + ";" + DateTime.Now.ToString("dd.MM.yyyy"));

        int expe = Preferences.Get("exp",0);
        if(strike > 1) Preferences.Set("exp", expe + 500 + strike * 50);
        else Preferences.Set("exp", expe + 500);
        OnAppearing();
    }


    private void objects_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new objects());
    }

    public async void Update()
    {
        running = true;
        while (running)
        {
            await Task.Delay(300);
            lvl = 1;
            expe = 0;
            expe = Preferences.Get("exp", 0);
            while (expe >= 100 * lvl && lvl <= 100)
            {
                expe -= 100 * lvl;
                lvl++;
            }
            if (lvl == 101) lvl = 100;
            levelico.Text = lvl.ToString();
            level.Text = $"Уровень {lvl}";
            if (lvl < 100) exp.Text = $"Опыт: {expe} / {lvl * 100}";
            else exp.Text = "Опыт: MAX";
            color();

            List<int> days = new List<int>();
            List<string> dates = Preferences.Get("dates", "").Split(";").ToList();
            if (!dates.Contains(DateTime.Now.ToString("dd.MM.yyyy")))
            {
                bonus.Text = "Забрать";
                bonus.IsEnabled = true;
            }
            else
            {
                bonus.Text = "Заходи завтра";
                bonus.IsEnabled = false;
            }

            /*
            dates.RemoveAt(0);
            foreach(string date in dates)
            {
                days.Add((int)Math.Floor((Convert.ToDateTime(date) - new DateTime(1900, 1, 1)).TotalDays));
            }
            days.Reverse();

            if (days.Count > 0)
            {
                for (int i = 0; i < days.Count - 1; i++)
                {
                    if (days[i] - days[i + 1] == 1) strike++;
                    else break;
                }
            }
            else strike = 0;

            if (strike > 1)
            {
                strikel.IsVisible = true;
                strikel.Text = $"Страйк: {strike}";
            }
            else strikel.IsVisible = false;

            if (strike <= 3) strikel.TextColor = Colors.Yellow;
            else if (strike <= 10) strikel.TextColor = Colors.Coral;
            else if (strike <= 20) strikel.TextColor = Colors.Red;
            else if (strike <= 50) strikel.TextColor = Colors.Pink;
            else if (strike > 50) strikel.TextColor = Colors.Purple;
            */
        }
    }

    private void exoplanets_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new exo());
    }

    private void cards_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new game());
    }

    private void how_Tapped(object sender, TappedEventArgs e)
    {
        howl.SetAppThemeColor(Label.TextColorProperty, Color.FromArgb("551A8B"), Color.FromArgb("db96f2"));
        Navigation.PushAsync(new web("game.html"));
    }

    private void book_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new book());
    }
}