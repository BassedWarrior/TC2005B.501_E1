function randomColor(alpha = 0.8) {
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
            const topScoresColors = topScoresData.map(() => randomColor());

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
            const lowestDamageColors = lowestDamageData.map(() => randomColor());

            const ctx_lowestDamage = document.getElementById('apiChart2').getContext('2d');
            new Chart(ctx_lowestDamage, {
                type: 'bar',
                data: {
                    labels: lowestDamageUsernames,
                    datasets: [{
                        label: 'Less Damage Taken',
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
            const averageTimePlayedColors = averageTimePlayedData.map(() => randomColor());

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



        const cardCountByCategoryResponse = await fetch(`http://localhost:3000/statistics/card_count_by_category`, { method: 'GET' });
        if (cardCountByCategoryResponse.ok) {
            let cardCountByCategoryResults = await cardCountByCategoryResponse.json();
            console.log('Card count by category:', cardCountByCategoryResults);

            const cardCountByCategoryData = cardCountByCategoryResults.cards;
            const categoryLabels = cardCountByCategoryData.map(e => e['category']);
            const cardCounts = cardCountByCategoryData.map(e => e['count']);
            const categoryColors = cardCountByCategoryData.map(() => randomColor());

            const ctx_cardCountByCategory = document.getElementById('apiChart4').getContext('2d');
            new Chart(ctx_cardCountByCategory, {
                type: 'pie',
                data: {
                    labels: categoryLabels,
                    datasets: [{
                        label: 'Card Count by Category',
                        backgroundColor: categoryColors,
                        borderWidth: 2,
                        data: cardCounts
                    }]
                }
            });
        }
        const averageDamageResponse = await fetch(`http://localhost:3000/statistics/average_damage`, { method: 'GET' });
if (averageDamageResponse.ok) {
    let averageDamageResults = await averageDamageResponse.json();
    console.log('Average damage:', averageDamageResults);

    // Acceder al objeto cards en la respuesta
    const averageDamageData = averageDamageResults.cards[0];

    // Obtener los valores de averageDamageDealt y averageDamageTaken
    const averageDamageLabels = ['Damage Dealt', 'Damage Taken'];
    const averageDamageValues = [averageDamageData.averageDamageDealt, averageDamageData.averageDamageTaken];
    const averageDamageColors = [randomColor(), randomColor()];

    const ctx_averageDamage = document.getElementById('apiChart5').getContext('2d');
    new Chart(ctx_averageDamage, {
        type: 'pie',
        data: {
            labels: averageDamageLabels,
            datasets: [{
                label: 'Average Damage',
                backgroundColor: averageDamageColors,
                borderWidth: 2,
                data: averageDamageValues
            }]
        }
    });
}

const playerRegistrationByYearResponse = await fetch(`http://localhost:3000/statistics/player_registration_by_year`, { method: 'GET' });
        if (playerRegistrationByYearResponse.ok) {
            let playerRegistrationByYearResults = await playerRegistrationByYearResponse.json();
            console.log('Player registration by year:', playerRegistrationByYearResults);

            const playerRegistrationByYearData = playerRegistrationByYearResults.cards;
            const player = playerRegistrationByYearData.map(e => e['player_count']);
            const playerCounts = playerRegistrationByYearData.map(e => e['player_count']);
            const lineColors = randomColor();

            const ctx_playerRegistrationByYear = document.getElementById('apiChart6').getContext('2d');
            new Chart(ctx_playerRegistrationByYear, {
                type: 'line',
                data: {
                    labels: player,
                    datasets: [{
                        label: 'Player Registration by Year',
                        backgroundColor: lineColors,
                        borderWidth: 2,
                        pointRadius: 10,
                        data: playerCounts
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
