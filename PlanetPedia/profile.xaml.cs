using MySqlConnector;
using System.Data.Common;

namespace PlanetPedia;

public partial class profile : ContentPage
{
	bool opened = false;
	public profile()
	{
		InitializeComponent();
		bor1.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
		name.Text = Preferences.Get("name", "USER");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            opened = true;
            load();
        }
        catch { }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        try
        {
            opened = false;
        }
        catch { }
    }

    private async void load()
    {
        try
        {
            if(opened)
            {
                container.Children.Clear();
                List<string> names = new List<string>();
                List<string> imgs = new List<string>();
                List<int> ids = new List<int>();
                using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand($"SELECT name, img, id FROM pages WHERE user_id = '{Preferences.Get("id", 0)}' ORDER BY date DESC;", conn);
                    try
                    {
                        DbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            names.Add(reader.GetString(0));
                            imgs.Add(reader.GetString(1));
                            ids.Add(reader.GetInt32(2));
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                for(int i = 0;  i < names.Count; i++)
                {
                    Border box = new Border()
                    {
                        Margin = new Thickness(10),
                        Stroke = Colors.Black,
                        BackgroundColor = Color.FromRgba(105, 108, 138, 0.3),
                        WidthRequest = 330,
                        HeightRequest = 350
                    };
                    container.Children.Add(box);
                    VerticalStackLayout vert = new VerticalStackLayout();
                    box.Content = vert;
                    Label title = new Label()
                    {
                        Text = names[i],
                        TextColor = Colors.Black,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontAttributes = FontAttributes.Bold,
                        FontSize = 24,
                        Margin = new Thickness(0,10,0,0)
                    };
                    vert.Children.Add(title);
                    Image img = new Image()
                    {
                        Source = ImageSource.FromUri(new Uri(imgs[i])),
                        HeightRequest = 170,
                        Margin = new Thickness(0,30,0,0)
                    };
                    vert.Children.Add(img);
                    HorizontalStackLayout hor = new HorizontalStackLayout()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Margin = new Thickness(0,35,0,0)
                    };
                    vert.Children.Add(hor);
                    Button go = new Button
                    {
                        Text = "Открыть",
                        TextColor = Colors.White,
                        WidthRequest = 90,
                        BorderColor = Colors.Black,
                        BorderWidth = 2,
                        BindingContext = ids[i]
                    };
                    go.Clicked += Go_Clicked;
                    hor.Children.Add(go);
                    Button edit = new Button
                    {
                        Text = "Изменить",
                        TextColor = Colors.White,
                        WidthRequest = 100,
                        Margin = new Thickness(10,0),
                        BorderColor = Colors.Black,
                        BorderWidth = 2,
                        BindingContext = ids[i]
                    };
                    edit.Clicked += Edit_Clicked;
                    hor.Children.Add(edit);
                    Button delete = new Button
                    {
                        Text = "Удалить",
                        TextColor = Colors.White,
                        WidthRequest = 90,
                        BorderColor = Colors.Black,
                        BorderWidth = 2,
                        BindingContext = ids[i]
                    };
                    delete.Clicked += Delete_Clicked;
                    hor.Children.Add(delete);
                }
            }
        }
        catch
        {
            await Task.Delay(5000);
            load();
        }
    }

    private void Edit_Clicked(object? sender, EventArgs e)
    {
        int id = int.Parse((sender as Button).BindingContext.ToString());
        Navigation.PushAsync(new edit(id));
    }

    private async void Delete_Clicked(object? sender, EventArgs e)
    {
        if ((sender as Button).Text == "Удалить")
        {
            (sender as Button).BackgroundColor = Colors.DarkRed;
            (sender as Button).Text = "Точно?";
        }
        else
        {
            using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
            {
                await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand($"DELETE FROM pages WHERE id = {int.Parse((sender as Button).BindingContext.ToString())}", conn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { }
            }
            load();
        }
    }

    private void Go_Clicked(object? sender, EventArgs e)
    {
        int id = int.Parse((sender as Button).BindingContext.ToString());
        Navigation.PushAsync(new card("","",false,id));
    }

    private async void add_Clicked(object sender, EventArgs e)
    {
        using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
        {
            await conn.OpenAsync();
            MySqlCommand cmd = new MySqlCommand($"INSERT INTO pages VALUES (NULL, 'Страница', 'https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/PM0xbWVu32cLTw','','','','','','','','{DateTime.Now.ToString("yyyy.MM.dd")}',{Preferences.Get("id", 0)});", conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }
        load();
    }
}