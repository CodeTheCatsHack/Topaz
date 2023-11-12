using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Scaffold.Model;
using Topaz.Data.Service;

namespace Topaz.Data;

public class NavMenuDataImportComponent : ComponentBase
{
    [Inject] protected ServiceInfoMeasure ServiceInfoMeasure { get; set; }

    protected ObservableCollection<Measure>? Measures { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var measures = ServiceInfoMeasure.Measures;
        if (measures.Count != 0)
            Measures = new ObservableCollection<Measure>(measures);
        await base.OnInitializedAsync();
    }


    protected void OnNavMeasure(int idMeasure)
    {
    }

    #region Динамический Css

    protected bool _collapseNavMenu = true;
    protected string? NavMenuCssClass => _collapseNavMenu ? "collapse" : null;

    protected void ToggleNavMenu()
    {
        _collapseNavMenu = !_collapseNavMenu;
    }

    #endregion
}