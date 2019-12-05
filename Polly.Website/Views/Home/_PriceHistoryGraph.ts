namespace Polly {

    export class PriceHistoryGraph {

        constructor(options: Options) {

            var ctx = <HTMLCanvasElement>document.getElementById("myChart");

            var myChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: options.labels,
                    datasets: [{
                        label: 'Price',
                        data: options.data,
                        backgroundColor: ['rgba(255, 99, 132, 0.2)'],
                        borderColor: ['rgba(255,99,132,1)'],
                        borderWidth: 1
                    }]
                },
                options: {
                    hover: {
                        mode: 'nearest',
                    },
                    scales: {
                        xAxes: [{
                            display: true,
                            scaleLabel: {
                                labelString: 'Date'
                            }
                        }],
                        yAxes: [{
                            display: true,
                            scaleLabel: {
                                labelString: 'Price'
                            },
                            ticks: {
                                beginAtZero: true
                            }
                        }]
                    },
                    maintainAspectRatio: false
                }
            });
        }
    }

    interface Options {
        data: Array<number>;
        labels: Array<string>;
    }
}