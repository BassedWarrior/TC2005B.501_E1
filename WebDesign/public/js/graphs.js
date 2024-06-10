/**
 * @param {number} alpha Indicated the transparency of the color
 * @returns {string} A string of the form 'rgba(240, 50, 123, 1.0)' that represents a color
 */

function random_color(alpha=1.0)
{
    const r_c = () => Math.round(Math.random() * 255)
    return `rgba(${r_c()}, ${r_c()}, ${r_c()}, ${alpha}`
}

Chart.defaults.font.size = 16;

try
{
    const levels_response = await fetch(`http://localhost:${port}/timePLayed`,{method: 'GET'})

    if(levels_response.ok)
    {
        console.log('Response is ok. Converting to JSON.')

        let results = await levels_response.json()

        console.log(results)
        console.log('Data converted correctly. Plotting chart.')

        // In this case, we just separate the data into different arrays using the map method of the values array. This creates new arrays that hold only the data that we need.
        const level_names = results.map(e => e['name'])
        const level_colors = results.map(e => random_color(0.8))
        const level_borders = results.map(e => 'rgba(0, 0, 0, 1.0)')
        const level_completion = results.map(e => e['completion_rate'])

        const ctx_levels1 = document.getElementById('apiChart1').getContext('2d');
        const levelChart1 = new Chart(ctx_levels1, 
            {
                type: 'pie',
                data: {
                    labels: level_names,
                    datasets: [
                        {
                            label: 'Completion Rate',
                            backgroundColor: level_colors,
                            borderColor: level_borders,
                            data: level_completion
                        }
                    ]
                }
            })
        /*
        const ctx_levels2 = document.getElementById('apiChart2').getContext('2d');
        const levelChart2 = new Chart(ctx_levels2, 
            {
                type: 'line',
                data: {
                    labels: level_names,
                    datasets: [
                        {
                            label: 'Completion Rate',
                            backgroundColor: level_colors,
                            pointRadius: 10,
                            data: level_completion
                        }
                    ]
                }
            })
        
        const ctx_levels3 = document.getElementById('apiChart3').getContext('2d');
        const levelChart3 = new Chart(ctx_levels3, 
            {
                type: 'bar',
                data: {
                    labels: level_names,
                    datasets: [
                        {
                            label: 'Completion Rate',
                            backgroundColor: level_colors,
                            borderColor: level_borders,
                            borderWidth: 2,
                            data: level_completion
                        }
                    ]
                }
            })*/
    }
}
catch(error)
{
    console.log(error)
}