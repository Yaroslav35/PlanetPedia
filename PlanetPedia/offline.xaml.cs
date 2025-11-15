using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Net.NetworkInformation;
#if WINDOWS
using Windows.Services.Store;
#elif ANDROID
using Android.Content;
using Android.OS;
#endif

namespace PlanetPedia;

public partial class offline : ContentPage
{
    string fullPath = "";
    Dictionary<string, string> names = new Dictionary<string, string>();
    Dictionary<CheckBox, string> tags = new Dictionary<CheckBox, string>();
    bool internet = false;
    public offline()
	{
		InitializeComponent();
        tags = new Dictionary<CheckBox, string>()
        {
            {sun,"sun"},
            {mercury,"mercury"},
            {venus,"venus"},
            {earth,"earth"},
            {mars,"mars"},
            {jupiter,"jupiter"},
            {saturn,"saturn"},
            {uranus,"uranus"},
            {neptune,"neptune"},
            {moon,"moon"},
            {io,"io"},
            {callisto,"callisto"},
            {ganymede,"ganymede"},
            {europa,"europa"},
            {titan,"titan"},
            {enceladus,"enceladus"},
            {yapet,"yapet"},
            {titania,"titania"},
            {oberon,"oberon"},
            {tritone,"tritone"},
            {voyager, "voyager"},
            {ton, "ton"}
        };

#if WINDOWS
		window_load();
#elif ANDROID
        android_load();
#endif
    }

	private void window_load()
	{
#if WINDOWS
        string userFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile), "PlanetPedia");
        if (!Directory.Exists(userFolder)) Directory.CreateDirectory(userFolder);
		DirectoryInfo directoryInfo = new DirectoryInfo(userFolder);
		foreach (FileInfo fi in directoryInfo.GetFiles()) names.Add(Path.GetFileNameWithoutExtension(fi.Name), fi.Extension);
		foreach(KeyValuePair<string, string> keyValuePair in names)
		{
			if (!new string[5] { ".mp4", ".avi", ".mov", ".mkv", ".webm" }.Contains(keyValuePair.Value)) names.Remove(keyValuePair.Key);
		}
        checkboxed(names);
#endif
    }

    private void android_load()
    {
        string dir = "";
#if ANDROID
        var context = Android.App.Application.Context;

        // Для Android 10+ (API 29+) используем Scoped Storage
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
        {
            dir = context.GetExternalFilesDir(null)?.AbsolutePath;
        }
        else
        {
            // Для более старых версий
            var externalPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;
            var packageName = context.PackageName;
            dir =  Path.Combine(externalPath, "Android", "data", packageName);
        }

        try
    {
        var directoryPath = dir;
        
        if (!string.IsNullOrEmpty(directoryPath))
        {
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                Console.WriteLine($"Папка создана: {directoryPath}");
            }
            else
            {
                Console.WriteLine($"Папка уже существует: {directoryPath}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при создании папки: {ex.Message}");
    }

    var basePath = dir;
    fullPath = Path.Combine(basePath, "offline");
    
    if (!Directory.Exists(fullPath))
    {
        Directory.CreateDirectory(fullPath);
    }
    DirectoryInfo directoryInfo = new DirectoryInfo(fullPath);
    foreach (FileInfo fi in directoryInfo.GetFiles()) names.Add(Path.GetFileNameWithoutExtension(fi.Name), fi.Extension);
		foreach(KeyValuePair<string, string> keyValuePair in names)
		{
			if (!new string[5] { ".mp4", ".avi", ".mov", ".mkv", ".webm" }.Contains(keyValuePair.Value)) names.Remove(keyValuePair.Key);
		}
        checkboxed(names);
#endif
    }

    //Сюда аргументом передаём словарь парой (имя; расширение файла)
    private void checkboxed(Dictionary<string, string> names)
	{
        foreach (string name in names.Keys)
        {
            switch (name)
            {
                case "sun":
                    sun.IsChecked = true;
                    break;
                case "mercury":
                    mercury.IsChecked = true;
                    break;
                case "venus":
                    venus.IsChecked = true;
                    break;
                case "earth":
                    earth.IsChecked = true;
                    break;
                case "mars":
                    mars.IsChecked = true;
                    break;
                case "jupiter":
                    jupiter.IsChecked = true;
                    break;
                case "saturn":
                    saturn.IsChecked = true;
                    break;
                case "uranus":
                    uranus.IsChecked = true;
                    break;
                case "neptune":
                    neptune.IsChecked = true;
                    break;
                case "moon":
                    moon.IsChecked = true;
                    break;
                case "io":
                    io.IsChecked = true;
                    break;
                case "callisto":
                    callisto.IsChecked = true;
                    break;
                case "ganymede":
                    ganymede.IsChecked = true;
                    break;
                case "europa":
                    europa.IsChecked = true;
                    break;
                case "titan":
                    titan.IsChecked = true;
                    break;
                case "enceladus":
                    enceladus.IsChecked = true;
                    break;
                case "yapet":
                    yapet.IsChecked = true;
                    break;
                case "titania":
                    titania.IsChecked = true;
                    break;
                case "oberon":
                    oberon.IsChecked = true;
                    break;
                case "tritone":
                    tritone.IsChecked = true;
                    break;
                case "voyager":
                    voyager.IsChecked = true;
                    break;
                case "ton":
                    ton.IsChecked = true;
                    break;

            }
        }
    }

    private async void internet_check()
    {
        using var ping = new Ping();
        var reply = await ping.SendPingAsync("8.8.8.8", 100);

        try
        {
            if (reply.Status == IPStatus.Success) internet = true;
            else internet = false;
        }
        catch
        {
            internet = false;
        }
    }

    private void apply_Clicked(object sender, EventArgs e)
    {
        List<string> add = new List<string>();
        List<string> delete = new List<string>();
        List<CheckBox> checks = new List<CheckBox>() {sun, mercury, venus, earth, mars, jupiter, saturn, uranus, 
            neptune, moon, io, callisto, ganymede, europa, titan, enceladus,
            yapet, titania, oberon, tritone, voyager, ton};
        foreach(CheckBox check in checks)
        {
            if(check.IsChecked)
            {
                if (!names.ContainsKey(tags[check]))
                {
                    add.Add(tags[check]);
                }
            }
            else
            {
                if (names.ContainsKey(tags[check]))
                {
                    delete.Add(tags[check]);
                }
            }
        }

        internet_check();
        if (internet) Navigation.PushModalAsync(new download(add, delete, fullPath));
    }
}