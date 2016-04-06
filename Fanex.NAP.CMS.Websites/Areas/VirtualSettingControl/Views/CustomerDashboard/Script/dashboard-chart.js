/*jshint strict: false*/
/* global Namespace, Highcharts, common, numeral */
////'use strict';

Namespace.Register("Fanex.NAP.CMS.DashboardColumnChart");
Fanex.NAP.CMS.DashboardChart = function () {
    var $this = this;

    $this.ApplyDarkTheme = function () {
        Highcharts.theme = {
            colors: ["#2b908f", "#90ee7e", "#f45b5b", "#7798BF", "#aaeeee", "#ff0066", "#eeaaee", "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
            chart: {
                backgroundColor: {
                    linearGradient: { x1: 0, y1: 0, x2: 1, y2: 1 },
                    stops: [
                        [0, '#2a2a2b'],
                        [1, '#3e3e40']
                     ]
                },
                style: {
                    fontFamily: "'Unica One', sans-serif"
                },
                plotBorderColor: '#606063',
                credits: {
                    enabled: false
                }
            },
            title: {
                style: {
                    color: '#E0E0E3',
                    textTransform: 'uppercase',
                    fontSize: '20px'
                }
            },
            subtitle: {
                style: {
                    color: '#E0E0E3',
                    textTransform: 'uppercase'
                }
            },

            xAxis: {
                gridLineColor: '#707073',
                labels: {
                    style: {
                        color: '#E0E0E3'
                    }
                },
                lineColor: '#707073',
                minorGridLineColor: '#505053',
                tickColor: '#707073',
                title: {
                    style: {
                        color: '#A0A0A3'
                    }
                }
            },
            yAxis: {
                gridLineColor: '#707073',
                labels: {
                    style: {
                        color: '#E0E0E3'
                    }
                },
                lineColor: '#707073',
                minorGridLineColor: '#505053',
                tickColor: '#707073',
                tickWidth: 1,
                title: {
                    style: {
                        color: '#A0A0A3'
                    }
                }
            },
            tooltip: {
                backgroundColor: 'rgba(0, 0, 0, 0.85)',
                style: {
                    color: '#F0F0F0'
                }
            },
            plotOptions: {
                series: {
                    dataLabels: {
                        color: '#B0B0B3'
                    },
                    marker: {
                        lineColor: '#333'
                    }
                },
                boxplot: {
                    fillColor: '#505053'
                },
                candlestick: {
                    lineColor: 'white'
                },
                errorbar: {
                    color: 'white'
                }
            },
            legend: {
                itemStyle: {
                    color: '#E0E0E3'
                },
                itemHoverStyle: {
                    color: '#FFF'
                },
                itemHiddenStyle: {
                    color: '#606063'
                }
            },
            credits: {
                style: {
                    color: '#666'
                }
            },
            labels: {
                style: {
                    color: '#707073'
                }
            },

            drilldown: {
                activeAxisLabelStyle: {
                    color: '#F0F0F3'
                },
                activeDataLabelStyle: {
                    color: '#F0F0F3'
                }
            },

            navigation: {
                buttonOptions: {
                    symbolStroke: '#DDDDDD',
                    theme: {
                        fill: '#505053'
                    }
                }
            },

            // scroll charts
            rangeSelector: {
                buttonTheme: {
                    fill: '#505053',
                    stroke: '#000000',
                    style: {
                        color: '#CCC'
                    },
                    states: {
                        hover: {
                            fill: '#707073',
                            stroke: '#000000',
                            style: {
                                color: 'white'
                            }
                        },
                        select: {
                            fill: '#000003',
                            stroke: '#000000',
                            style: {
                                color: 'white'
                            }
                        }
                    }
                },
                inputBoxBorderColor: '#505053',
                inputStyle: {
                    backgroundColor: '#333',
                    color: 'silver'
                },
                labelStyle: {
                    color: 'silver'
                }
            },

            navigator: {
                handles: {
                    backgroundColor: '#666',
                    borderColor: '#AAA'
                },
                outlineColor: '#CCC',
                maskFill: 'rgba(255,255,255,0.1)',
                series: {
                    color: '#7798BF',
                    lineColor: '#A6C7ED'
                },
                xAxis: {
                    gridLineColor: '#505053'
                }
            },

            scrollbar: {
                barBackgroundColor: '#808083',
                barBorderColor: '#808083',
                buttonArrowColor: '#CCC',
                buttonBackgroundColor: '#606063',
                buttonBorderColor: '#606063',
                rifleColor: '#FFF',
                trackBackgroundColor: '#404043',
                trackBorderColor: '#404043'
            },

            // special colors for some of the
            legendBackgroundColor: 'rgba(0, 0, 0, 0.5)',
            background2: '#505053',
            dataLabelsColor: '#B0B0B3',
            textColor: '#C0C0C0',
            contrastTextColor: '#F0F0F3',
            maskColor: 'rgba(255,255,255,0.3)'
        };

        // Apply the theme
        Highcharts.setOptions(Highcharts.theme);
    };

    $this.ColumnChartOption = function () {
        return {
            chart: {
                type: 'column',
                //margin: 65,
                //marginLeft: 65,
                //marginRight: 35,
                options3d: {
                    enabled: true,
                    alpha: 15,
                    beta: 15,
                    depth: 75,
                    viewDistance: 25
                }
            },
            colors: ['#f28f43', '#1aadce', '#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#492970', '#77a1e5', '#c42525', '#a6c96a'],
            title: {
                style: {
                    color: '#5a7dc1',
                    textTransform: 'uppercase',
                    fontSize: '18px'
                }
            },
            legend: {
                enabled: false
            },
            tooltip: {
                hideDelay: 250
            },
            xAxis: {
                //categories: Highcharts.getOptions().lang.shortMonths
                type: 'category',
                labels: {
                    rotation: -30,
                    formatter: function () {
                        var text = this.value;
                        var formatted = text.length > 9 ? text.substring(0, 9) + '...' : text;

                        return '<div title="' + text + '">' + formatted + '</div>';
                    },
                    style: {
                        fontSize: '12px',
                        fontFamily: 'Verdana, sans-serif'
                    },
                    useHTML: false
                }
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    style: {
                        color: 'black'
                    }
                },
                title: {
                    text: '',
                    style: {
                        color: 'black'
                    }
                },
                opposite: false
            }],
            credits: {
                enabled: false
            },
            plotOptions: {
                column: {
                    depth: 35,
                    colors: ['#7cb5ec', '#B22222', '#90ed7d', '#8085e9', '#f7a35c', '#f15c80', '#e4d354', '#8085e8', '#8d4653', '#91e8e1']
                },
                series: {
                    dataLabels: {
                        enabled: false,
                        color: '#E0E0E3',
                        align: 'right',
                        x: 0,
                        y: -5,
                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                        }
                    }
                }
            }
        };
    };

    $this.PipeChartOption = function () {
        return {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 55,
                    beta: 0
                }
            },
            colors: ['#7cb5ec', '#434348', '#90ed7d', '#f7a35c', '#ea7e70', '#f15c80', '#e4d354', '#8085e8', '#266dbb', '#8d4653', '#91e8e1', '#e44e35'],
            //colors: ['#f28f43', '#1aadce', '#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#492970', '#77a1e5', '#c42525', '#a6c96a'],
            title: {
                style: {
                    color: '#5a7dc1',
                    textTransform: 'uppercase',
                    fontSize: '18px'
                }
            },
            credits: {
                enabled: false
            },
            tooltip: {
                pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>',
                hideDelay: 250
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 45,
                    dataLabels: {
                        enabled: true,
                        distance: 10,

                        style: {
                            fontSize: '13px',
                            fontFamily: 'Verdana, sans-serif'
                            //textShadow: '0 0 3px black'
                        },
                        format: '<b>{point.name}</b>: {point.y: ,.0f}'
                    }
                }
            }
        };
    };

    $this.MixChartOption = function () {
        return {
            chart: {
                type: 'scatter',
                //margin: 65,
                //marginRight: 65,
                options3d: {
                    enabled: true,
                    alpha: 15,
                    beta: 15,
                    depth: 75,
                    viewDistance: 25
                }
            },
            colors: ['#f28f43', '#1aadce', '#2f7ed8', '#0d233a', '#8bbc21', '#910000', '#492970', '#77a1e5', '#c42525', '#a6c96a'],
            title: {
                style: {
                    color: '#5a7dc1',
                    textTransform: 'uppercase',
                    fontSize: '18px'
                }
            },
            legend: {
                enabled: true,
                align: 'center',
                verticalAlign: 'bottom',
                y: 5,
                x: 0,
                itemMarginTop: -20,
                itemStyle: {
                    lineHeight: '10px'
                },
                layout: 'horizontal'
            },
            tooltip: {
                hideDelay: 200,
                shared: false,
                followPointer: false
                //useHTML: true
                //backgroundColor: "rgba(255,255,255,1)"
            },
            xAxis: {
                //categories: Highcharts.getOptions().lang.shortMonths
                type: 'category',
                labels: {
                    rotation: -30,
                    formatter: function () {
                        var text = this.value;
                        var formatted = text.length > 9 ? text.substring(0, 9) + '...' : text;

                        return '<div title="' + text + '">' + formatted + '</div>';

                    },
                    style: {
                        fontSize: '12px',
                        //fontWeight: 'normal',
                        fontFamily: 'Verdana, sans-serif'
                    },
                    useHTML: false
                }
            },
            yAxis: [{ // Primary yAxis
                labels: {
                    //format: '{value}',
                    style: {
                        color: '#f28f43'
                    },
                    formatter: function () {
                        return formatAxis(this);
                    }
                },
                lineWidth: 1,
                title: {
                    text: 'TURNOVER',
                    style: {
                        color: '#f28f43'
                    }
                },
                opposite: false
            }, { // Secondary yAxis
                lineWidth: 0.5,
                title: {
                    text: 'WIN LOSS',
                    style: {
                        color: '#7cb5ec'
                    }
                },
                labels: {
                    style: {
                        color: '#7cb5ec'
                    },
                    formatter: function () {
                        return formatAxis(this);
                    }
                },
                opposite: true
            }],
            credits: {
                enabled: false
            },
            plotOptions: {
                column: {
                    depth: 35,
                    colors: ['#7cb5ec', '#B22222', '#90ed7d', '#8085e9', '#f7a35c', '#f15c80', '#e4d354', '#8085e8', '#8d4653', '#91e8e1']
                },
                series: {
                    stickyTracking: false,
                    dataLabels: {
                        enabled: false,
                        color: '#E0E0E3',
                        align: 'left',
                        x: 0,
                        y: -5,
                        style: {
                            fontSize: '12px',
                            fontFamily: 'Verdana, sans-serif'
                            //textShadow: '0 0 3px black'
                        }
                    }
                }
            }
        };
    };

    var formatAxis = function (data) {
        if (Math.abs(data.value) >= 1000000000) {
            if (data.value % 1000000000 === 0) {
                return numeral(data.value / 1000000000).format('0,0') + " Bil";
            }
            else {
                return numeral(data.value / 1000000000).format(0, 0.00) + " Bil";
            }
        } else if (Math.abs(data.value) >= 1000000) {
            if (data.value % 1000000 === 0) {
                return numeral(data.value / 1000000).format('0,0') + " M";
            }
            else {
                return numeral(data.value / 1000000).format(0, 0.00) + " M";
            }
        }
        else if (Math.abs(data.value) >= 1000) {
            if (data.value % 1000 === 0) {
                return numeral(data.value / 1000).format('0,0') + " K";
            }
            else {
                return numeral(data.value / 1000).format('0,0') + " K";
            }
        } else {
            return data.value;
        }
    };
};