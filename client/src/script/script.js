// [Sidebar]

function configureSidebar()
{
    document.getElementById('outside-sidebar-button').addEventListener('click', () => {
        openSidebar();
    });

    document.getElementById('inside-sidebar-button').addEventListener('click', () => {
        closeSidebar();
    });
}

function openSidebar()
{
    const outsideButton = document.getElementById('outside-sidebar-button');
    outsideButton.hidden = true

    const sidebar = document.getElementById('sidebar');
    sidebar.classList.remove('sidebar-closed');  
    sidebar.classList.add('sidebar-opened');  
}

function closeSidebar()
{         
    const outsideButton = document.getElementById('outside-sidebar-button');
    outsideButton.hidden = false

    const sidebar = document.getElementById('sidebar');

    sidebar.classList.remove('sidebar-opened');  
    sidebar.classList.add('sidebar-closed');  
}

// [Time]

function updateTime() {
    const now     = new Date();
    const hours   = String(now.getHours()).padStart(2, '0');
    const minutes = String(now.getMinutes()).padStart(2, '0');
    const seconds = String(now.getSeconds()).padStart(2, '0');
    document.getElementById('sidebar-time').textContent = `${hours}:${minutes}:${seconds}`;
}

// [Version]

function updateVersion() {
    document.getElementById('sidebar-version').textContent = '1.0.0';
}

// [Company]

function updateCompany() {
    document.getElementById('sidebar-company').textContent = 'Fazenda do Seu João';
}

function startWebSocket() {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7274/weather-monitoring", {
            headers: { 
                "Accept-Language": "pt-BR" 
            },   
            withCredentials: true
        })
        .build();
 
    connection.on("weather-monitoring", function (weatherStatus) {

        const {
            status: {
                date,
                status,
                temperature,
                humidity,
                wind,
                uvIndex,
                visibility,
                precipitation,
                airPressure
            }
        } = weatherStatus;

        console.log(weatherStatus)

        var tableBody = document.getElementById('content-table').getElementsByClassName('content-table-body')[0];

        // [Row]
        var newRow = tableBody.insertRow();
        
        // [Cells]
        var dateCell        = newRow.insertCell();
        var statusCell      = newRow.insertCell();
        var temperatureCell = newRow.insertCell();
        var humidityCell    = newRow.insertCell();
        
        var dateText = document.createTextNode(date.toString());
        dateCell.appendChild(dateText);
        
        var statusText = document.createTextNode(status.toString());
        statusCell.appendChild(statusText);
        
        // [Temperature]
        var temperatureContainer       = document.createElement('div');
        temperatureContainer.className = 'content-table-temperature';

        // [Create the title div]
        var temperatureTitleDiv          = document.createElement('div');
        temperatureTitleDiv.className    = 'content-table-temperature-title';
        var temperatureTitleText         = document.createElement('p');
        temperatureTitleDiv.appendChild(temperatureTitleText);

        // [Append the title div to the container]
        temperatureContainer.appendChild(temperatureTitleDiv);

        // [Create the variations div]
        var temperatureVariationsDiv       = document.createElement('div');
        temperatureVariationsDiv.className = 'content-table-temperature-variations';

        // [Create the Fahrenheit variation]
        var fahrenheitVariation         = document.createElement('p');
        fahrenheitVariation.className   = 'content-table-temperature-variations-left';
        fahrenheitVariation.textContent = `${temperature.fahrenheit.toString()}°F`;
        temperatureVariationsDiv.appendChild(fahrenheitVariation);

        // [Create the Celsius variation]
        var celsiusVariation         = document.createElement('p');
        celsiusVariation.className   = 'content-table-temperature-variations-right';
        celsiusVariation.textContent = `${temperature.celsius.toString()}°C`;
        temperatureVariationsDiv.appendChild(celsiusVariation);

        // [Append the variations div to the container]
        temperatureContainer.appendChild(temperatureVariationsDiv);

        // [Append the container to the temperature cell]
        temperatureCell.appendChild(temperatureContainer);

        var humidityText = document.createTextNode(`${humidity.percentage.toString()}%`);
        humidityCell.appendChild(humidityText);
    });

    connection.start().catch(err => console.error(err.toString()));
}

setInterval(updateTime, 1000);

configureSidebar();

updateTime();
updateVersion();
updateCompany();

startWebSocket();