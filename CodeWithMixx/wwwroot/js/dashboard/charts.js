
export function initCharts() {
   
    getFinanceChart();
    getSubjectsChart();
    
}

function getFinanceChart() {
    const ctx = document.getElementById('finance-chart').getContext('2d');
    

    const financeChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'Maj', 'Jun'],
            datasets: [{
                label: 'Zarada (RSD)',
                data: [35000, 42000, 60000, 55000, 72000, 85000],
                borderColor: '#34d399',
                backgroundColor: 'rgba(52, 211, 153, 0.1)',
                borderWidth: 3,
                tension: 0.4,
                pointBackgroundColor: '#fbbf24'
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
                    ticks: { color: '#a7f3d0' }
                },
                y: {
                    grid: { color: 'rgba(6, 78, 59, 0.2)' },
                    ticks: { color: '#a7f3d0' }
                }
            }
        }
    });

}

function getSubjectsChart() {
    const ctx = document.getElementById('subjects-chart').getContext('2d');


    const subjectsChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['OOP (C# / Java)', 'Osnove C-a', 'Baze podataka', 'Web Dev', "Desktop App"],
            datasets: [{
                label: 'Broj održanih časova',
                data: [25, 18, 12, 15, 9],

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
                    display: true,
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