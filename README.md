<html>
<head>
  <script src="https://cdn.jsdelivr.net/npm/apexcharts"></script>
  <style>
    #chart1 {
      width: 300px;
      margin: 0 auto;
    }
  </style>
</head>

<body>
  <div id="chart1"></div>

  <script>
    var options1 = {
      chart: {
        height: 280,
        type: "radialBar",
      },
      series: [67, 84, 97, 61],
      plotOptions: {
        radialBar: {
          dataLabels: {
            total: {
              show: true,
              label: 'TOTAL'
            }
          }
        }
      },
      labels: ['TEAM A', 'TEAM B', 'TEAM C', 'TEAM D']
    };

    new ApexCharts(document.querySelector("#chart1"), options1).render();
  </script>
</body>
</html>
