using Microsoft.Maui.Controls.PlatformConfiguration;
using MySqlConnector;
using System.Data.Common;
using System.Xml.Linq;
using static ZXing.QrCode.Internal.Mode;

namespace PlanetPedia;

public partial class edit : ContentPage
{
	int id = 0;
    bool opened = false;
	public edit(int id_get)
	{
		InitializeComponent();
		id = id_get;

        propertybox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        aboutbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        uniquebox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        picbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        vidbox.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        box3d.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        tests.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            opened = true;
            load();
        }
        catch (Exception ex)
        {
            Console.WriteLine();
        }
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
        await Task.Delay(1);
        try
        {
            if(opened)
            {
                using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand($"SELECT * FROM pages WHERE id = {id};", conn);
                    try
                    {
                        DbDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            title.Text = reader.GetString(1);
                            img.Source = ImageSource.FromUri(new Uri(reader.GetString(2)));
                            imge.Text = reader.GetString(2);
                            prope.Text = reader.GetString(3);
                            aboute.Text = reader.GetString(4);
                            uniquee.Text = reader.GetString(5);
                            pice.Text = reader.GetString(6);
                            vide.Text = reader.GetString(7);
                            obje.Text = reader.GetString(8);
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
        catch { }
    }

    private async void save_Clicked(object sender, EventArgs e)
    {
        using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
        {
            await conn.OpenAsync();
            if (string.IsNullOrEmpty(imge.Text) || !imge.Text.Contains("http")) imge.Text = "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/PM0xbWVu32cLTw";
            MySqlCommand cmd = new MySqlCommand($"UPDATE pages SET `name`='{title.Text}', `img`='{imge.Text}', `properties`='{prope.Text}', `info`='{aboute.Text}', `facts`='{uniquee.Text}', `gallery`='{pice.Text}', `video`='{vide.Text}', `obj3d`='{obje.Text}' WHERE `id`={id}", conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { }
        }
    }
}