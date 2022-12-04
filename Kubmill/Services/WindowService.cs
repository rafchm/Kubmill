using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace Kubmill.Services;

/// <inheritdoc/>
public class WindowService : IWindowService
{
    private readonly IServiceProvider _serviceProvider;

    public WindowService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public T Show<T>() where T : Window
    {
        var win = _serviceProvider.GetRequiredService<T>();

        win.Show();

        return win;
    }

    /// <inheritdoc/>
    public T Get<T>() where T : Window
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}

