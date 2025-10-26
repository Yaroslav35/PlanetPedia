using Microsoft.Maui.Layouts;

namespace PlanetPedia;

public partial class pictureview : ContentPage
{
    double currentScale = 1;
    double startScale = 1;
    double lastX, lastY;
    bool isZooming = false;

    public pictureview(string path, string desc, string redirect)
    {
        InitializeComponent();
        descl.Text = desc;
        from.Text = "Страница: " + redirect;
        pic.Source = ImageSource.FromFile(path);

        // Настраиваем изображение в AbsoluteLayout
        SetupImageInAbsoluteLayout();
        SetupGestures();
    }

    private void SetupImageInAbsoluteLayout()
    {
        // Устанавливаем изображение по центру AbsoluteLayout
        AbsoluteLayout.SetLayoutBounds(pic, new Rect(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
        AbsoluteLayout.SetLayoutFlags(pic, AbsoluteLayoutFlags.PositionProportional);

        // Убеждаемся, что изображение заполняет контейнер
        pic.Aspect = Aspect.AspectFit;
        pic.HorizontalOptions = LayoutOptions.Center;
        pic.VerticalOptions = LayoutOptions.Center;
    }

    private void SetupGestures()
    {
        // Очищаем старые жесты
        pic.GestureRecognizers.Clear();

        // Жест масштабирования
        var pinchGesture = new PinchGestureRecognizer();
        pinchGesture.PinchUpdated += OnPinchUpdated;

        // Жест перемещения
        var panGesture = new PanGestureRecognizer();
        panGesture.PanUpdated += OnPanUpdated;

        // Двойное нажатие для сброса
        var doubleTapGesture = new TapGestureRecognizer
        {
            NumberOfTapsRequired = 2
        };
        doubleTapGesture.Tapped += OnDoubleTapped;

        // Добавляем жесты к изображению
        pic.GestureRecognizers.Add(pinchGesture);
        pic.GestureRecognizers.Add(panGesture);
        pic.GestureRecognizers.Add(doubleTapGesture);
    }

    private void OnPinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
    {
        switch (e.Status)
        {
            case GestureStatus.Started:
                isZooming = true;
                startScale = pic.Scale;

                // Устанавливаем точку привязки в центр жеста масштабирования
                pic.AnchorX = e.ScaleOrigin.X;
                pic.AnchorY = e.ScaleOrigin.Y;
                break;

            case GestureStatus.Running:
                if (isZooming)
                {
                    currentScale = startScale * e.Scale;

                    // Ограничиваем масштаб
                    currentScale = Math.Max(0.5, Math.Min(5, currentScale));

                    pic.Scale = currentScale;
                }
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                isZooming = false;
                // Сохраняем текущую позицию после масштабирования
                lastX = pic.TranslationX;
                lastY = pic.TranslationY;
                break;
        }
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        // Если активно масштабирование - игнорируем перемещение
        if (isZooming) return;

        // Разрешаем перемещение только при масштабе > 1
        if (pic.Scale <= 1) return;

        switch (e.StatusType)
        {
            case GestureStatus.Started:
                lastX = pic.TranslationX;
                lastY = pic.TranslationY;
                break;

            case GestureStatus.Running:
                // Вычисляем новые координаты
                double newX = lastX + e.TotalX;
                double newY = lastY + e.TotalY;

                // Ограничиваем перемещение границами контейнера
                var container = imageContainer;
                var maxTranslationX = (container.Width * (pic.Scale - 1)) / 2;
                var maxTranslationY = (container.Height * (pic.Scale - 1)) / 2;

                if (maxTranslationX > 0)
                    pic.TranslationX = Math.Max(-maxTranslationX, Math.Min(maxTranslationX, newX));

                if (maxTranslationY > 0)
                    pic.TranslationY = Math.Max(-maxTranslationY, Math.Min(maxTranslationY, newY));
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                // Сохраняем конечную позицию
                lastX = pic.TranslationX;
                lastY = pic.TranslationY;
                break;
        }
    }

    private void OnDoubleTapped(object sender, EventArgs e)
    {
        // Сбрасываем флаг масштабирования
        isZooming = false;

        // Анимация сброса к исходному состоянию
        pic.AnchorX = 0.5;
        pic.AnchorY = 0.5;

        pic.ScaleTo(1, 250, Easing.SpringOut);
        pic.TranslateTo(0, 0, 250, Easing.SpringOut);

        currentScale = 1;
        lastX = lastY = 0;
    }
}