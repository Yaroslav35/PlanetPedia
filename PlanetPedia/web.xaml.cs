namespace PlanetPedia;

public partial class web : ContentPage
{
	bool modal = false;
	public web(string source_get)
	{
		InitializeComponent();
		webview.Source = source_get;
        if (modal) backl.IsVisible = true;
    }
    public web(string source_get, bool modal_get)
    {
        InitializeComponent();
        webview.Source = source_get;
		modal = modal_get;
		if(modal) backl.IsVisible = true;
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

    private void back_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PopModalAsync();
    }
}