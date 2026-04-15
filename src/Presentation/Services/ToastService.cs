namespace Presentation.Services;

public interface IToastService
{
    void ShowSuccess(string message);
    void ShowError(string message);
    void ShowInfo(string message);
    void ShowWarning(string message);
}

public class ToastService : IToastService
{
    public event Action<string, ToastType>? OnShow;

    public void ShowSuccess(string message) => OnShow?.Invoke(message, ToastType.Success);
    public void ShowError(string message) => OnShow?.Invoke(message, ToastType.Error);
    public void ShowInfo(string message) => OnShow?.Invoke(message, ToastType.Info);
    public void ShowWarning(string message) => OnShow?.Invoke(message, ToastType.Warning);
}

public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}
