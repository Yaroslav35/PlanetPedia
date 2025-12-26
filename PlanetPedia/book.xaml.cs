using Microsoft.Maui;
//using MySql.Data.MySqlClient;
using MySqlConnector;
using System.Data.Common;
using System.IO;
using System.Text;

namespace PlanetPedia;

public partial class book : ContentPage
{
    bool opened = true;
    List<VisualElement> elements = new List<VisualElement>();
    bool anim = false;
    string login = "";
    string name = "";
	public book()
	{
		InitializeComponent();
        login = Preferences.Get("login", "");
        name = Preferences.Get("name", "");
        anim = true;
        System.Text.EncodingProvider ppp = System.Text.CodePagesEncodingProvider.Instance;
        Encoding.RegisterProvider(ppp);

        if (string.IsNullOrEmpty(login))
        {
            loginb.IsVisible = true;
            panel.IsVisible = false;
        }
        else
        {
            loginb.IsVisible = false;
            panel.IsVisible = true;
        }
    }

    protected override void OnAppearing()
    {
        login = Preferences.Get("login", "");
        name = Preferences.Get("name", "");
        if (string.IsNullOrEmpty(login))
        {
            loginb.IsVisible = true;
            panel.IsVisible = false;
        }
        else
        {
            loginb.IsVisible = false;
            panel.IsVisible = true;
        }
        username.Text = name;
        try
        {
            opened = true;
            load();
        }
        catch (Exception ex) 
        { 
            Console.WriteLine(); 
        }
        base.OnAppearing();
    }
    protected override void OnDisappearing()
    {
        try
        {
            opened = false;
        }
        catch { }
        base.OnDisappearing();
    }

    private void To_Profile(object sender, EventArgs e)
    {
        Navigation.PushAsync(new profile());
    }

    private async void load()
    {
        
            try
            {
                if (opened)
                {
                    container.Children.Clear();
                    List<string> names = new List<string>();
                    List<string> imgs = new List<string>();
                    List<string> dates = new List<string>();
                    List<int> ids = new List<int>();
                    List<int> userids = new List<int>();
                using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name, img, date, id, user_id FROM `pages` WHERE name LIKE '%{search.Text}%' AND name != 'Страница' AND img != 'https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/PM0xbWVu32cLTw';", conn);
                    try
                    {
                        DbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));
                            imgs.Add(reader.GetString(1));
                            dates.Add(reader.GetString(2));
                            ids.Add(reader.GetInt32(3));
                            userids.Add(reader.GetInt32(4));
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                    for (int i = 0; i < names.Count; i++)
                    {
                        string author = "";
                    using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
                    {
                        await conn.OpenAsync();
                        MySqlCommand cmd1 = new MySqlCommand($"SELECT name FROM users WHERE id = '{userids[i]}'", conn);
                        try
                        {
                            DbDataReader reader = cmd1.ExecuteReader();
                            while (reader.Read())
                            {
                                author = reader.GetString(0);
                            }
                            reader.Close();
                            Console.WriteLine();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                        Border pagebox = new Border()
                        {
                            Stroke = Colors.Black,
                            BackgroundColor = Color.FromRgba(105, 108, 138, 0.3),
                            HeightRequest = 350,
                            WidthRequest = 330,
                            Margin = new Thickness(10),
                            Opacity = 0
                        };
                        container.Children.Add(pagebox);
                        elements.Add(pagebox);
                        VerticalStackLayout vert = new VerticalStackLayout();
                        pagebox.Content = vert;
                        Label title = new Label()
                        {
                            Text = names[i],
                            TextColor = Colors.Black,
                            HorizontalTextAlignment = TextAlignment.Center,
                            FontSize = 24,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        vert.Children.Add(title);
                        Label by = new Label()
                        {
                            Text = $"От: {author}",
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0,5,0,0)
                        };
                        vert.Children.Add(by);
                        Image img = new Image()
                        {
                            Source = ImageSource.FromUri(new Uri(imgs[i])),
                            HeightRequest = 150,
                            Margin = new Thickness(0, 20, 0, 0)
                        };
                        vert.Children.Add(img);
                        Button go = new Button()
                        {
                            Text = "Исследовать",
                            TextColor = Colors.White,
                            BorderColor = Colors.Black,
                            BorderWidth = 2,
                            Margin = new Thickness(10, 30, 10, 0),
                            WidthRequest = 150,
                            BindingContext = ids[i]
                        };
                        go.Clicked += Go_Clicked;
                        vert.Children.Add(go);
                        Label date = new Label()
                        {
                            Text = $"Дата написания: {dates[i]}",
                            TextColor = Colors.Black,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Margin = new Thickness(0, 10, 0, 0)
                        };
                        vert.Children.Add(date);
                    }
                    if (!anim) foreach (var el in elements) el.Opacity = 1;
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            await Task.Delay(1);
        
    }

    private void Go_Clicked(object? sender, EventArgs e)
    {
        int id = int.Parse((sender as Button).BindingContext.ToString());
        Navigation.PushAsync(new card("","", false, id));
    }

    private void apply_Clicked(object sender, EventArgs e)
    {
        load();
    }

    private void loginb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new login());
    }

    private void quit_Clicked(object sender, EventArgs e)
    {
        Preferences.Remove("login");
        Preferences.Remove("name");
        Preferences.Remove("id");

        login = Preferences.Get("login", "");
        name = Preferences.Get("name", "");
        if (string.IsNullOrEmpty(login))
        {
            loginb.IsVisible = true;
            panel.IsVisible = false;
        }
        else
        {
            loginb.IsVisible = false;
            panel.IsVisible = true;
        }
        username.Text = name;
    }
}