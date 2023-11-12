using Microsoft.AspNetCore.Components;
using Scaffold.Model;
using Topaz.Data.Service;

namespace Topaz.Data.Component.Index;

public class IndexComponent : ComponentBase
{
    [Parameter] public string IdMeasure { get; set; }

    public Measure? Measure { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (int.TryParse(IdMeasure, out var paramIdMeasure))
            Measure = ServiceInfoMeasure.Measures.FirstOrDefault(x => x.IdMeasure == paramIdMeasure);
        await base.OnInitializedAsync();
    }
}