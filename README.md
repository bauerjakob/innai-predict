# InnAi Predict 🎯

<p align="center">
    <img src="./images/screenshot.png" width="400"  />
</p>

Visit Website of InnAi 👉 <a href="https://innai.de">here</a> 👈

<hr>

## What is InnAi 🌊
InnAI is an attempt to predict water level values using precipitation data with a neural network.

## All InnAi Projects
<table>
    <tr>
        <th>Model</th>
        <th>Link</th>
        <th>Short description</th>
    </tr>
    <tr>
        <th>InnAi-Train 🏃</th>
        <td><a href="https://github.com/bauerjakob/innai-train">Click here to open</a> 👈</td>
        <td>AI models are trained there, and training data is prepared there.</td>
    </tr>
    <tr>
        <th>InnAi-Predict 🎯</th>
        <td>You are here 🏆</td>
        <td>Website for displaying and comparing results, with a server featuring a REST interface for data provision.</td>
    </tr>
    <tr>
        <th>InnAi-Production 🚀</th>
        <td><a href="https://github.com/bauerjakob/innai-production">👉 Click here to open</a></td>
        <td>Deployment of reverse proxy and services.</td>
    </tr>
    <tr>
        <th>InnAi-Results 👀</th>
        <td><a href="https://github.com/bauerjakob/innai-results">Click here to open 👈</a></td>
        <td>After the models have been trained, their performance must be tested on unknown data.</td>
    </tr>
</table>

## Architecture Overview
<p align="center">
    <img src="./images/architecture.svg" width="700" />
</p>

## InnAi Models

### Salmon Swirl
#### Model Input
<p align="center">
    <img src="./images/models/salmon_swirl/model_input.png" width="500"  />
</p>

#### Training Results
<table>
    <tr>
        <th>Loss graph</th>
        <th>Evaluation graph (normalized)</th>
    </tr>
    <tr>
        <td>
            <img src="./images/models/salmon_swirl/training_results/1.png" width="500"  />
        </td>
        <td>
            <img src="./images/models/salmon_swirl/training_results/2.png" width="500"  />
        </td>
    </tr>
    <tr>
        <td>
            <img src="./images/models/salmon_swirl/training_results/3.png" width="500"  />
        </td>
        <td>
            <p align="center">11.9380</p>
        </td>
    </tr>
    <tr>
        <th>Evaluation graph (denormalized)</th>
        <th>Average evaluation loss (denormalized)</th>
    </tr>
</table>

### Roach River
#### Model Input
<p align="center">
    <img src="./images/models/roach_river/model_input.png" width="500"  />
</p>

#### Training Results
<table>
    <tr>
        <th>Loss graph</th>
        <th>Evaluation graph (normalized)</th>
    </tr>
    <tr>
        <td>
            <img src="./images/models/roach_river/training_results/1.png" width="500"  />
        </td>
        <td>
            <img src="./images/models/roach_river/training_results/2.png" width="500"  />
        </td>
    </tr>
    <tr>
        <td>
            <img src="./images/models/roach_river/training_results/3.png" width="500"  />
        </td>
        <td>
            <p align="center">12.4296</p>
        </td>
    </tr>
    <tr>
        <th>Evaluation graph (denormalized)</th>
        <th>Average evaluation loss (denormalized)</th>
    </tr>
</table>

### Zander Zenith
#### Model Input
<p align="center">
    <img src="./images/models/zander_zenith/model_input.png" width="500"  />
</p>

#### Training Results
<table>
    <tr>
        <th>Loss graph</th>
        <th>Evaluation graph (normalized)</th>
    </tr>
    <tr>
        <td>
            <img src="./images/models/zander_zenith/training_results/1.png" width="500"  />
        </td>
        <td>
            <img src="./images/models/zander_zenith/training_results/2.png" width="500"  />
        </td>
    </tr>
    <tr>
        <td>
            <img src="./images/models/zander_zenith/training_results/3.png" width="500"  />
        </td>
        <td>
            <p align="center">12.2053</p>
        </td>
    </tr>
    <tr>
        <th>Evaluation graph (denormalized)</th>
        <th>Average evaluation loss (denormalized)</th>
    </tr>
</table>


## Installation
Just look in the <a href="./docker-compose.yaml">docker-compose.yaml</a> 🐳