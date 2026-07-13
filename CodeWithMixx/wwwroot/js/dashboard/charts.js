
export function initCharts() {
    if(!window.location.pathname.includes('/dashboard'))
        return;
    getFinanceChart();
    getSubjectsChart();
    
}
let financeChartInstance = null;

export function getFinanceChart() {
    const ctx = document.getElementById('finance-chart').getContext('2d');
    const studentsDataRaw = document.getElementById('finance-chart').dataset.chartStudents;
    const incomeDataRaw = document.getElementById('finance-chart').dataset.chartIncome;
    const chartData = {
        income: JSON.parse(incomeDataRaw),
        students: JSON.parse(studentsDataRaw)
    };

    if (financeChartInstance !== null) {
        financeChartInstance.destroy();
    }

    financeChartInstance = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
            datasets: [{
                label: 'Zarada (RSD)',
                data: chartData.income,
                borderColor: '#34d399',
                backgroundColor: 'rgba(52, 211, 153, 0.1)',
                borderWidth: 3,
                tension: 0.4,
                pointBackgroundColor: '#fbbf24',
                yAxisID: 'profit'
            },
            {
                label: 'Učenici',
                data: chartData.students,
                borderColor: 'orange',
                borderWidth: 3,
                tension: 0.4,
                pointBackgroundColor: '#fbbf24',
                yAxisID: 'students'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false    
                }
            },
            scales: {
                x: {
                    grid: { color: 'rgba(6, 78, 59, 0.2)' },
                    ticks: { color: '#a7f3d0' },
                },
                profit: {
                    type: 'linear',
                    position: 'left',
                    grid: { color: 'rgba(6, 78, 59, 0.2)' },
                    ticks: { color: '#a7f3d0' }
                },
                students: {
                    type: 'linear',
                    position: 'right',
                    grid: { drawOnChartArea: false },
                    ticks: { color: 'orange' }
                }
            }
        }
    });

}

export function getSubjectsChart() {
    const ctx = document.getElementById('subjects-chart').getContext('2d');
    
    const chartDataRaw = document.getElementById('subjects-chart').dataset.chartData;
    const chartData = JSON.parse(chartDataRaw);
    
    const subjectsChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: Object.keys(chartData),
            datasets: [{
                label: 'Broj održanih časova',
                data: Object.values(chartData),
                backgroundColor: [
                    '#34d399',
                    '#fbbf24',
                    '#818cf8',
                    '#38bdf8',
                    '#a78bfa'
                ],
                borderColor: '#064e3b',
                borderWidth: 2,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false,
                    position: 'right',
                    labels: {
                        color: '#a7f3d0',
                        font: {
                            size: 12,
                            family: 'sans-serif'
                        },
                        boxWidth: 12,
                        padding: 15
                    }
                },
                tooltip: {
                    backgroundColor: '#022c22',
                    titleColor: '#fff',
                    bodyColor: '#a7f3d0',
                    borderColor: '#047857',
                    borderWidth: 1
                }
            },
            scales: {
                x: { display: false },
                y: { display: false }
            }
        }
    });
}