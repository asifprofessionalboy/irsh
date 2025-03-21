window.onload = function () {
    try {
        var hiddenField3 = document.getElementById('<%= HiddenField3.ClientID %>').value;
        console.log("HiddenField3 value: ", hiddenField3);

        var formattedData = hiddenField3
            .replace(/([{,]\s*)([A-Za-z0-9_]+)(\s*:)/g, '$1"$2"$3') 
            .replace(/(['"])?([a-zA-Z0-9_]+)(['"])?:/g, '"$2":')     
            .replace(/'/g, '"');                                    

        var dataPoints = JSON.parse(formattedData);
        console.log("Parsed data: ", dataPoints);

        var labels = dataPoints.map(dp => `${dp.Month}/${dp.Year}`);
        var l1Data = dataPoints.map(dp => dp.L1);
        var l2Data = dataPoints.map(dp => dp.L2);

        var ctx = document.getElementById('DualLineChart').getContext('2d');
        Chart.register(ChartDataLabels);

        new Chart(ctx, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'L1 Data',
                        data: l1Data,
                        borderColor: 'blue',
                        backgroundColor: 'blue',
                        fill: false,
                        tension: 0.3
                    },
                    {
                        label: 'L2 Data',
                        data: l2Data,
                        borderColor: 'green',
                        backgroundColor: 'green',
                        fill: false,
                        tension: 0.3
                    },
                    {
                        label: 'Average (3)',
                        data: Array(labels.length).fill(3),
                        borderColor: 'red',
                        borderWidth: 1,
                        borderDash: [5, 5],
                        pointRadius: 0,
                        fill: false
                    }
                ]
            },
            options: {
                responsive: true,
                plugins: {
                    datalabels: {
                        color: 'black',
                        anchor: 'end',
                        align: 'bottom',
                        font: {
                            weight: 'bold',
                            size: 10
                        },
                        formatter: (value) => value,
                        display: (context) => context.dataset.label !== 'Average (3)'
                    }
                },
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: { stepSize: 1 }
                    }
                }
            }
        });

    } catch (error) {
        console.error("Error parsing data: ", error.message);
    }
};
