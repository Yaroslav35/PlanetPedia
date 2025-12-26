using MySqlConnector;
using System.Data.Common;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static ZXing.QrCode.Internal.Mode;

namespace PlanetPedia;

public partial class login : ContentPage
{
    bool logintype = true;
    bool opened = false;
    int id = 0;
	public login()
	{
		InitializeComponent();
	}

    private void look1_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
		passe.IsPassword = look1.IsChecked ? false : true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        try
        {
            opened = true;
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

    private void loginb_Clicked(object sender, EventArgs e)
    {
        loginerror.IsVisible = false;
        regerror.IsVisible = false;
        loginb.BackgroundColor = Color.FromArgb("555555");
        regb.BackgroundColor = Color.FromArgb("7b5dc7");
        logintype = true;

        if(logintype)
        {
            loginpanel.IsVisible = true;
            regpanel.IsVisible = false;
        }
        else
        {
            loginpanel.IsVisible = false;
            regpanel.IsVisible = true;
        }
    }

    private void regb_Clicked(object sender, EventArgs e)
    {
        loginerror.IsVisible = false;
        regerror.IsVisible = false;
        regb.BackgroundColor = Color.FromArgb("555555");
        loginb.BackgroundColor = Color.FromArgb("7b5dc7");
        logintype = false;

        if (logintype)
        {
            loginpanel.IsVisible = true;
            regpanel.IsVisible = false;
        }
        else
        {
            loginpanel.IsVisible = false;
            regpanel.IsVisible = true;
        }
    }

    private async void confirmlogin_Clicked(object sender, EventArgs e)
    {
        if (opened)
        {
            string login = logine.Text;
            string password = "";
            string name = "";

            using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
            {
                await conn.OpenAsync();
                MySqlCommand cmd = new MySqlCommand($"SELECT password, name, id FROM users WHERE login = '{login}'", conn);
                try
                {
                    DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        password = reader.GetString(0);
                        name = reader.GetString(1);
                        id = reader.GetInt32(2);
                    }
                    reader.Close();
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            if (CreateSHA256(passe.Text) == password)
            {
                Preferences.Set("login", login);
                Preferences.Set("name", name);
                Preferences.Set("id", id);
                done();
            }
            else loginerror.IsVisible = true;
        }
    }

    private static string CreateSHA256(string input)
    {
        using SHA256 hash = SHA256.Create();
        return Convert.ToHexString(hash.ComputeHash(Encoding.ASCII.GetBytes(input))).ToUpper();
    }

    private void done()
    {
        switcher.IsVisible = false;
        loginpanel.IsVisible = false;
        regpanel.IsVisible = false;

        Label end = new Label()
        {
            Text = "Готово! Теперь вы можете вернуться!",
            Margin = new Thickness(10),
            HorizontalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 20
        };
        main.Children.Add(end);
    }

    private void look2_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        passrege.IsPassword = look2.IsChecked ? false : true;
        confirme.IsPassword = look2.IsChecked ? false : true;
    }

    private async void confirmreg_Clicked(object sender, EventArgs e)
    {
        if(passrege.Text == confirme.Text)
        {
            if(!string.IsNullOrEmpty(loginrege.Text) && !string.IsNullOrEmpty(namee.Text) && !string.IsNullOrEmpty(passrege.Text) && !string.IsNullOrEmpty(confirme.Text))
            {
                using (var conn = new MySqlConnection(SQLClass.CONNECTION_STRING))
                {
                    await conn.OpenAsync();
                    MySqlCommand cmd = new MySqlCommand($"INSERT INTO users VALUES (NULL, '{loginrege.Text}','{CreateSHA256(confirme.Text)}','{namee.Text}',0);", conn);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex) { }

                    int idget = 0;
                    MySqlCommand cmd1 = new MySqlCommand($"SELECT id FROM users WHERE login = '{loginrege.Text}'", conn);
                    try
                    {
                        DbDataReader reader = cmd1.ExecuteReader();
                        while (reader.Read())
                        {
                            idget = reader.GetInt32(0);
                        }
                        reader.Close();
                        Console.WriteLine();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }


                    Preferences.Set("login", loginrege.Text);
                    Preferences.Set("name", namee.Text);
                    Preferences.Set("id", idget);
                }
                done();
            }
        }
        else regerror.IsVisible = true;
    }
}