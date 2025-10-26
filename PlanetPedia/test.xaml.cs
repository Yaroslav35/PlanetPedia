using Microsoft.Maui.Controls.PlatformConfiguration;

namespace PlanetPedia;

public partial class test : ContentPage
{
    string[,] data;
    string[] rights;
	string title, dir;
    int count;
    int now = 1;
    public test(string title_get, string dir_get)
    {
        InitializeComponent();
        title = title_get;
        dir = dir_get;
        Title = title;
        string[] content = new string[0];
        #if WINDOWS
            content = File.ReadAllLines("tests/" + dir_get);
        #elif ANDROID
            using var stream = Android.App.Application.Context.Assets.Open(dir_get);
            using var reader = new StreamReader(stream);
            var lines = new List<string>();
            string line;
            while((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }
            content = lines.ToArray();
        #endif
        count = content.Length / 2;
        data = new string[count,2];
        rights = new string[count];

        for(int i = 0; i < content.Length; i+=2)
        {
            data[i/2,0] = content[i];
            data[i/2,1] = content[i + 1];
        }
        num.Text = $"Вопрос {now}/{count}";
        question.Text = data[0, 0];
        a1.Content = data[0, 1].Split(";")[0];
        a2.Content = data[0, 1].Split(";")[1];
        a3.Content = data[0, 1].Split(";")[2];
        a4.Content = data[0, 1].Split(";")[3];
    }
    private void previous_Clicked(object sender, EventArgs e)
    {
        if(now > 1)
        {
            now--;
            num.Text = $"Вопрос {now}/{count}";
            question.Text = data[now - 1, 0];
            a1.Content = data[now - 1, 1].Split(";")[0];
            a2.Content = data[now - 1, 1].Split(";")[1];
            a3.Content = data[now - 1, 1].Split(";")[2];
            a4.Content = data[now - 1, 1].Split(";")[3];
        }
    }

    private void next_Clicked(object sender, EventArgs e)
    {
        if(now < count)
        {
            now++;
            num.Text = $"Вопрос {now}/{count}";
            question.Text = data[now-1, 0];
            a1.Content = data[now-1, 1].Split(";")[0];
            a2.Content = data[now - 1, 1].Split(";")[1];
            a3.Content = data[now - 1, 1].Split(";")[2];
            a4.Content = data[now - 1, 1].Split(";")[3];
        }
    }

    private void finish_Clicked(object sender, EventArgs e)
    {
        int n = 0;
        testview.IsVisible = false;
        answersview.IsVisible = true;
        for(int i = 0;i < count;i++)
        {
            if (rights[i] == data[i, 1].Split(";")[4]) n++;
        }
        result.Text = $"Правильные ответы: {n}/{count}";

        if(dir.Contains("_exp") && (float)n/(float)count >= 0.8)
        {
            if(!Preferences.Get("completed", "").Contains(Title))
            {
                int expe = Preferences.Get("exp", 0);
                Preferences.Set("exp", expe + 5000);
                string comp = Preferences.Get("completed", "");
                Preferences.Set("completed", comp + ";" + Title);
            }
        }

        for(int i = 0; i < count;i++)
        {
            Label name = new Label();
            name.Text = $"Вопрос {i + 1}";
            name.FontSize = 20;
            name.FontAttributes = FontAttributes.Bold;
            name.HorizontalTextAlignment = TextAlignment.Center;
            name.Margin = new Thickness(0,30,0,0);
            answersview.Add(name);

            Label answer = new Label();
            if (!dir.Contains("_control")) answer.Text = $"Правильный ответ: {data[i, 1].Split(";")[4]}";
            else answer.Text = "Правильный ответ: ???";
            answer.FontSize = 16;
            answer.HorizontalTextAlignment = TextAlignment.Center;
            answer.Margin = new Thickness(0, 10, 0, 0);
            answersview.Add(answer);

            Label my = new Label();
            my.Text = $"Ваш ответ: {rights[i]}";
            my.FontSize = 16;
            my.HorizontalTextAlignment = TextAlignment.Center;
            my.Margin = new Thickness(0, 10, 0, 0);
            answersview.Add(my);
        }
        if((float)n/(float)count >= 0.8f)
        {
            string tests = Preferences.Get("tests", "");
            Preferences.Set("tests", tests + ";" + Title);
        }
    }

    private void FlexLayout_SizeChanged(object sender, EventArgs e)
    {
        flexbox.Children.Clear();
        if (flexbox.Width < 379)
        {
            flexbox.Children.Add(previous);
            flexbox.Children.Add(next);
            flexbox.Children.Add(apply);
        }
        else
        {
            flexbox.Children.Add(previous);
            flexbox.Children.Add(apply);
            flexbox.Children.Add(next);
        }
    }

    private void apply_Clicked(object sender, EventArgs e)
    {
        string right = "";
        if (a1.IsChecked) right = a1.Content.ToString();
        if (a2.IsChecked) right = a2.Content.ToString();
        if (a3.IsChecked) right = a3.Content.ToString();
        if (a4.IsChecked) right = a4.Content.ToString();
        rights[now - 1] = right;
        next_Clicked(sender, e);
    }
    
}