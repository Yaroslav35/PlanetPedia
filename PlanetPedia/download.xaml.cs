using System;
using System.Net;

namespace PlanetPedia;

public partial class download : ContentPage
{
	Dictionary<string, string> urls = new Dictionary<string, string>()
	{
		{"sun", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/72t_PHGoinBpYw"},
		{"mercury", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/_AISAGiOukZsfg"},
        {"venus", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/PNCguW6gYLB2qA"},
        {"earth", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/Ue1qULQh-gECxA"},
        {"mars", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/AYhwVo5dVZUaew"},
        {"jupiter", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/k6-sTFDRxa2kLw"},
        {"saturn", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/Xs5pv-lRZ7wWYw"},
        {"uranus", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/LgOEOd4O5-qqyA"},
        {"neptune", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/-2xl-ZtSEPTgZA"},
        {"moon", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/cwP6kv45W3G2sw"},
        {"io", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/JSuAak0nIyqpMQ"},
        {"callisto", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/R5yF97RDqG-Tvw"},
        {"ganymede", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/nwtSDAshOTDQ7A"},
        {"europa", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/yplw_cZ1VDjgRQ"},
        {"titan", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/ASXqS8hVTGf2cQ"},
        {"enceladus", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/R8Y-12wSX3eb6w"},
        {"yapet", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/DPuQfUySNHaf0A"},
        {"titania", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/1d6C_TRAhT5OcQ"},
        {"oberon", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/pJryik1PPnbV8Q"},
        {"tritone", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/5G4poH5m5QXuNg"},
        {"voyager", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/PshjB2eWgsTCFA"},
        {"ton", "https://getfile.dokpub.com/yandex/get/https://disk.yandex.ru/i/pcc0PFssCKHDPg"}
    };
    List<string> add, delete;
    string android_dir;
	public download(List<string> add_get, List<string> delete_get, string android_dir_get)
	{
		InitializeComponent();
        add = add_get;
        delete = delete_get;
        android_dir = android_dir_get;

        #if WINDOWS
        windows();
        #elif ANDROID
        android();
        #endif
    }

    private async void windows()
    {
#if WINDOWS
        string userFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);

        status.Text = "Удаляем файлы";
        foreach (string filename in delete)
        {
            task.Text = $"Удаляем: {filename}";
            File.Delete(Path.Combine(userFolder, "PlanetPedia", filename + ".mp4"));
            await Task.Delay(500);
        }

        status.Text = "Скачиваем файлы";
        foreach(string filename in add)
        {
            task.Text = $"Скачиваем: {filename}";
            using (WebClient client = new WebClient())
            {
                client.DownloadProgressChanged += (sender, e) =>
                {
                    progres.Text = $"Загружено: {e.ProgressPercentage}%";
                };

                await client.DownloadFileTaskAsync(new Uri(urls[filename]), Path.Combine(userFolder,"PlanetPedia", filename + ".mp4"));
            }
            await Task.Delay(500);
        }

        Navigation.PopModalAsync();
#endif
    }

    private async void android()
    {
        status.Text = "Удаляем файлы";
        foreach (string filename in delete)
        {
            task.Text = $"Удаляем: {filename}";
            File.Delete(Path.Combine(android_dir, filename + ".mp4"));
            await Task.Delay(500);
        }

        status.Text = "Скачиваем файлы";
        foreach (string filename in add)
        {
            task.Text = $"Скачиваем: {filename}";
            using (WebClient client = new WebClient())
            {
                client.DownloadProgressChanged += (sender, e) =>
                {
                    progres.Text = $"Загружено: {e.ProgressPercentage}%";
                };

                await client.DownloadFileTaskAsync(new Uri(urls[filename]), Path.Combine(android_dir, filename + ".mp4"));
            }
            await Task.Delay(500);
        }

        Navigation.PopModalAsync();
    }
}