using Blazorise.Charts;
using Microsoft.AspNetCore.Components;
using Scaffold.Model;
using Topaz.Data.Service;

namespace Topaz.Data.Component.Index;

public class IndexComponent : ComponentBase
{
    [Parameter]
    public int measureId { get; set; }

    public Measure? Measure { get; set; }

    public List<Measure>? SidebarMeasures { get; set; }

    public Chart<float> VCMetricsChart { get; set; } = new Chart<float>();

    [Inject]
    public ServiceDataDataBase ServiceDataDataBase { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; }

    protected async Task SaveMeasure()
    {
        await ServiceDataDataBase.SaveMeasure(Measure);
    }

    protected async Task HandleRedraw()
    {
        //List<float> metrics = new List<Measure>() { Measure }
        //    .CastTo<VoiceConnectionMetric>().Select(x => x.VoiceServiceNonAcessibility).ToList();

        List<string> chartLabels = new List<string>();
        List<float> chartData = new List<float>();

        foreach (MeasureGroup group in Measure.MeasureGroups)
        {
            chartLabels.Add(nameof(group.VoiceConnectionMetric.VoiceServiceNonAcessibility) + " " + group.MeasureSubject);
            chartData.Add(group.VoiceConnectionMetric!.VoiceServiceNonAcessibility);

            chartLabels.Add(nameof(group.VoiceConnectionMetric.VoiceServiceCutOfffRatio) + " " + group.MeasureSubject);
            chartData.Add(group.VoiceConnectionMetric!.VoiceServiceCutOfffRatio);

            chartLabels.Add(nameof(group.VoiceConnectionMetric.SpeechQualityCallBasis) + " " + group.MeasureSubject);
            chartData.Add(group.VoiceConnectionMetric!.SpeechQualityCallBasis);

            chartLabels.Add(nameof(group.VoiceConnectionMetric.NegativeMossamplesRatio) + " " + group.MeasureSubject);
            chartData.Add(group.VoiceConnectionMetric!.NegativeMossamplesRatio);
        }

        List<string> backgroundColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 0.2f), ChartColor.FromRgba(54, 162, 235, 0.2f), ChartColor.FromRgba(255, 206, 86, 0.2f), ChartColor.FromRgba(75, 192, 192, 0.2f), ChartColor.FromRgba(153, 102, 255, 0.2f), ChartColor.FromRgba(255, 159, 64, 0.2f) };
        List<string> borderColors = new List<string> { ChartColor.FromRgba(255, 99, 132, 1f), ChartColor.FromRgba(54, 162, 235, 1f), ChartColor.FromRgba(255, 206, 86, 1f), ChartColor.FromRgba(75, 192, 192, 1f), ChartColor.FromRgba(153, 102, 255, 1f), ChartColor.FromRgba(255, 159, 64, 1f) };

        ChartDataset<float> dataSet = new ChartDataset<float>()
        {
            Label = "Voice connection chart",
            Data = chartData,
            BorderWidth = 1,
            BackgroundColor = backgroundColors,
            BorderColor = borderColors
        };

        await VCMetricsChart.Clear();

        await VCMetricsChart.AddLabelsDatasetsAndUpdate(chartLabels, dataSet);
    }

    protected async Task OpenMeasureByIdAsync(int measureId)
    {
        NavigationManager.NavigateTo($"/measure/{measureId}");
        Measure = await ServiceDataDataBase.GetDataMeasureById(measureId);
    }

    protected override async Task OnInitializedAsync()
    {
        Measure = await ServiceDataDataBase.GetDataMeasureById(measureId);
        SidebarMeasures = await ServiceDataDataBase.GetLastTenMeasures();

        await base.OnInitializedAsync();
        await HandleRedraw();

    }
}