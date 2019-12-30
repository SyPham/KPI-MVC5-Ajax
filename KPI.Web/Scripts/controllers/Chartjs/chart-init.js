var DATA_COUNT = 8;
var labels = [];

Samples.srand(8);

for (var i = 0; i < DATA_COUNT; ++i) {
    labels.push('' + i);
}

Chart.helpers.merge(Chart.defaults.global, {
    aspectRatio: 4 / 3,
    tooltips: false,
    layout: {
        padding: {
            top: 10,
            right: 16,
            bottom: 32,
            left: 8
        }
    },
    elements: {
        line: {
            fill: false
        }
    },
    plugins: {
        legend: true,
        title: true
    }
});
