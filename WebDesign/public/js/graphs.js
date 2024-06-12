
function randomColor(alpha = 1.0) {
    const r_c = () => Math.round(Math.random() * 255);
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha})`;
}

Chart.defaults.font.size = 16;

try {
    const levels_response = await fetch(`http://localhost:3000/statistics/topScores`, { method: 'GET' });

    if (levels_response.ok) {
        console.log('Response is ok. Converting to JSON.');

        let results = await levels_response.json();

        console.log(results);
        console.log('Data converted correctly. Plotting chart.');

        // Accede a los datos correctos
        const data = results.cards;

        // Aquí separamos los datos en diferentes arrays usando el método map del array de datos.
        const usernames = data.map(e => e['username']);
        const scores = data.map(e => e['score']);
        const randomColors = data.map(() => randomColor(0.2)); // Genera un color random para cada entrada

        // Configuración del gráfico
        const ctx_levels1 = document.getElementById('apiChart1').getContext('2d');
        const levelChart1 = new Chart(ctx_levels1, {
            type: 'bar', // Cambié el tipo de gráfico a 'bar' para visualizar mejor las puntuaciones
            data: {
                labels: usernames, // Usaremos los nombres de usuario como etiquetas en el gráfico
                datasets: [{
                    label: 'Top Scores',
                    backgroundColor: randomColors, // Usa el array de colores random
                    borderWidth: 1,
                    data: scores // Usaremos las puntuaciones como datos en el gráfico
                }]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true
                    }
                }
            }
        });
    }
} catch (error) {
    console.error('Error fetching data:', error);
}
