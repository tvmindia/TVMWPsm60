﻿function CreateEnquiryFollowupSummary() {
    Chart.defaults.global.legend.labels.usePointStyle = true;
    var chrt = document.getElementById("enquiryChart").getContext("2d");
    var colors = [];
    var i;
    for (i = 0; i < enqFlw.length; i++) {
        if (parseInt( enqFlw[i]) == 0) {

            colors.push("rgb(250,128,114)")
        }
        else {

            colors.push("rgb(50,230,127)")
        }
    }

    var data = {
        labels: enqLbl, //x-axis
        datasets: [
             {
                 legendText: "",
                 label: "Followups Done", //optional
                 fillColor: "rgba(100, 218, 7,0.8)",
                 strokeColor: "rgba(100, 218, 7,0.8)",
                 highlightFill: "rgba(100, 218, 7,0.75)",
                 highlightStroke: "rgba(100, 218, 7,1)",
                 data: enqData,// y-axis
                 backgroundColor: colors
             } 

        ]

    };


    options = {
        scales: {
            xAxes: [{
                gridLines: {
                    display: false
                }
                ,
                ticks: {
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
                        label += ' done ';
                    }
                    label = "Enquiry Value : ₹" + formatCurrency(tooltipItem.yLabel);
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

    CreateEnquiryFollowupSummary();
    $('#enqsummaryDiv').fadeIn();
    // $('#salessummarydiv').css({ opacity: 1, visibility: "visible" }).animate({ opacity: 1 }, 500);
});