namespace PlanetPedia;

public partial class moons : ContentPage
{
    string empty = "sunmoons.txt";
	public moons()
	{
		InitializeComponent();
	}

    private void other_Clicked(object sender, EventArgs e)
    {
        other.SetAppThemeColor(Label.TextColorProperty, Color.FromArgb("551A8B"), Color.FromArgb("db96f2"));
        Navigation.PushAsync(new web("https://ru.wikipedia.org/wiki/%D0%A1%D0%BF%D1%83%D1%82%D0%BD%D0%B8%D0%BA%D0%B8_%D0%B2_%D0%A1%D0%BE%D0%BB%D0%BD%D0%B5%D1%87%D0%BD%D0%BE%D0%B9_%D1%81%D0%B8%D1%81%D1%82%D0%B5%D0%BC%D0%B5"));
    }

    private void moonb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("moon.txt", empty));
    }

    private void fobosb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("fobos.txt", empty));
    }

    private void deimosb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("deimos.txt", empty));
    }

    private void iob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("io.txt", empty));
    }

    private void callisto_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("callisto.txt", empty));
    }

    private void ganymedeb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("ganymede.txt", empty));
    }

    private void europab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("europa.txt", empty));
    }

    private void titanb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("titan.txt", empty));
    }

    private void mimasb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("mimas.txt", empty));
    }

    private void enceladusb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("enceladus.txt", empty));
    }

    private void reyab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("reya.txt", empty));
    }

    private void yapetb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("yapet.txt", empty));
    }

    private void mirandab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("miranda.txt", empty));
    }

    private void titaniab_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("titania.txt", empty));
    }

    private void oberonb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("oberon.txt", empty));
    }

    private void tritoneb_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new card("tritone.txt", empty));
    }

    private void earthview_Clicked(object sender, EventArgs e)
    {
        if (moonview.IsVisible)
        {
            moonview.IsVisible = false;
            earthview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            moonview.IsVisible = true;
            earthview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void marsview_Clicked(object sender, EventArgs e)
    {
        if (fobosview.IsVisible)
        {
            fobosview.IsVisible = false;
            deimosview.IsVisible = false;
            marsview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            fobosview.IsVisible = true;
            deimosview.IsVisible = true;
            marsview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void jupiterview_Clicked(object sender, EventArgs e)
    {
        if (ioview.IsVisible)
        {
            ioview.IsVisible = false;
            callistoview.IsVisible = false;
            ganymedeview.IsVisible = false;
            europaview.IsVisible = false;
            jupiterview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            ioview.IsVisible = true;
            callistoview.IsVisible = true;
            ganymedeview.IsVisible = true;
            europaview.IsVisible = true;
            jupiterview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void saturnview_Clicked(object sender, EventArgs e)
    {
        if (titanview.IsVisible)
        {
            titanview.IsVisible = false;
            mimasview.IsVisible = false;
            enceladusview.IsVisible = false;
            reyaview.IsVisible = false;
            yapetview.IsVisible = false;
            saturnview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            titanview.IsVisible = true;
            mimasview.IsVisible = true;
            enceladusview.IsVisible = true;
            reyaview.IsVisible = true;
            yapetview.IsVisible = true;
            saturnview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void uranusview_Clicked(object sender, EventArgs e)
    {
        if (mirandaview.IsVisible)
        {
            mirandaview.IsVisible = false;
            titaniaview.IsVisible = false;
            oberonview.IsVisible = false;
            uranusview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            mirandaview.IsVisible = true;
            titaniaview.IsVisible = true;
            oberonview.IsVisible = true;
            uranusview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void neptuneview_Clicked(object sender, EventArgs e)
    {
        if (tritoneview.IsVisible)
        {
            tritoneview.IsVisible = false;
            neptuneview.BackgroundColor = Microsoft.Maui.Graphics.Colors.LightGray;
        }
        else
        {
            tritoneview.IsVisible = true;
            neptuneview.BackgroundColor = Microsoft.Maui.Graphics.Colors.Transparent;
        }
    }

    private void neirob_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new neiro());
    }
}