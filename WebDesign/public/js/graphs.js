function randomColor(alpha = 1.0) {
    const r_c = () => Math.round(Math.random() * 255);
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha})`;
}

Chart.defaults.font.size = 16;

async function fetchDataAndPlot() {
    try {
        // Fetch top scores
        const topScoresResponse = await fetch(`http://localhost:3000/statistics/topScores`, { method: 'GET' });
        if (topScoresResponse.ok) {
            let topScoresResults = await topScoresResponse.json();
            console.log('Top scores:', topScoresResults);

            const topScoresData = topScoresResults.cards;
            const topScoresUsernames = topScoresData.map(e => e['username']);
            const topScoresValues = topScoresData.map(e => e['score']);
            const topScoresColors = topScoresData.map(() => randomColor(0.2));

            const ctx_topScores = document.getElementById('apiChart1').getContext('2d');
            new Chart(ctx_topScores, {
                type: 'bar',
                data: {
                    labels: topScoresUsernames,
                    datasets: [{
                        label: 'Top Scores',
                        backgroundColor: topScoresColors,
                        borderWidth: 2,
                        data: topScoresValues
                    }]
                }
            });
        }

        // Fetch lowest damage players
        const lowestDamageResponse = await fetch(`http://localhost:3000/statistics/lowestDamagePlayers`, { method: 'GET' });
        if (lowestDamageResponse.ok) {
            let lowestDamageResults = await lowestDamageResponse.json();
            console.log('Lowest damage players:', lowestDamageResults);

            const lowestDamageData = lowestDamageResults.cards;
            const lowestDamageUsernames = lowestDamageData.map(e => e['username']);
            const lowestDamageValues = lowestDamageData.map(e => e['damageTaken']);
            const lowestDamageColors = lowestDamageData.map(() => randomColor(0.2));

            const ctx_lowestDamage = document.getElementById('apiChart2').getContext('2d');
            new Chart(ctx_lowestDamage, {
                type: 'bar',
                data: {
                    labels: lowestDamageUsernames,
                    datasets: [{
                        label: 'Damage Taken',
                        backgroundColor: lowestDamageColors,
                        borderWidth: 2,
                        data: lowestDamageValues
                    }]
                }
            });
        }

        const TimePlayedResponse = await fetch(`http://localhost:3000/statistics/TimePlayed`, { method: 'GET' });
        if (TimePlayedResponse.ok) {

            let averageTimePlayedResults = await TimePlayedResponse.json();
            console.log('Average time played per player:', averageTimePlayedResults);
            const averageTimePlayedData = averageTimePlayedResults.cards;

            const averageTimePlayedUsernames = averageTimePlayedData.map(e => e['username']);
            const averageTimePlayedValues = averageTimePlayedData.map(e => e['average_time_played']);
            const averageTimePlayedColors = averageTimePlayedData.map(() => randomColor(0.2));

            const ctx_averageTimePlayed = document.getElementById('apiChart3').getContext('2d');
            new Chart(ctx_averageTimePlayed, {
                type: 'bar',
                data: {
                    labels: averageTimePlayedUsernames,
                    datasets: [{
                        label: 'Average Time Played',
                        backgroundColor: averageTimePlayedColors,
                        borderWidth: 2,
                        data: averageTimePlayedValues
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
}

fetchDataAndPlot();
