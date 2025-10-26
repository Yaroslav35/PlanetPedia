namespace PlanetPedia;

public partial class web : ContentPage
{
	public web(string source_get)
	{
		InitializeComponent();
		webview.Source = source_get;
	}

    private void webview_Navigating(object sender, WebNavigatingEventArgs e)
    {
		if(e.Url.StartsWith("maui://"))
		{
			e.Cancel = true;
			string action = e.Url.Replace("maui://","");
			if (action == "sun/") Navigation.PushAsync(new card("sun.txt","sunmoons.txt"));
		}
    }
}