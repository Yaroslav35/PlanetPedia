using Microsoft.Maui.Controls.PlatformConfiguration;

namespace PlanetPedia;

public partial class test : ContentPage
{
    string[,] data;
    string[] rights;
	string title, dir;
    int count;
    int now = 1;
    bool anim = false;
    bool initialized = false;
    List<VisualElement> second = new List<VisualElement>();
    public test(string title_get, string dir_get)
    {
        InitializeComponent();

        List<VisualElement> elements = new List<VisualElement>() {num, question, a1, a2, a3, a4, flexbox, finish, tresh};
        second.Add(result);
        foreach (VisualElement element in elements) element.Opacity = 0;
        anim = true;

        title = title_get;
        dir = dir_get;
        Title = title;

        reset();

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

        initialized = true;
    }

    private async void animating(List<VisualElement> els)
    {
        foreach (VisualElement el in els) el.Opacity = 0;

        foreach (VisualElement element in els)
        {
            float tr = 0f;
            while (tr < 1)
            {
                tr += 0.2f;
                element.Opacity = tr;
                await Task.Delay(2);
            }
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<VisualElement> elements = new List<VisualElement>() { num, question, a1, a2, a3, a4, flexbox, finish, tresh };

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
            reset();

            if (!string.IsNullOrEmpty(rights[now - 1]))
            {
                if (rights[now - 1] == a1.Content.ToString()) a1.IsChecked = true;
                if (rights[now - 1] == a2.Content.ToString()) a2.IsChecked = true;
                if (rights[now - 1] == a3.Content.ToString()) a3.IsChecked = true;
                if (rights[now - 1] == a4.Content.ToString()) a4.IsChecked = true;
            }

            animating(new List<VisualElement>() {num, question, a1, a2, a3, a4});
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
            reset();

            if (!string.IsNullOrEmpty(rights[now - 1]))
            {
                if (rights[now - 1] == a1.Content.ToString()) a1.IsChecked = true;
                if (rights[now - 1] == a2.Content.ToString()) a2.IsChecked = true;
                if (rights[now - 1] == a3.Content.ToString()) a3.IsChecked = true;
                if (rights[now - 1] == a4.Content.ToString()) a4.IsChecked = true;
            }

            animating(new List<VisualElement>() { num, question, a1, a2, a3, a4 });
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
            second.Add(name);
            answersview.Add(name);

            Label answer = new Label();
            if (!dir.Contains("_control")) answer.Text = $"Правильный ответ: {data[i, 1].Split(";")[4]}";
            else answer.Text = "Правильный ответ: ???";
            answer.FontSize = 16;
            answer.HorizontalTextAlignment = TextAlignment.Center;
            answer.Margin = new Thickness(0, 10, 0, 0);
            second.Add(answer);
            answersview.Add(answer);

            Label my = new Label();
            my.Text = $"Ваш ответ: {rights[i]}";
            my.FontSize = 16;
            my.HorizontalTextAlignment = TextAlignment.Center;
            my.Margin = new Thickness(0, 10, 0, 0);
            second.Add(my);
            answersview.Add(my);
        }
        if((float)n/(float)count >= 0.8f)
        {
            string tests = Preferences.Get("tests", "");
            Preferences.Set("tests", tests + ";" + Title);
        }
        animate();
    }

    private async void animate()
    {
        foreach (VisualElement element in second) element.Opacity = 0;
        await Task.Delay(300);
        foreach (VisualElement element in second)
        {
            float tr = 0f;
            while (tr < 1)
            {
                tr += 0.1f;
                element.Opacity = tr;
                await Task.Delay(10);
            }
            await Task.Delay(10);
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

    private void a1_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(initialized)
        {
            if (a1.IsChecked) rights[now - 1] = a1.Content.ToString();
        }
    }

    private void a2_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (initialized)
        {
            if (a2.IsChecked) rights[now - 1] = a2.Content.ToString();
        }
    }

    private void a3_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (initialized)
        {
            if (a3.IsChecked) rights[now - 1] = a3.Content.ToString();
        }
    }

    private void a4_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (initialized)
        {
            if (a4.IsChecked) rights[now - 1] = a4.Content.ToString();
        }
    }

    private void reset()
    {
        a1.IsChecked = false;
        a2.IsChecked = false;
        a3.IsChecked = false;
        a4.IsChecked = false;
    }
    
}