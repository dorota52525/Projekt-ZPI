var historicalData = window.model.historicalData;
var keys = Object.keys(historicalData);
var colors = [
    { r: 30, g: 154, b: 204 },
    { r: 68, g: 204, b: 30 },
    { r: 204, g: 30, b: 68 },
    { r: 154, g: 204, b: 30 },
    { r: 110, g: 99, b: 16 },
    { r: 204, g: 30, b: 47 },
    { r: 205, g: 186, b: 30 },
    { r: 231, g: 93, b: 8 }];

if (Object.keys(historicalData).length !== 0) {
    keys.forEach((element,i) => {
        var canv = document.createElement("canvas");
        canv.id = element;
        canv.height = 100;
        canv.width = 300;
        document.getElementById('chart').appendChild(canv); 
        var ctx = document.getElementById([element]).getContext('2d');

        var hist = historicalData[element].map(x => x.date);
        var lab = [];
        hist.forEach(x => lab.push(moment(x).format("YYYY-MM-DD HH:mm:ss")))

        var myChart = new Chart(ctx, {
            type: 'line',
            data: {
                labels: lab,
                datasets: [{
                    label: element.toUpperCase(),
                    data: historicalData[element].map(x => x.value),
                    backgroundColor: [
                        'rgba(' + colors[i].r + ',' + colors[i].g + ',' + colors[i].b + ', 0.1)'
                    ],
                    borderColor: [
                        'rgba(' + colors[i].r + ',' + colors[i].g + ',' + colors[i].b + ', 1)'
                    ],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: true
                        }
                    }]
                }
            }
        });
    })
}