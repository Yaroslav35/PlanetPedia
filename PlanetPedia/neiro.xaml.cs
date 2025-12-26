namespace PlanetPedia;

public partial class neiro : ContentPage
{
    bool anim = false;
	public neiro()
	{
		InitializeComponent();
        List<VisualElement> elements = new List<VisualElement>() {block, f1, f2, f3, f4, f5, f6, f7};

        foreach (VisualElement element in elements) element.Opacity = 0;
        anim = true;

		f1.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f2.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f3.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f4.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f5.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f6.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
        f7.BackgroundColor = Color.FromRgba(105, 108, 138, 0.3);
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        List<VisualElement> elements = new List<VisualElement>() {block, f1, f2, f3, f4, f5, f6, f7 };

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

    private void kepler_Clicked(object sender, EventArgs e)
    {
		Navigation.PushAsync(new card("kepler22.txt","sunmoons.txt", true, 0));
    }
}