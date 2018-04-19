function CreateSalesSummary() {
    Chart.defaults.global.legend.labels.usePointStyle = true;
    var chrt = document.getElementById("saleschart").getContext("2d");
   
    var data = {
        labels: lbls, //x-axis
        datasets: [
             {
                 legendText: "Month wise Sales",
                 label: "Last Year", //optional
                 fillColor: "rgba(100, 218, 7,0.8)",
                 strokeColor: "rgba(100, 218, 7,0.8)",
                 highlightFill: "rgba(100, 218, 7,0.75)",
                 highlightStroke: "rgba(100, 218, 7,1)",
                 data: dtaLastYear,// y-axis
                 backgroundColor: "rgba(100, 218, 7,0.5)"
             },
        {
            legendText: "Month wise Sales",
            label: "This Year", //optional
            fillColor: "rgba(180, 180, 7,0.8)",
            strokeColor: "rgba(180, 180, 7,0.8)",
            highlightFill: "rgba(180, 180, 7,0.75)",
            highlightStroke: "rgba(180, 180, 7,1)",
            data: dta,// y-axis
            backgroundColor: "rgba(180, 180, 7,0.5)"
        }
        
        ]

    };

     
    options = {
        scales: {
            xAxes: [{
                gridLines: {
                    display: false
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false,
                    color: "rgba(1,1, 1, .1)",

                }
            }]
        },
        tooltips: {
            callbacks: {
                label: function (tooltipItem, data) {
                    var label = data.datasets[tooltipItem.datasetIndex].label || '';

                    if (label) {
                        label += ': ';
                    }
                    label += tooltipItem.yLabel + 'Lakhs';
                    return label;
                }
            }
        }
    }

    var myBarChart = new Chart(chrt, {
        type: 'bar',
        data: data,
        options: options
    });

    //  var saleschart = new Chart(chrt).Bar(data, options);

}

$(function () {
   
    CreateSalesSummary();
    $('#salessummarydiv').fadeIn();   
   // $('#salessummarydiv').css({ opacity: 1, visibility: "visible" }).animate({ opacity: 1 }, 500);
});